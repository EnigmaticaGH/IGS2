using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlockInteraction : MonoBehaviour {


    public Scene lowGravity;

    private Rigidbody body;
    private Rigidbody r;
    private Vector3 start;
    private float time;
    private Material blockMaterial;
    private Color blockColor;
    private bool isGrabbedBySomeoneElse;

    void Start()
    {
        //lowGravity = SceneManager.LoadScene(6);
        lowGravity = SceneManager.GetActiveScene();
        blockMaterial = GetComponent<MeshRenderer>().material;
        blockColor = blockMaterial.color;
        body = GetComponent<Rigidbody>();
        start = transform.position;
        time = 0;
        isGrabbedBySomeoneElse = false;

        if (lowGravity.name == "Level 4 - No Gravity!")
        {
            Debug.Log("Low Gravity loaded");
            body.isKinematic = false;
        }
        
    }

    void FixedUpdate()
    {
        if (transform.position.y < -100)
            Destroy(gameObject);
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
                r.AddForce(Vector3.up * 5 * body.velocity.sqrMagnitude);
            }
            if (playerPosition.y < transform.position.y && c.relativeVelocity.y > 6f)
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
                if (p.point.y < transform.position.y - 0.45f && c.relativeVelocity.y > 10)
                {
                    Debug.Log("Upsmash");
                    //player hits from under with high Y speed
                    Launch(Vector2.up * 1000f, false);
                }
                if (p.point.y > transform.position.y + 0.45f && c.relativeVelocity.y < -10)
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

        if (c.collider.CompareTag("Player")
            && AbilityRegistry.AbilityStatus(c.gameObject.name, "BlockDrop") == Ability.Status.ACTIVE)
        {
            StartCoroutine(Shatter(0.3f));
        }
    }

    void Launch(Vector3 force, bool isSidewaysLaunch)
    {
        body.useGravity = true;
        body.isKinematic = false;
        blockMaterial.color = new Color(1, blockColor.g, blockColor.b);
        if (isSidewaysLaunch)
            body.MovePosition(transform.position + Vector3.up * 0.2f);
        body.AddForce(force);
        time += 5;
        StartCoroutine(Reset());
    }

    void PushPlayer(bool right, Movement m, float power)
    {
        int direction = right ? 1 : -1;
        r.MovePosition(r.transform.position + Vector3.up * 0.1f);
        r.useGravity = false;
        r.AddForce(Vector3.right * direction * power);
        m.UseForceInstead(0.5f);
        time += 5;
        Invoke("SetGravity", 0.5f);
    }

    void Squish(Movement m, float power)
    {
        if (m.State == Movement.MovementState.GROUND)
        {
            //squish the player
            Debug.Log("Squish!");
            m.gameObject.GetComponent<DeathControl>().Hurt(1);
        }
        else
        {
            r.AddForce(Vector3.down * 5 * power);
        }
    }

    void SetGravity()
    {
        r.useGravity = true;
    }

    public void Throw(Vector3 force, float respawnTime, Color color)
    {
        body.isKinematic = false;
        body.useGravity = true;
        blockMaterial.color = color;
        body.AddForce(force);
        time += respawnTime;
        Debug.Log(time);
        StartCoroutine(Reset());
    }

    public void SetColor(Color c)
    {
        blockMaterial.color = c;
    }

    public void StopReset()
    {
        StopCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        while ((time -= Time.deltaTime) >= 0)
        {
            yield return new WaitForFixedUpdate();
        }
        blockMaterial.color = blockColor;
        transform.position = start;
        transform.rotation = Quaternion.identity;
        body.useGravity = false;
        body.isKinematic = true;
    }

    IEnumerator Shatter(float t)
    {
        float red = blockColor.r;
        float grn = blockColor.g;
        float blu = blockColor.b;
        float maxt = t;
        while((t -= Time.deltaTime) > 0)
        {
            float normalizedTime = t / maxt;

            blockMaterial.color = new Color(
                Mathf.Lerp(red, 1, (1 - normalizedTime)),
                Mathf.Lerp(grn, 0, (1 - normalizedTime)),
                Mathf.Lerp(blu, 0, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        body.useGravity = true;
        body.isKinematic = false;
        time += 5;
        StartCoroutine(Reset());
    }

    public bool IsGrabbedBySomeoneElse
    {
        get;
        set;
    }
}
