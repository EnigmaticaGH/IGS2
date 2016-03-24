using UnityEngine;
using System.Collections;

public class CloudMover : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(200, transform.position.y, transform.position.z), (speed * Time.deltaTime) / 2);
        
	
	}
}
