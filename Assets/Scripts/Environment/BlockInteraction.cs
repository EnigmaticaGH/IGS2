using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlockInteraction : MonoBehaviour {


    public Scene lowGravity;
    private Quaternion startRotation;
    private Rigidbody body;
    private Rigidbody r;
    private Vector3 startPosition;
    private float time;
    private IEnumerator reset;
    private Transform originalParent;
    private bool isShattering;
    private ParticleSystem cloudParticle;
    private ParticleSystem.EmissionModule cloudParticleEM;
    private ParticleSystem.MinMaxCurve cloudParticleMMC;
    private ParticleSystem.ShapeModule cloudParticleSM;
    private ParticleSystem clouds;
    private ParticleSystem.ShapeModule cloudsSM;
    private Color originalColor;
    private Color currentColor;
    public Color warning;
    public float lethalVelocity;
    private Vector3 vel;
    private float startRate;
    private float startSize;
    private float pushMult;

    void Awake()
    {
        cloudParticle = transform.FindChild("STARs").GetComponent<ParticleSystem>();
        cloudParticleEM = cloudParticle.emission;
        cloudParticleMMC = cloudParticleEM.rate;
        cloudParticleSM = cloudParticle.shape;
        originalColor = cloudParticle.startColor;
        currentColor = originalColor;
        startSize = cloudParticle.startSize;
        startRate = cloudParticleMMC.constantMax;
        IsBeingThrown = false;
        pushMult = 10;
    }
    void Start()
    {
        clouds = transform.FindChild("Clouds 1(Clone)").GetComponent<ParticleSystem>();
        cloudsSM = clouds.shape;
        lowGravity = SceneManager.GetActiveScene();
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        time = 0;
        reset = Reset();
        startRotation = transform.rotation;
        originalParent = transform.parent;
        isShattering = false;

        if (lowGravity.name == "Level 4 - No Gravity!")
        {
            Debug.Log("Low Gravity loaded");
            body.isKinematic = false;
        }   
    }

    void FixedUpdate()
    {
        vel = body.velocity;
        if (transform.position.y < -50)
        {
            ResetImmediately();
        }
        if (!IsGrabbedBySomeoneElse)
        {
            if (!IsBeingThrown)
            {
                float normalizedVelocity = Mathf.Clamp01(body.velocity.magnitude / lethalVelocity);
                cloudParticle.startColor = Color.Lerp(currentColor, warning, normalizedVelocity);
                cloudParticle.startSize = startSize;
            }
            cloudParticleEM.type = ParticleSystemEmissionType.Distance;
            cloudParticleMMC.constantMax = startRate;
            cloudParticleEM.rate = cloudParticleMMC;
            cloudParticleSM.box = Vector3.one;
        }
        else
        {
            cloudParticle.startSize = startSize / 3;
            cloudParticleEM.type = ParticleSystemEmissionType.Time;
            cloudParticleMMC.constantMax = startRate * 10;
            cloudParticleEM.rate = cloudParticleMMC;
            cloudParticleSM.box = Vector3.one / 2;
        }
        cloudsSM.box = transform.localScale;
    }

	void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Player") && !c.gameObject.GetComponent<GrabBlock>().Invincible(gameObject) && // it has to be player
            AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.READY && //player is not dashing
            c.relativeVelocity.magnitude > lethalVelocity && vel.magnitude > lethalVelocity && //block is going fast enough
            (Mathf.Abs(transform.position.x - c.transform.position.x) < 0.5f || //block has to be a direct hit, not a graze
            Mathf.Abs(transform.position.y - c.transform.position.y) < 0.5f))
        {
            r = c.gameObject.GetComponent<Rigidbody>();
            Movement m = c.gameObject.GetComponent<Movement>();
            Vector3 playerPosition = c.gameObject.transform.position;
            Vector3 diff = (playerPosition - transform.position).normalized;
            Debug.Log(c.relativeVelocity);
            Vector3 force = new Vector3(-c.relativeVelocity.x * pushMult, -c.relativeVelocity.y * pushMult);
            PushPlayer(m, force);
            return;
        }
        else if (c.collider.CompareTag("Player") && c.relativeVelocity.magnitude > lethalVelocity && //has to be player who is moving fast enough
            AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.ACTIVE)
        {
            Vector3 force = new Vector3((c.relativeVelocity.x / lethalVelocity) * 100, (c.relativeVelocity.y / lethalVelocity) * 100, 0);
            if (!float.IsInfinity(force.magnitude) && !float.IsNaN(force.magnitude))
            {
                Launch(force, Mathf.Abs(c.relativeVelocity.x) > 20);
            }
            return;
        }

        if (c.collider.CompareTag("Player") && !isShattering && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
            currentColor = c.gameObject.GetComponent<GrabBlock>().teamColor;
            cloudParticle.startColor = currentColor;
            StartCoroutine(Shatter(0.5f));
        }
    }

    void Launch(Vector3 force, bool isSidewaysLaunch)
    {
        body.useGravity = true;
        body.isKinematic = false;
        if (isSidewaysLaunch)
            body.MovePosition(transform.position + Vector3.up * 0.2f);
        body.AddForce(force);
        time += 5;
        reset = Reset();
        StartCoroutine(reset);
    }

    void PushPlayer(Movement m, Vector3 power)
    {
        float damage = power.magnitude / pushMult / 100;
        Vector3 force = power * (1 + m.gameObject.GetComponent<DeathControl>().Damage);
        Debug.Log(force + " | " + power);
        r.MovePosition(r.transform.position + Vector3.up * 0.1f);
        r.AddForce(force);
        if (power.y < -lethalVelocity * pushMult)
        {
            m.Disable(2, true);
            cloudParticle.Emit(25);
            body.AddForce(-power / 2);
            GetComponent<Collider>().enabled = false;
            r.velocity = Vector3.zero;
        }
        else
        {
            m.UseForceInstead(0.5f);
            r.velocity = Vector3.zero;
        }
        m.gameObject.GetComponent<DeathControl>().AddDamage(damage);
        time = 5;
    }

    public void Throw(Vector3 force, float respawnTime, Color color)
    {
        body.isKinematic = false;
        body.useGravity = true;
        body.AddForce(force);
        time = respawnTime;
        reset = Reset();
        StartCoroutine(reset);
    }

    public void Explode(Vector3 force)
    {
        body.isKinematic = false;
        body.useGravity = true;
        body.AddForce(force);
        time += 5;
        reset = Reset();
        StartCoroutine(reset);
    }

    public void Respawn(float t)
    {
        if ((time += t) <= t)
        {
            reset = Reset();
            StartCoroutine(reset);
        }
    }

    IEnumerator Reset()
    {
        while (time >= 0)
        {
            yield return new WaitForFixedUpdate();
            if (transform.parent == originalParent && body.velocity.sqrMagnitude < 1)
            {
                time -= Time.deltaTime;
            }
        }
        ResetImmediately();
        StopCoroutine(reset);
    }

    IEnumerator Shatter(float t)
    {
        SpriteRenderer s = transform.FindChild("Square(Clone)").GetComponent<SpriteRenderer>();
        isShattering = true;
        float maxt = t;
        Color c = currentColor;
        while ((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;
            s.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        body.useGravity = true;
        body.isKinematic = false;
        time = 5;
        t = 0.5f;
        while ((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;
            s.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1, 0, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        reset = Reset();
        StartCoroutine(reset);
    }

    public void ResetImmediately()
    {
        IsBeingThrown = false;
        cloudParticle.startSize = 0;
        time = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        cloudParticle.startColor = originalColor;
        currentColor = originalColor;
        cloudParticle.startSize = startSize;
        cloudParticle.Emit(10);
        cloudParticle.Play();
        body.useGravity = false;
        body.isKinematic = true;
        isShattering = false;
        GetComponent<Collider>().enabled = true;
    }

    public void SetParticleColor(Color teamColor)
    {
        currentColor = teamColor;
        cloudParticle.startColor = currentColor;
    }

    public bool IsGrabbedBySomeoneElse
    {
        get;
        set;
    }

    public bool IsBeingThrown
    {
        get;
        set;
    }

    public Quaternion StartRotation
    {
        get { return startRotation; }
        set { startRotation = StartRotation; }
    }
}
