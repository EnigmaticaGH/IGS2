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
        if (c.collider.CompareTag("Player")
            && c.relativeVelocity.sqrMagnitude > 44
            && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") != Ability.Status.ACTIVE)
        {
            r = c.gameObject.GetComponent<Rigidbody>();
            Movement m = c.gameObject.GetComponent<Movement>();
            Vector3 playerPosition = c.gameObject.transform.position;
            if (playerPosition.y > transform.position.y && c.relativeVelocity.y < -6f)
            {
                //Block hit from bottom
                r.AddForce(Vector3.up * 5 * Mathf.Pow(body.velocity.y, 2));
            }
            if (playerPosition.y < transform.position.y && c.relativeVelocity.y > 12f)
            {
                //Block hit from top (crush)
                Squish(m, body.velocity.sqrMagnitude);
            }
            if (playerPosition.x > transform.position.x && c.relativeVelocity.x < -6f)
            {
                //Block hit from left side
                PushPlayer(true, m, body.velocity.sqrMagnitude);
            }
            if (playerPosition.x < transform.position.x && c.relativeVelocity.x > 6f)
            {
                //Block hit from right side
                PushPlayer(false, m, body.velocity.sqrMagnitude);
            }
        }

        if (c.collider.CompareTag("Player")
            && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockSmash") == Ability.Status.ACTIVE
            && c.relativeVelocity.magnitude > 10)
        {
            foreach(ContactPoint p in c.contacts)
            {
                //Debug.Log(p.point + " | " + transform.position);
                if (p.point.y < transform.position.y - 0.35f && c.relativeVelocity.y > 10)
                {
                    Debug.Log("Upsmash");
                    //player hits from under with high Y speed
                    Launch(Vector2.up * 1000f, false);
                }
                if (p.point.y > transform.position.y + 0.35f && c.relativeVelocity.y < -10)
                {
                    Debug.Log("Downsmash");
                    //player hits from on top with high Y speed
                    Launch(Vector2.down * 1000f, false);
                }
                if (p.point.x > transform.position.x + 0.25f && c.relativeVelocity.x < -10)
                {
                    Debug.Log("Leftsmash");
                    //player hits from the right
                    Launch(Vector2.left * 1000f, true);
                }
                if (p.point.x < transform.position.x - 0.25f && c.relativeVelocity.x > 10)
                {
                    Debug.Log("Rightsmash");
                    //player hits from left
                    Launch(Vector2.right * 1000f, true);
                }
            }
        }

        if (c.collider.CompareTag("Player") && !isShattering
            && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
            StartCoroutine(Shatter(0.5f));
        }
    }

    void Launch(Vector3 force, bool isSidewaysLaunch)
    {
        body.useGravity = true;
        body.isKinematic = false;
        //blockMaterial.color = new Color(1, blockColor.g, blockColor.b);
        if (isSidewaysLaunch)
            body.MovePosition(transform.position + Vector3.up * 0.2f);
        body.AddForce(force);
        time += 5;
        reset = Reset();
        StartCoroutine(reset);
    }

    void PushPlayer(bool right, Movement m, float power)
    {
        int direction = right ? 1 : -1;
        r.MovePosition(r.transform.position + Vector3.up * 0.1f);
        //r.useGravity = false;
        r.AddForce(Vector3.right * direction * power + Vector3.up * 20);
        m.UseForceInstead(0.5f);
        time += 5;
        //Invoke("SetGravity", 0.5f);
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

    void SetGravity()
    {
        //r.useGravity = true;
    }

    public void Throw(Vector3 force, float respawnTime, Color color)
    {
        body.isKinematic = false;
        body.useGravity = true;
        //blockMaterial.color = color;
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
        isShattering = true;
        float red = blockColor.r;
        float grn = blockColor.g;
        float blu = blockColor.b;
        float maxt = t;
        //grabSystemEmission.enabled = true;
        while ((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;

            blockMaterial.color = new Color(
                Mathf.Lerp(red, 1, (1 - normalizedTime)),
                Mathf.Lerp(grn, 0, (1 - normalizedTime)),
                Mathf.Lerp(blu, 0, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
            //grabSystem.startColor = blockMaterial.color;
        }
        blockMaterial.color = blockColor;
        body.useGravity = true;
        body.isKinematic = false;
        time += 5;
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
