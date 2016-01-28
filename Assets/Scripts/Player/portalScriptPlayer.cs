using UnityEngine;
using System.Collections;

public class portalScriptPlayer : MonoBehaviour {

    public Rigidbody player;
    RigidbodyConstraints orginialConstranints;
    public bool portalTimer = false;
    //private bool isGrounded;

    public delegate void PortalInit(string[] names, string name);
    public static event PortalInit PortalInitEvent;
    public delegate void PortalStatus(string sender, string status, int index, float time);
    public static event PortalStatus PortalStatusEvent;

    void Awake()
    {
        orginialConstranints = player.constraints;
    }

	// Use this for initialization
	void Start () {
        PortalInitEvent(new string[] { "Portal" }, name);
        PortalStatusEvent(name, "Ready", 0, 0);
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
            //Debug.LogError("Collsion Detected from player to portal");

            player.velocity = Vector3.zero;

            player.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            portalTimer = false;



            //Debug.LogError("Teleport me now!!!!!");
            PortalStatusEvent(name, "Active", 0, 0);
            Invoke("Teleport", .5f);
            Invoke("TimerReset", 3f);
        }
    }

    void Teleport()
    {
    
        player.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        //Debug.LogError("TELEPORTATION DEVICE CALLED");
        transform.position = new Vector3(transform.position.x + 4, transform.position.y, 0);
        //Debug.LogError(player.velocity);
        Invoke("playerReset", .2f);
        PortalStatusEvent(name, "Cooldown", 0, 2.5f);
    }

    void TimerReset()
    {
        portalTimer = true;
        PortalStatusEvent(name, "Ready", 0, 0);
    }

    void playerReset()
    {
        player.constraints = orginialConstranints;
    }
    
}
