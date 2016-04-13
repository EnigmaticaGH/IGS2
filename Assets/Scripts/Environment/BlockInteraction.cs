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
    private ParticleSystem starParticle;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem.MinMaxCurve mmc;
    private ParticleSystem.ShapeModule sm;
    private Color originalColor;
    private Color currentColor;
    public Color warning;
    public float lethalVelocity;
    private float vel;
    private float startRate;
    private float startSize;

    void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();
        em = starParticle.emission;
        mmc = em.rate;
        sm = starParticle.shape;
        originalColor = GetComponentInChildren<ParticleSystem>().startColor;
        currentColor = originalColor;
        startSize = starParticle.startSize;
        startRate = mmc.constantMax;
    }

    void Start()
    {
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
        vel = body.velocity.magnitude;
        if (transform.position.y < -50)
        {
            ResetImmediately();
        }
        if (!IsGrabbedBySomeoneElse)
        {
            float normalizedVelocity = Mathf.Clamp01(body.velocity.magnitude / lethalVelocity);
            starParticle.startColor = Color.Lerp(currentColor, warning, normalizedVelocity);
            starParticle.startSize = startSize;
            em.type = ParticleSystemEmissionType.Distance;
            mmc.constantMax = startRate;
            em.rate = mmc;
            sm.box = Vector3.one;
        }
        else
        {
            starParticle.startSize = startSize / 3;
            em.type = ParticleSystemEmissionType.Time;
            mmc.constantMax = startRate * 10;
            em.rate = mmc;
            sm.box = Vector3.one / 2;
        }
    }

	void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Player") && // it has to be player
            AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") != Ability.Status.ACTIVE && //player is not dashing
            c.relativeVelocity.magnitude > lethalVelocity && vel > lethalVelocity && //block is going fast enough
            (Mathf.Abs(transform.position.x - c.transform.position.x) < 0.5f || //block has to be a direct hit, not a graze
            Mathf.Abs(transform.position.y - c.transform.position.y) < 0.5f))
        {
            r = c.gameObject.GetComponent<Rigidbody>();
            Movement m = c.gameObject.GetComponent<Movement>();
            Vector3 playerPosition = c.gameObject.transform.position;
            Vector3 diff = (playerPosition - transform.position).normalized;
            Vector3 force = new Vector3(diff.x * Mathf.Abs(c.relativeVelocity.x) * 15, diff.y * Mathf.Abs(c.relativeVelocity.y) * 15 + 250);
            if (r.velocity.y > body.velocity.y) //player is not falling faster than the block itself (i.e. falling into a falling block)
                PushPlayer(m, force);
            return;
        }
        //else if (((c.collider.CompareTag("Player") && //has to be player who is moving fast enough
        //    AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.ACTIVE) || //dash ability must be active
        //    c.collider.CompareTag("Block") && body.velocity.magnitude == 0) && c.relativeVelocity.magnitude > lethalVelocity) //or it can be another block
        else if (c.collider.CompareTag("Player") && c.relativeVelocity.magnitude > lethalVelocity && //has to be player who is moving fast enough
            AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.ACTIVE)
        {
            Vector3 force = new Vector3((c.relativeVelocity.x / lethalVelocity) * 50, (c.relativeVelocity.y / lethalVelocity) * 50, 0);
            if (!float.IsInfinity(force.magnitude) && !float.IsNaN(force.magnitude))
            {
                Launch(force, Mathf.Abs(c.relativeVelocity.x) > 20);
            }
            return;
        }

        if (c.collider.CompareTag("Player") && !isShattering && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
            currentColor = c.gameObject.GetComponent<GrabBlock>().teamColor;
            starParticle.startColor = currentColor;
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
        r.MovePosition(r.transform.position + Vector3.up * 0.1f);
        r.AddForce(power);
        m.gameObject.GetComponent<DeathControl>().Hurt(1);
        r.velocity = Vector3.zero;
        m.Disable(2, true);
        starParticle.Emit(25);
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
        starParticle.startSize = 0;
        time = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        starParticle.startColor = originalColor;
        currentColor = originalColor;
        starParticle.startSize = startSize;
        starParticle.Emit(10);
        starParticle.Play();
        body.useGravity = false;
        body.isKinematic = true;
        isShattering = false;
    }

    public void SetParticleColor(Color teamColor)
    {
        currentColor = teamColor;
        starParticle.startColor = currentColor;
    }

    public bool IsGrabbedBySomeoneElse
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
