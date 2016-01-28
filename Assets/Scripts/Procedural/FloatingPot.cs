using UnityEngine;
using System.Collections;

public class FloatingPot : MonoBehaviour {

    Vector3 startingPosition;

	// Use this for initialization
	void Start () {

        startingPosition = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = startingPosition + Vector3.up * Mathf.Sin(-Time.time / 3);
	
	}
}
