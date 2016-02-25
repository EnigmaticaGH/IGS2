using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CharacterMenuController : MonoBehaviour {

    public string[] ActiveControllers;
    public static int ControllerNumber;
    public Image[] CharacterPictures;
    //GameObject[] InvCharacters;
    public GameObject[] pictureLocations;
    public GameObject[] playerCursors;
    public GameObject[] NextButtons;

    public GameObject[] Characters;

    public GameObject[] PlayerPrefabs;

    public static int p1Pos = 0;
    public static int p2Pos = 0;
    int p3Pos = 0;
    int p4Pos = 0;


    int i = 0;

    void OnEnable()
    {
        CursorMovement.p1_Next += ButtonClickP1;
        CursorMovement.p2_Next += ButtonClickP2;
        CursorMovement.p3_Next += ButtonClickP3;
        CursorMovement.p4_Next += ButtonClickP4;
    }

    void OnDisable()
    {
        CursorMovement.p1_Next -= ButtonClickP1;
        CursorMovement.p2_Next -= ButtonClickP2;
        CursorMovement.p3_Next -= ButtonClickP3;
        CursorMovement.p4_Next -= ButtonClickP4;



    }

	// Use this for initialization
	void Start () {

        //PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[0].GetComponentInChildren<SpriteRenderer>().sprite; //*****IMPORTANT FOR SETTING SPRITE

        //CharacterPictures[0].sprite = Characters[0];

        //Make the character picture image the first element in the characters array *******
        for (int i = 0; i < CharacterPictures.Length; i++)
        {
            CharacterPictures[i].sprite = Characters[0].GetComponentInChildren<SpriteRenderer>().sprite; //Awesome this works 
        }


            //ActiveControllers[Input.GetJoystickNames().Length]
            ControllerNumber = 0;

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            Debug.LogError(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("Xbox"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);                
            }

            if (Input.GetJoystickNames()[j].Contains("XBOX"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }

            ActiveControllers = Input.GetJoystickNames();
          
        }

        for (int k = 0; k < NextButtons.Length; k++)
        {
            NextButtons[k].SetActive(false);
        }

        for (int i = 0; i < playerCursors.Length; i++)
        {
           playerCursors[i].SetActive(false); //Set all cursors to false for now

        }

        for (int g = 0; g < ControllerNumber; g++)
        {
            playerCursors[g].SetActive(true);
            NextButtons[g].SetActive(true);

        }

        ColorBlock temp;
        temp = NextButtons[0].GetComponent<Button>().colors;
        temp.normalColor = Color.red;
        NextButtons[0].GetComponent<Button>().colors = temp;

        temp = NextButtons[0].GetComponent<Button>().colors;
        temp.normalColor = Color.blue;
        NextButtons[1].GetComponent<Button>().colors = temp;

       /* for (int f = 0; f < 4; f++)
        {
            CharacterPictures[0].SetActive(true);
            //Instantiate(CharacterPictures[0], new Vector3(pictureLocations[f].transform.position.x, pictureLocations[f].transform.position.y, pictureLocations[f].transform.position.z + 8), Quaternion.identity);
        }*/
	

	}

    void Update()
    {
      
    }

    public void ButtonClickP1()
    {
        p1Pos = p1Pos + 1;
        if (p1Pos > 3)
            p1Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        /*PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<PlayerAnim>().enabled = false;
        PlayerPrefabs[0].GetComponentInChildren<Animator>().enabled = false;*/
    }
    public void ButtonClickP2()
    {
        p2Pos = p2Pos + 1;
        if (p2Pos > 3)
            p2Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        /*PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<PlayerAnim>().enabled = false;
        PlayerPrefabs[0].GetComponentInChildren<Animator>().enabled = false;*/
    }
    public void ButtonClickP3()
    {
        p3Pos = p3Pos + 1;
        if (p3Pos > 1)
            p3Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<PlayerAnim>().enabled = false;
        PlayerPrefabs[0].GetComponentInChildren<Animator>().enabled = false;
    }
    public void ButtonClickP4()
    {
        p4Pos = p4Pos + 1;
        if (p4Pos > 1)
            p4Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<PlayerAnim>().enabled = false;
        PlayerPrefabs[0].GetComponentInChildren<Animator>().enabled = false;
    }
}
