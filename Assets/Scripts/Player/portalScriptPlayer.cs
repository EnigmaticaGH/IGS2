using UnityEngine;
using System.Collections;

public class portalScriptPlayer : MonoBehaviour {

    public Rigidbody player;
    RigidbodyConstraints orginialConstranints;
    public bool portalTimer = false;
    //private bool isGrounded;

    void Awake()
    {
        orginialConstranints = player.constraints;
    }

	// Use this for initialization
	void Start () {

        portalTimer = true;
        //isGrounded = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if ((col.tag == "Portal") && (portalTimer))
        {
            portalTimer = false;

            player.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            //Debug.LogError("Teleport me now!!!!!");

            Invoke("Teleport", .5f);
            Invoke("TimerReset", 3f);
        }
    }

    void Teleport()
    {
    
        player.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        //Debug.LogError("TELEPORTATION DEVICE CALLED");
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0);
        //Debug.LogError(player.velocity);
        Invoke("playerReset", .2f);
    }

    void TimerReset()
    {
        portalTimer = true;
    }

    void playerReset()
    {
        player.constraints = orginialConstranints;
    }
    
}
