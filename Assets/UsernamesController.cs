using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsernamesController : MonoBehaviour {

    public InputField[] Usernames;
    private int ControllerNumber;

	// Use this for initialization
	void Start () {

        ControllerNumber = 0;
        

        /*for (int m = 0; m <= Usernames.Length; m++)
        {
            
        }*/

        /*for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetJoystickNames()[i].Contains("360"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }
        }*/
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
