using UnityEngine;
using System.Collections;

public class GameSetupController : MonoBehaviour {

    public string[] ActiveControllers;
    private int ControllerNumber;

	// Use this for initialization
	void Start () {

        ControllerNumber = 0;

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            //Debug.LogError(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("Xbox"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }

            ActiveControllers = Input.GetJoystickNames();

        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
