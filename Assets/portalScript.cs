using UnityEngine;
using System.Collections;

public class portalScript : MonoBehaviour {

    public GameObject Portal;
    public bool portalReset = false;
    Renderer portalRenderer;
    Material colorRenderer;

	// Use this for initialization
	void Start () {
        portalRenderer = Portal.GetComponent<Renderer>();
        //colorRenderer = Portal.GetComponent<Material>();

        //colorRenderer.color = Color.white;

        portalReset = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if ((col.tag == "Player") && (portalReset))
        {
            Debug.LogError("omg Dis The Player On Me!!!!1");

            Invoke("teleport", .5f); //Teleports portal
            Invoke("portalTimer", 3);

            portalReset = false; //Just incase player goes back onto portal
        }
    }

    void teleport()
    {
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0);
        //portalRenderer.material = colorRenderer;
        Portal.GetComponent<Renderer>().material.color = Color.magenta;
    }

    void portalTimer()
    {
        Destroy(Portal);

        portalReset = true;
    }
}
