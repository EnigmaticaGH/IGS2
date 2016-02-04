using UnityEngine;
using System.Collections;

public class trap3FloatingIsland : MonoBehaviour {

    public float rightMax = 2;
    public float leftMax = 2;
    public float speed = .5F;
    public float waitTime = .5F;
    float startingLocationX;

	// Use this for initialization
	void Start () {

        rightMax = transform.position.x + rightMax;
        leftMax = transform.position.x - leftMax;
        StartCoroutine("movementController");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator movementController()
    {
        float step = speed * Time.deltaTime;
        startingLocationX = transform.position.x;
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + elevatorHeight, transform.position.z), step);

        while (transform.position.x < rightMax)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + rightMax, transform.position.y, transform.position.z), step);

            yield return null;
        }

        //Debug.Log("CoRoutine reached target location");

        yield return new WaitForSeconds(waitTime);

        while (transform.position.x > leftMax)
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - leftMax, transform.position.y, transform.position.z), step * 2);

            yield return null;

        }

        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine("movementController");
        //Debug.Log("CoRoutine Finished");
    }
}
