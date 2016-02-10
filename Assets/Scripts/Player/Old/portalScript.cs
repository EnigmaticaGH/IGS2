using UnityEngine;
using System.Collections;

public class portalScript : MonoBehaviour {

    public GameObject Portal;
    public bool portalReset = false;

	// Use this for initialization
	void Start () {

        portalReset = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (((coll.tag == "Player") || (coll.tag == "Sensor")) && (portalReset == false))
        {
            Invoke("teleport", .5f); //Teleports portal
            Invoke("portalTimer", 3);

            //Debug.LogError("Collision Detected from portal to player");

            portalReset = true; //Just incase player goes back onto portal
        }
        if (coll.tag == "Player")
        {
            //Debug.LogError("Collision Detected from portal to player");

        }
    }

    void teleport()
    {
        transform.position = new Vector3(transform.position.x + 4, 0, 0);

        Portal.GetComponent<Renderer>().material.color = Color.magenta;
    }

    void portalTimer()
    {
        Destroy(Portal);

        portalReset = false ;
    }
}
