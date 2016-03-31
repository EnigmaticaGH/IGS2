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
    private Material blockMaterial;
    private Color blockColor;
    private ParticleSystem grabSystem;
    private ParticleSystem.EmissionModule grabSystemEmission;
    private bool isGrabbedBySomeoneElse;
    private IEnumerator reset;
    private Transform originalParent;
    private bool isShattering;

    void Start()
    {
        //lowGravity = SceneManager.LoadScene(6);
        lowGravity = SceneManager.GetActiveScene();
        grabSystem = GetComponent<ParticleSystem>();
        blockMaterial = GetComponent<MeshRenderer>().material;
        //blockColor = blockMaterial.color;
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        time = 0;
        isGrabbedBySomeoneElse = false;
        grabSystemEmission = grabSystem.emission;
        grabSystemEmission.enabled = false;
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
        if (transform.position.y < -50)
        {
            ResetImmediately();
        }
    }

	void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Player") && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") != Ability.Status.ACTIVE)
        {
            r = c.gameObject.GetComponent<Rigidbody>();
            Movement m = c.gameObject.GetComponent<Movement>();
            Vector3 playerPosition = c.gameObject.transform.position;
            float xDiff = Mathf.Abs(playerPosition.x - transform.position.x);
            if (xDiff < 0.5f && c.relativeVelocity.y > 10f)
            {
                //Block hit from top (crush)
                Debug.Log("Squish");
                Squish(m, body.velocity.sqrMagnitude);
            }
            else if (body.velocity.magnitude > 5)
            {
                Vector3 diff = (playerPosition - transform.position).normalized;
                Vector3 force = new Vector3(diff.x * Mathf.Abs(c.relativeVelocity.x) * 15, diff.y * Mathf.Abs(c.relativeVelocity.y) * 15 + 250);
                PushPlayer(m, force);
            }
        }
        else if (c.collider.CompareTag("Player") && c.relativeVelocity.magnitude > 20)
        {
            foreach(ContactPoint p in c.contacts)
            {
                Vector3 force = new Vector3(c.relativeVelocity.x * 50f, c.relativeVelocity.y * 50f, 0);
                Launch(force, Mathf.Abs(c.relativeVelocity.x) > 20);
            }
        }

        if (c.collider.CompareTag("Player") && !isShattering && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
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
        m.UseForceInstead(0.5f);
        time = 5;
    }

    void Squish(Movement m, float power)
    {
        if (m.State == Movement.MovementState.GROUND)
        {
            //squish the player
            m.gameObject.GetComponent<DeathControl>().Hurt(1);
        }
        else
        {
            r.AddForce(Vector3.down * 5 * power);
        }
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
        body.AddForce(force);
        body.useGravity = true;
        time += 5;
        reset = Reset();
        StartCoroutine(reset);
    }

    public void SetColor(Color c)
    {
        //blockMaterial.color = c;
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
            if (transform.parent == originalParent)
                time -= Time.deltaTime;
        }
        time = 0;
        grabSystemEmission.enabled = false;
        blockMaterial.color = blockColor;
        transform.position = startPosition;
        transform.rotation = startRotation;
        body.useGravity = false;
        body.isKinematic = true;
        isShattering = false;
        StopCoroutine(reset);
    }

    IEnumerator Shatter(float t)
    {
        SpriteRenderer s = transform.GetChild(0).GetComponent<SpriteRenderer>();
        isShattering = true;
        float maxt = t;
        //grabSystemEmission.enabled = true;
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
        grabSystemEmission.enabled = false;
        blockMaterial.color = blockColor;
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

    public ParticleSystem GrabParticleSystem
    {
        get { return grabSystem; }
    }
}
