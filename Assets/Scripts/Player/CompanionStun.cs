using UnityEngine;
using System.Collections;

public class CompanionStun : MonoBehaviour {

    public bool companionHit = false;
    public GameObject stunVisual;
    GameObject temp;
    Rigidbody rb;
    RigidbodyConstraints rbOrginial;
    int collisionControl = 0;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        rbOrginial = rb.constraints;
        //temp = stunVisual;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
       
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Companion") && (GameObject.FindWithTag("Companion").GetComponent<companionScript>().companionStun))
        {
            collisionControl++;
            if (collisionControl == 1)
            {
                StartCoroutine(Stun());

                //Debug.Log("Collision Detected with companion");

                companionHit = true;
            }
           

            //Invoke("StunPlayer", 0.1f);

            //StunPlayer();
        }
        else if (col.CompareTag("Companion") && (GameObject.FindWithTag("Companion").GetComponent<companionScript>().moveTowardsPlayer))
        {
            //Debug.LogError("Collision detected but ignored");
            //This should never be called 
            //Unless the enemy collides with the companion without the ability for stun being activated
        }
    }

    IEnumerator Stun()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;

        GameObject Temp = Instantiate(stunVisual, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity) as GameObject;

        //Debug.Log("CoRoutine Freeze Player: Starting timer");

        yield return new WaitForSeconds(3);

        rb.constraints = rbOrginial;

        collisionControl = 0;

        Destroy(Temp);

        //Debug.Log("CoRoutine Finished");
    }

    void StunPlayer()
    {

        if (companionHit && GameObject.Find("Companion").GetComponent<companionScript>().companionStun)
        {
            Debug.LogError("Stun Player:" + companionHit);
            //rb.velocity = Vector3.zero;
        }
    }
}
