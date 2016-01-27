using UnityEngine;
using System.Collections;

public class portalScript : MonoBehaviour {

    public GameObject Portal;
    public bool portalReset = false;

	// Use this for initialization
	void Start () {

        portalReset = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if ((col.tag == "Player") && (portalReset))
        {
            Invoke("teleport", .5f); //Teleports portal
            Invoke("portalTimer", 3);

            portalReset = false; //Just incase player goes back onto portal
        }
    }

    void teleport()
    {
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0);

        Portal.GetComponent<Renderer>().material.color = Color.magenta;
    }

    void portalTimer()
    {
        Destroy(Portal);

        portalReset = true;
    }
}
