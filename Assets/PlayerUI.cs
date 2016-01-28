using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    public Text distanceText;
    public Text playersShield;
    public Text playersPortal;
    public GameObject[] abilites;

    float ShieldTime;
    float PortalTimer;

	// Use this for initialization
	void Start () {

        distanceText.text = "Distance:" + transform.position.x + "/100";

        ShieldTime = GameObject.Find("Companion").GetComponent<companionScript>().shieldDuration;

        //Instantiate(distanceText, new Vector3(92, 375, 0), Quaternion.identity);

        /*for (int i = 0; i <= abilites.Length; i++)
        {
            
        }*/
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void FixedUpdate()
    {
        distanceText.text = "Distance:" + Mathf.Round(transform.position.x) + "/100";

        
        //playersPortal.text = "Portal: "

        if (!GameObject.Find("Companion").GetComponent<companionScript>().portalActive)
        {
            PortalTimer = 3;
            playersPortal.text = "Portal: Ready";
            playersPortal.color = Color.white;
        }
        else
        {
            PortalTimer -= Time.deltaTime;
            playersPortal.text = "Portal: " + Mathf.Round(PortalTimer);
            playersPortal.color = Color.yellow;
            if (Mathf.Round(PortalTimer) == 2)
            {
                playersPortal.text = "Portal: " + Mathf.Round(PortalTimer) + " Teleported";
                playersPortal.color = Color.yellow;
            }
        }

        if (!GameObject.Find("Companion").GetComponent<companionScript>().playerShield)
        {
            playersShield.text = "Shield: Ready!";
            ShieldTime = GameObject.Find("Companion").GetComponent<companionScript>().shieldDuration;
            playersShield.color = Color.white;
        }
        else if(GameObject.Find("Companion").GetComponent<companionScript>().playerShield)
        {
            ShieldTime -= Time.deltaTime;
            playersShield.text = "Shield: " + Mathf.Round(ShieldTime);
            playersShield.color = Color.yellow;
        }
    }
}
