using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CharacterMenuController : MonoBehaviour {

    public string[] ActiveControllers;
    public static int ControllerNumber;
    public Image[] CharacterPictures;
    public Text[] CharacterNames;
    public string[] Names;
    //GameObject[] InvCharacters;
    public GameObject[] pictureLocations;
    public GameObject[] playerCursors;
    public GameObject[] NextButtons;
    public Sprite[] NextButtonsHover;
    public Sprite[] NextButtonClick; //Pointless
    public Text[] NextText;

    //private Sprite[] original;

    public GameObject[] Characters;

    public GameObject[] PlayerPrefabs;

    public static int p1Pos = 0;
    public static int p2Pos = 0;
    public static int p3Pos = 0;
    public static int p4Pos = 0;

    private Sprite[] original = new Sprite[4];
    int i = 0;

    void OnEnable()
    {
        CursorMovement.p1_Next += ButtonClickP1;
        CursorMovement.p2_Next += ButtonClickP2;
        CursorMovement.p3_Next += ButtonClickP3;
        CursorMovement.p4_Next += ButtonClickP4;
        CursorMovement.p1_Hover += ButtonHoverP1;
        CursorMovement.p2_Hover += ButtonHoverP2;
        CursorMovement.p3_Hover += ButtonHoverP3;
        CursorMovement.p4_Hover += ButtonHoverP4;
        CursorMovement.p1_HoverExit += ButtonHoverExitP1;
        CursorMovement.p2_HoverExit += ButtonHoverExitP2;
        CursorMovement.p3_HoverExit += ButtonHoverExitP3;
        CursorMovement.p4_HoverExit += ButtonHoverExitP4;
    }

    void OnDisable()
    {
        CursorMovement.p1_Next -= ButtonClickP1;
        CursorMovement.p2_Next -= ButtonClickP2;
        CursorMovement.p3_Next -= ButtonClickP3;
        CursorMovement.p4_Next -= ButtonClickP4;
        CursorMovement.p1_Hover -= ButtonHoverP1;
        CursorMovement.p2_Hover -= ButtonHoverP2;
        CursorMovement.p3_Hover -= ButtonHoverP3;
        CursorMovement.p4_Hover -= ButtonHoverP4;
        CursorMovement.p1_HoverExit -= ButtonHoverExitP1;

    }

    void Awake()
    {
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

        for (int i = 0; i < NextButtons.Length; i++)
        {
            original[i] = NextButtons[i].GetComponent<Button>().image.overrideSprite;
        }
        //Switch next button colors
        /*ColorBlock temp;
        temp = NextButtons[0].GetComponent<Button>().colors;
        temp.normalColor = Color.red;
        NextButtons[0].GetComponent<Button>().colors = temp;
        NextText[0].color = Color.green;

        temp = NextButtons[1].GetComponent<Button>().colors;
        temp.normalColor = Color.blue;
        NextButtons[1].GetComponent<Button>().colors = temp;
        NextText[1].color = Color.yellow;

        temp = NextButtons[2].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        NextButtons[2].GetComponent<Button>().colors = temp;
        NextText[2].color = Color.magenta;

        temp = NextButtons[3].GetComponent<Button>().colors;
        temp.normalColor = Color.green;
        NextButtons[3].GetComponent<Button>().colors = temp;
        NextText[3].color = Color.red;*/

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
        NextButtons[0].GetComponent<Button>().image.overrideSprite = NextButtonClick[0];
        switch (p1Pos)
        {
            case (0):
                CharacterNames[0].text = Names[p1Pos];
                break;
            case (1):
                CharacterNames[0].text = Names[p1Pos];
                break;
            case (2):
                CharacterNames[0].text = Names[p1Pos];
                break;
            case(3):
                CharacterNames[0].text = Names[p1Pos];
                break;
            default:
                break;
        }
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
        NextButtons[1].GetComponent<Button>().image.overrideSprite = NextButtonClick[1];
        switch (p2Pos)
        {
            case (0):
                CharacterNames[1].text = Names[p2Pos];
                break;
            case (1):
                CharacterNames[1].text = Names[p2Pos];
                break;
            case (2):
                CharacterNames[1].text = Names[p2Pos];
                break;
            case (3):
                CharacterNames[1].text = Names[p2Pos];
                break;
            default:
                break;
        }

    }
    public void ButtonClickP3()
    {
        p3Pos = p3Pos + 1;
        if (p3Pos > 1)
            p3Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        NextButtons[2].GetComponent<Button>().image.overrideSprite = NextButtonClick[2];
        switch (p3Pos)
        {
            case (0):
                CharacterNames[2].text = Names[p3Pos];
                break;
            case (1):
                CharacterNames[2].text = Names[p3Pos];
                break;
            case (2):
                CharacterNames[2].text = Names[p3Pos];
                break;
            case (3):
                CharacterNames[2].text = Names[p3Pos];
                break;
            default:
                break;
        }

    }
    public void ButtonClickP4()
    {
        p4Pos = p4Pos + 1;
        if (p4Pos > 1)
            p4Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        NextButtons[3].GetComponent<Button>().image.overrideSprite = NextButtonClick[3];

        switch (p4Pos)
        {
            case(0):
                CharacterNames[3].text = Names[p4Pos];
                break;
            case (1):
                CharacterNames[3].text = Names[p4Pos];
                break;
            case (2):
                CharacterNames[3].text = Names[p4Pos];
                break;
            case (3):
                CharacterNames[3].text = Names[p4Pos];
                break;
            default:
                break;
        }
    }

    void ButtonHoverP1()
    {
        NextButtons[0].GetComponent<Button>().image.overrideSprite = NextButtonsHover[0];
    }
    void ButtonHoverP2()
    {
        NextButtons[1].GetComponent<Button>().image.overrideSprite = NextButtonsHover[1];

    }
    void ButtonHoverP3()
    {
        NextButtons[2].GetComponent<Button>().image.overrideSprite = NextButtonsHover[2];

    }
    void ButtonHoverP4()
    {
        NextButtons[3].GetComponent<Button>().image.overrideSprite = NextButtonsHover[3];

    }

    void ButtonHoverExitP1()
    {
        NextButtons[0].GetComponent<Button>().image.overrideSprite = original[0];
    }
    void ButtonHoverExitP2()
    {
        NextButtons[1].GetComponent<Button>().image.overrideSprite = original[1];
    }
    void ButtonHoverExitP3()
    {
        NextButtons[2].GetComponent<Button>().image.overrideSprite = original[2];
    }
    void ButtonHoverExitP4()
    {
        NextButtons[3].GetComponent<Button>().image.overrideSprite = original[3];
    }
}
