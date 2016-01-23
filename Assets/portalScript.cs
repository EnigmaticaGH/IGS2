using UnityEngine;
using System.Collections;

public class portalScript : MonoBehaviour {

    public GameObject Portal;
    Renderer portalRenderer;
    Material colorRenderer;

	// Use this for initialization
	void Start () {
        portalRenderer = Portal.GetComponent<Renderer>();

        colorRenderer.color = Color.white;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.LogError("omg Dis The Player On Me!!!!1");

            Invoke("teleport", .5f); //Teleports portal
            Invoke("portalTimer", 3);
        }
    }

    void teleport()
    {
        transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0);
        portalRenderer.material = colorRenderer;
    }

    void portalTimer()
    {
        Destroy(Portal);
    }
}
