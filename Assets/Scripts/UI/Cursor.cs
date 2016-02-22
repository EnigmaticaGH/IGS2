using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
    private Rigidbody body;
    public float cursorSpeed;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontal = Input.GetAxis("L_XAxis_2");
        float vertical = -Input.GetAxis("L_YAxis_2");
        body.velocity = new Vector3(horizontal * cursorSpeed, vertical * cursorSpeed, 0);
    }
}
