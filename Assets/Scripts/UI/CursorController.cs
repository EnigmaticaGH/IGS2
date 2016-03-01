using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public GameObject[] Cursors;
    private int ControllerNumber;
    public static int ControllerAmount = 0;
	// Use this for initialization

    void Awake()
    {
        
    }
	void Start () {

        ControllerNumber = CharacterMenuController.ControllerNumber;
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
