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
    private MeshRenderer cube;
    private IEnumerator reset;
    private Transform originalParent;
    private bool isShattering;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem starParticle;
    private Color originalColor;
    public Color warning;
    public float lethalVelocity;
    private float vel;

    void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();
        originalColor = GetComponentInChildren<ParticleSystem>().startColor;
        cube = GetComponent<MeshRenderer>();
        em = starParticle.emission;
        em.enabled = false;
    }

    void Start()
    {
        lowGravity = SceneManager.GetActiveScene();
        //blockMaterial = GetComponent<MeshRenderer>().material;
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        time = 0;
        //isGrabbedBySomeoneElse = false;
        reset = Reset();
        startRotation = transform.rotation;
        originalParent = transform.parent;
        isShattering = false;
        warning = new Color(1, 0.4f, 0.25f, 1);

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
            float normalizedVelocity = Mathf.Clamp01(body.velocity.magnitude / lethalVelocity - 0.1f);
            starParticle.startColor = Color.Lerp(originalColor, warning, normalizedVelocity);
            cube.enabled = body.isKinematic;
            em.enabled = !body.isKinematic;
        }
        else
        {
            cube.enabled = false;
            em.enabled = true;
            starParticle.startColor = Color.white;
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
            PushPlayer(m, force);
            return;
        }
        else if (((c.collider.CompareTag("Player") && //has to be player who is moving fast enough
            AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.ACTIVE) || //dash ability must be active
            c.collider.CompareTag("Block") && body.velocity.magnitude == 0) && c.relativeVelocity.magnitude > lethalVelocity) //or it can be another block
        {
            Vector3 force = new Vector3((c.relativeVelocity.x / lethalVelocity) * 500, (c.relativeVelocity.y / lethalVelocity) * 500, 0);
            Launch(force, Mathf.Abs(c.relativeVelocity.x) > 20);
            return;
        }

        if (c.collider.CompareTag("Player") && !isShattering && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
            StartCoroutine(Shatter(0.5f));
        }
    }

    void Launch(Vector3 force, bool isSidewaysLaunch)
    {
        Debug.Log("Emission started");
        em.enabled = true;
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
        em.enabled = true;
        r.MovePosition(r.transform.position + Vector3.up * 0.1f);
        r.AddForce(power);
        m.UseForceInstead(0.5f);
        m.gameObject.GetComponent<DeathControl>().Hurt(1);
        time = 5;
    }

    public void Throw(Vector3 force, float respawnTime, Color color)
    {
        em.enabled = true;
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
        body.AddForce(force);
        body.useGravity = true;
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
        int count = 0;
        while (time >= 0)
        {
            yield return new WaitForFixedUpdate();
            if (transform.parent == originalParent && body.velocity.sqrMagnitude < 1)
            {
                time -= Time.deltaTime;
            }
            if (body.velocity.sqrMagnitude < 1 && em.enabled)
            {
                count++;
                if(count > 40)
                {
                    em.enabled = false;
                    cube.enabled = true;
                }
            }
            else
            {
                count = 0;
            }
        }
        em.enabled = false;
        cube.enabled = true;
        time = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        body.useGravity = false;
        body.isKinematic = true;
        isShattering = false;
        StopCoroutine(reset);
    }

    IEnumerator Shatter(float t)
    {
        SpriteRenderer s = transform.FindChild("Square(Clone)").GetComponent<SpriteRenderer>();
        isShattering = true;
        float maxt = t;
        while ((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;
            s.color = new Color(1, 0, 0, Mathf.Lerp(0, 1, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        body.useGravity = true;
        body.isKinematic = false;
        time = 5;
        t = 0.5f;
        while ((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;
            s.color = new Color(1, 0, 0, Mathf.Lerp(1, 0, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        reset = Reset();
        StartCoroutine(reset);
    }

    public void ResetImmediately()
    {
        body.useGravity = false;
        body.isKinematic = true;
        em.enabled = false;
        transform.rotation = startRotation;
        transform.position = startPosition;
        isShattering = false;
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
