using UnityEngine;
using System.Collections;

public class elevatorTrap : MonoBehaviour {

    public float elevatorWaitTime = 3;
    public float elevatorHeight = 5;
    public float speed;
    int startingLocationY;

	// Use this for initialization
	void Start () {

        StartCoroutine("elevatorTrapController");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator elevatorTrapController()
    {
        float step = speed * Time.deltaTime;
        startingLocationY = (int)transform.position.y;
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + elevatorHeight, transform.position.z), step);

        while (transform.position.y < elevatorHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + elevatorHeight, transform.position.z), step);

            yield return null;
        }

        //Debug.Log("CoRoutine reached target location");

        yield return new WaitForSeconds(elevatorWaitTime);

        while (transform.position.y > startingLocationY)
        {
        
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startingLocationY, transform.position.z), step);

             yield return null;
            
        }

        yield return new WaitForSeconds(elevatorWaitTime);
        yield return StartCoroutine("elevatorTrapController");
        Debug.Log("CoRoutine Finished");
    }
}
