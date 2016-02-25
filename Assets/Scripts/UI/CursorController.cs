using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public GameObject[] Cursors;
    [SerializeField]private int ControllerNumber;
    public static int ControllerAmount = 0;
	// Use this for initialization
	void Start () {

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            Debug.LogError(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("360"))
            {
                ControllerNumber++;
                ControllerAmount++;
                Debug.Log("Controller " + ControllerNumber);
            }
            if (Input.GetJoystickNames()[j].Contains("XBOX"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }
            //ActiveControllers = Input.GetJoystickNames();

        }

        //ControllerAmount = ControllerNumber;

        for(int i = 0; i <= 3; i++)
        {
            Renderer temp = Cursors[i].GetComponent<Renderer>();
            Cursors[i].SetActive(false);
            switch (i)
            {
                case 0:
                    temp.material.color = Color.red; // Player 1 Red Cursor
                    break;
                case 1:
                    temp.material.color = Color.blue; // PLayer 2 Blue cursor
                    break;
                case 2:
                    temp.material.color = Color.yellow; // Player 3 Yellow Cursor
                    break;
                case 3:
                    temp.material.color = Color.green; //player 4 Green cursor
                    break;
                default:
                    break;



            }
        }

        for (int g = 0; g < ControllerNumber; g++)
        {
            Cursors[g].SetActive(true);


        }

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
