using UnityEngine;
using System.Collections;

public class DevScript : MonoBehaviour {

    public GameObject DevField;

	// Use this for initialization
	void Start () {

        DevField.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointClick()
    {
        DevField.SetActive(true);
    }
}
