using UnityEngine;
using System.Collections;

public class shieldScript : MonoBehaviour {


    /*public Transform player;
    public GameObject shieldObject;
    bool shield = false;

	// Use this for initialization
	void Start () {

        shield = GameObject.Find("Companion").GetComponent<companionScript>().playerShield;

        InvokeRepeating("playerShieldCheck", 1, 1);
	
	}

    void playerShieldCheck()
    {
        Debug.LogError("Player Shield Check: " + GameObject.Find("Companion").GetComponent<companionScript>().playerShield);

        if (GameObject.Find("Companion").GetComponent<companionScript>().playerShield == false)
        {
            //Destroy(transform);
            //transform.position = new Vector3(0, -300, 0);
            //Destroy(shieldObject);
            shieldObject.SetActive(false);
        }
        else if (GameObject.Find("Companion").GetComponent<companionScript>().playerShield)
        {
            shieldObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {

        /*if (shield)
        {
            transform.position = player.transform.position;
            Debug.LogError("HIIIIIII");
        }
        else if (shield == false)
            Destroy(transform);
	
	}

    void FixedUpdate()
    {
        
    }*/
}
