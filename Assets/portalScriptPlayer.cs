using UnityEngine;
using System.Collections;

public class portalScriptPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Portal")
        {
            Debug.LogError("Teleport me now!!!!!");

            Invoke("Teleport", .5f);
        }
    }

    void Teleport()
    {
        /*
         
         ****** NEED TO ADJUST BASED ON PLAYER"S VELOCITY***********
         
         */

        Debug.LogError("TELEPORTATION DEVICE CALLED");
        transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
    }

}
