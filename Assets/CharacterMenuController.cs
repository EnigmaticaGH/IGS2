using UnityEngine;
using System.Collections;

public class CharacterMenuController : MonoBehaviour {

    public string[] ActiveControllers;
    private int ControllerNumber;
    public GameObject[] CharacterPictures;
    public GameObject[] pictureLocations;
    public GameObject[] playerCursors;
    public GameObject[] Buttons;

    int i = 0;

	// Use this for initialization
	void Start () {

        //ActiveControllers[Input.GetJoystickNames().Length];
        ControllerNumber = 0;

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            Debug.LogError(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("360"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);                
            }
            
        }

        for (int k = 0; k < Buttons.Length; k++)
        {
            Buttons[k].SetActive(false);
        }

        for (int i = 0; i < playerCursors.Length; i++)
        {
           playerCursors[i].SetActive(false); //Set all cursors to false for now

        }

        for (int g = 0; g < ControllerNumber; g++)
        {
            playerCursors[g].SetActive(true);
            Buttons[g].SetActive(true);

        }

        for (int f = 0; f < 4; f++)
        {
            CharacterPictures[0].SetActive(true);
            Instantiate(CharacterPictures[0], new Vector3(pictureLocations[f].transform.position.x, pictureLocations[f].transform.position.y, pictureLocations[f].transform.position.z + 8), Quaternion.identity);
        }
	

	}

    void Update()
    {
      
    }

    public void ButtonClickP1()
    {
        i++;
        if(i >= CharacterPictures.Length)
        {
            i = 0;
        }
    }
    public void ButtonClickP2()
    {
        i++;
        if (i >= CharacterPictures.Length)
        {
            i = 0;
        }
    }
    public void ButtonClickP3()
    {
        i++;
        if (i >= CharacterPictures.Length)
        {
            i = 0;
        }
    }
    public void ButtonClickP4()
    {
        i++;
        if (i >= CharacterPictures.Length)
        {
            i = 0;
        }
    }
}
