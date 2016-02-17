using UnityEngine;
using System.Collections;

public class BlockInteraction : MonoBehaviour {
    private Rigidbody body;
    private Rigidbody r;
    private bool freezing;
    private float startY;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        freezing = false;
        startY = transform.position.y;
    }

    void FixedUpdate()
    {
        if (transform.position.y < -100)
            Destroy(gameObject);
        /*if (transform.position.y < 0 && body.isKinematic == false && startY > 0.5f)
        {
            body.isKinematic = true;
            startY = transform.position.y;
        }*/
    }

	void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Player") && body.velocity.sqrMagnitude > 49f)
        {
            r = c.gameObject.GetComponent<Rigidbody>();
            Movement m = c.gameObject.GetComponent<Movement>();
            //c.gameObject.GetComponent<DeathControl>().Hurt(1);
            if (c.transform.position.y > transform.position.y && body.velocity.y > 4f)
            {
                r.AddForce(Vector3.up * 5 * body.velocity.sqrMagnitude);
            }
            if (c.transform.position.x > transform.position.x && body.velocity.x > 4f)
            {
                r.MovePosition(r.transform.position + Vector3.up * 0.1f);
                r.useGravity = false;
                r.AddForce(Vector3.right * body.velocity.sqrMagnitude);
                m.UseForceInstead(0.5f);
                Invoke("SetGravity", 0.5f);
            }
            if (c.transform.position.x < transform.position.x && body.velocity.x < -4f)
            {
                r.MovePosition(r.transform.position + Vector3.up * 0.1f);
                r.useGravity = false;
                r.AddForce(Vector3.left * body.velocity.sqrMagnitude);
                m.UseForceInstead(0.5f);
                Invoke("SetGravity", 0.5f);
            }

        }

        /*if (c.collider.CompareTag("Player") && body.velocity.y < -7f)
        {
            Debug.Log(c.relativeVelocity.y);
            //if (c.relativeVelocity.y)
            c.gameObject.GetComponent<DeathControl>().Hurt(1);
        }*/

        if (c.collider.CompareTag("Player") && AbilityRegistry.AbilityStatus(c.gameObject.name, "Block Smash") == Ability.Status.ACTIVE && c.relativeVelocity.magnitude > 10)
        {
            foreach(ContactPoint p in c.contacts)
            {
                if (p.point.y < transform.position.y - 0.45f
                    //&& p.point.x < transform.position.x + 0.4f 
                    //&& p.point.x > transform.position.x - 0.4f 
                    && c.relativeVelocity.y > 10)
                {
                    body.useGravity = true;
                    body.isKinematic = false;
                    Debug.Log("Upsmash");
                    //player hits from under with high Y speed
                    body.AddForce(Vector2.up * 1000f);
                }
                if (p.point.y > transform.position.y + 0.45f 
                    //&& p.point.x < transform.position.x + 0.4f 
                    //&& p.point.x > transform.position.x - 0.4f 
                    && c.relativeVelocity.y < -10)
                {
                    body.useGravity = true;
                    body.isKinematic = false;
                    Debug.Log("Downsmash");
                    //player hits from on top with high Y speed
                    body.AddForce(Vector2.down * 1000f);
                }
                if (p.point.x > transform.position.x + 0.25f 
                    //&& p.point.y < transform.position.y + 0.4f
                    //&& p.point.y > transform.position.y - 0.4f 
                    && c.relativeVelocity.x < -10)
                {
                    body.useGravity = true;
                    body.isKinematic = false;
                    Debug.Log("Leftsmash");
                    //player hits from the right
                    body.MovePosition(transform.position + Vector3.up * 0.2f);
                    body.AddForce(Vector2.left * 1000f);
                }
                if (p.point.x < transform.position.x - 0.25f 
                    //&& p.point.y < transform.position.y + 0.4f
                    //&& p.point.y > transform.position.y - 0.4f
                    && c.relativeVelocity.x > 10)
                {
                    body.useGravity = true;
                    body.isKinematic = false;
                    Debug.Log("Rightsmash");
                    //player hits from left
                    body.MovePosition(transform.position + Vector3.up * 0.2f);
                    body.AddForce(Vector2.right * 1000f);
                }
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Block") || c.CompareTag("Ground") && !freezing)
        {
            freezing = true;
            Invoke("FreezeBlock", 1);
        }
    }

    void SetGravity()
    {
        r.useGravity = true;
    }

    void FreezeBlock()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.6f))
        {
            body.useGravity = false;
            body.isKinematic = true;
        }
        freezing = false;
    }
}
