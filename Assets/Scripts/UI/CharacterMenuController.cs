using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class CharacterMenuController : MonoBehaviour {

    public Image[] ReadyStamp;
    public string[] ActiveControllers;
    public static int ControllerNumber;
    public Image[] CharacterPictures;
    public Text[] CharacterNames;
    public string[] Names;
    //GameObject[] InvCharacters;
    public GameObject[] pictureLocations;
    public GameObject[] playerCursors;
    //public Text[] NextText;
    public Image[] PlayerReady;
    public Image[] PlayerDC;
    public Image[] ogArrowUP;
    public Image[] ogArrowDOWN;
    public Sprite activeArrowUP; 
    public Sprite activeArrowDOWN;
    Sprite ogSpriteUp; 
    Sprite ogSpriteDown;
    //private Sprite[] original;

    public GameObject[] Characters;

    public GameObject[] PlayerPrefabs;

    public static int p1Pos = 0;
    public static int p2Pos = 0;
    public static int p3Pos = 0;
    public static int p4Pos = 0;

    private Sprite[] original = new Sprite[4];

    int stickResetP1 = 0;
    int AResetP1 = 0;
    int stickResetP2 = 0;
    int AResetP2 = 0;
    int stickResetP3 = 0;
    int AResetP3 = 0;
    int stickResetP4 = 0;
    int AResetP4 = 0;


    int z = 0;
    int j = 0;
    int r = 0;

    public bool p1Ready;
    public bool p2Ready;
    public bool p3Ready;
    public bool p4Ready;

    void OnEnable()
    {
        //CursorMovement.p1_Next += ButtonClickP1;
        //CursorMovement.p2_Next += ButtonClickP2;
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
        //CursorMovement.p1_Next -= ButtonClickP1;
        //CursorMovement.p2_Next -= ButtonClickP2;
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

        ogSpriteUp = ogArrowUP[0].sprite;
        ogSpriteDown = ogArrowDOWN[0].sprite;


            for (int j = 0; j < Input.GetJoystickNames().Length; j++)
            {

                //Debug.LogError(Input.GetJoystickNames()[j]);
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
            //Debug.Log(ControllerNumber);

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

        /*for (int k = 0; k < NextButtons.Length; k++)
        {
            NextButtons[k].SetActive(false);
        }*/

        for (int i = 0; i < playerCursors.Length; i++)
        {
           playerCursors[i].SetActive(false); //Set all cursors to false for now

        }

        for (int i = 0; i < ReadyStamp.Length; i++)
        {
            ReadyStamp[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerDC.Length; i++)
        {
            PlayerDC[i].enabled = true;
            PlayerReady[i].enabled = false;
        }

            for (int g = 0; g < ControllerNumber; g++)
            {
                playerCursors[g].SetActive(true);
                //NextButtons[g].SetActive(true);
                PlayerReady[g].enabled = false;
                PlayerDC[g].enabled = false;
            }


        /*for (int i = 0; i < NextButtons.Length; i++)
        {
            original[i] = NextButtons[i].GetComponent<Button>().image.overrideSprite;
        }*/
	}

    void Update()
    {
        if (p1Ready == false)
        {
            if ((Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && (j == stickResetP1))
            {
                stickResetP1++;
                LeftStickDownP1();
                ogArrowDOWN[0].sprite = activeArrowDOWN;
                Invoke("StickReset", .5f);
            }

            if ((Input.GetAxisRaw("L_YAxis_1") == -1 || Input.GetAxisRaw("DPad_YAxis_1") == -1) && (j == stickResetP1))
            {
                stickResetP1++;
                LeftStickUpP1();
                ogArrowUP[0].sprite = activeArrowUP;
                Invoke("StickReset", .5f);
            }
        }

        if (Input.GetButtonDown("A_1") && (z <= AResetP1))
        {
            AResetP1++;
            p1Ready = true;
            ReadyStamp[0].gameObject.SetActive(true);
            PlayerReady[0].enabled = true;
            //Debug.Log("i == 1" + z + "Player 1 Ready = true" + p1Ready);
            if (z == 2) 
            {
                p1Ready = false;
                ReadyStamp[0].gameObject.SetActive(false);
                PlayerReady[0].enabled = false;
                Invoke("AReset", .5f);
                //Debug.Log("i == 2" + z + "Player 1 Ready = false" + p1Ready);
            }
        }

        if (p2Ready == false)
        {
            if ((Input.GetAxisRaw("L_YAxis_2") == 1 || Input.GetAxisRaw("DPad_YAxis_2") == 1) && (r == stickResetP2))
            {
                stickResetP2++;
                LeftStickDownP2();
                ogArrowDOWN[1].sprite = activeArrowDOWN;
                Invoke("StickReset", .5f);
            }

            if ((Input.GetAxisRaw("L_YAxis_2") == -1 || Input.GetAxisRaw("DPad_YAxis_2") == -1) && (r == stickResetP2))
            {
                stickResetP2++;
                LeftStickUpP2();
                ogArrowUP[1].sprite = activeArrowUP;
                Invoke("StickReset", .5f);
            }
        }

        if (Input.GetButtonDown("A_2") && (z <= AResetP2))
        {
            AResetP2++;
            p2Ready = true;
            PlayerReady[1].enabled = true;
            ReadyStamp[1].gameObject.SetActive(true);
            if (z == 2)
            {
                p2Ready = false;
                PlayerReady[1].enabled = false;
                ReadyStamp[1].gameObject.SetActive(false);
                Invoke("AReset", .5f);
            }
        }

        if (p3Ready == false)
        {
            if ((Input.GetAxisRaw("L_YAxis_3") == 1 || Input.GetAxisRaw("DPad_YAxis_3") == 1) && (r == stickResetP3))
            {
                stickResetP3++;
                LeftStickDownP3();
                ogArrowDOWN[2].sprite = activeArrowDOWN;
                Invoke("StickReset", .5f);
            }

            if ((Input.GetAxisRaw("L_YAxis_3") == -1 || Input.GetAxisRaw("DPad_YAxis_3") == -1) && (r == stickResetP3))
            {
                stickResetP3++;
                LeftStickUpP3();
                ogArrowUP[2].sprite = activeArrowUP;
                Invoke("StickReset", .5f);
            }
        }

        if (Input.GetButtonDown("A_3") && (AResetP3 <= 2))
        {
            AResetP3++;
            p2Ready = true;
            PlayerReady[2].enabled = true;
            ReadyStamp[2].gameObject.SetActive(true);
            Debug.Log("i == 1" + z + "Player 3 Ready = true" + p3Ready);
            if (z == 2)
            {
                p2Ready = false;
                PlayerReady[2].enabled = false;
                ReadyStamp[2].gameObject.SetActive(false);
                Invoke("AReset", .5f);            }
        }

        if (p4Ready == false)
        {
            if ((Input.GetAxisRaw("L_YAxis_4") == 1 || Input.GetAxisRaw("DPad_YAxis_4") == 1) && (stickResetP4 == 0))
            {
                stickResetP4++;
                LeftStickDownP4();
                ogArrowDOWN[3].sprite = activeArrowDOWN;
                Invoke("StickReset", .5f);
            }

            if ((Input.GetAxisRaw("L_YAxis_4") == -1 || Input.GetAxisRaw("DPad_YAxis_4") == -1) && (stickResetP4 == 0))
            {
                stickResetP4++;
                LeftStickUpP4();
                ogArrowUP[3].sprite = activeArrowUP;
                Invoke("StickReset", .5f);
            }
        }

        if (Input.GetButtonDown("A_4") && (AResetP4 <= 2))
        {
            AResetP4++;
            p4Ready = true;
            PlayerReady[3].enabled = true;
            ReadyStamp[3].gameObject.SetActive(true);
            Debug.Log("i == 1" + z + "Player 4 Ready = true" + p4Ready);
            if (z == 2)
            {
                p4Ready = false;
                PlayerReady[3].enabled = false;
                ReadyStamp[3].gameObject.SetActive(false);
                Invoke("AReset", .5f);
            }
        }



        if (ControllerNumber == 1)
        {
            if (p1Ready)
            {
                if (Input.GetButtonDown("Start_1"))
                {
                    SceneManager.LoadScene(2);
                }
            }
        }

        if (p1Ready)
        {
            if (Input.GetButtonDown("Start_1"))
            {
                SceneManager.LoadScene(2);
            }
        }
        
    }

    void StickReset()
    {
        ogArrowUP[0].sprite = ogSpriteUp;
        ogArrowDOWN[0].sprite = ogSpriteDown;
        ogArrowUP[1].sprite = ogSpriteUp;
        ogArrowDOWN[1].sprite = ogSpriteDown;
        ogArrowUP[2].sprite = ogSpriteUp;
        ogArrowDOWN[2].sprite = ogSpriteDown;
        ogArrowUP[3].sprite = ogSpriteUp;
        ogArrowDOWN[3].sprite = ogSpriteDown;

        stickResetP1 = 0;
        stickResetP2 = 0;
        stickResetP3 = 0;
        stickResetP4 = 0;


    }

    void AReset()
    {
        AResetP1 = 0;
        AResetP2 = 0;
        AResetP3 = 0;
        AResetP4 = 0;
    }

    void LeftStickDownP1() 
    {
        p1Pos = p1Pos - 1;
        if (p1Pos < 0)
            p1Pos = Characters.Length - 1;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[0].text = Names[p1Pos];
        
    }

    void LeftStickUpP1()
    {
        p1Pos = p1Pos + 1;
        if (p1Pos > Characters.Length - 1)
            p1Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[0].text = Names[p1Pos];

    }
    void LeftStickUpP2()
    {
        p2Pos = p2Pos + 1;
        if (p2Pos > Characters.Length - 1)
            p2Pos = 0;
        CharacterPictures[1].sprite = Characters[p2Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[1].text = Names[p2Pos];


    }
    void LeftStickDownP2()
    {
        p2Pos = p2Pos - 1;
        if (p2Pos < 0)
            p2Pos = Characters.Length - 1;
        CharacterPictures[1].sprite = Characters[p2Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[1].text = Names[p2Pos];


    }

    void LeftStickUpP3()
    {
        p3Pos = p3Pos + 1;
        if (p3Pos > Characters.Length - 1)
            p3Pos = 0;
        CharacterPictures[2].sprite = Characters[p3Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[2].text = Names[p3Pos];

    }
    void LeftStickDownP3()
    {
        p3Pos = p3Pos - 1;
        if (p3Pos < 0)
            p3Pos = Characters.Length - 1;
        CharacterPictures[2].sprite = Characters[p3Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        CharacterNames[2].text = Names[p3Pos];
    }

    void LeftStickUpP4()
    {
        p4Pos = p4Pos + 1;
        if (p4Pos > Characters.Length - 1)
            p4Pos = 0;
        CharacterPictures[3].sprite = Characters[p4Pos].GetComponentInChildren<SpriteRenderer>().sprite;

        CharacterNames[3].text = Names[p4Pos];

    }
    void LeftStickDownP4()
    {
        p4Pos = p4Pos - 1;
        if (p4Pos < 0)
            p4Pos = Characters.Length - 1;
        CharacterPictures[3].sprite = Characters[p4Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        CharacterNames[3].text = Names[p4Pos];
    }

    public void ButtonClickP3()
    {
        p3Pos = p3Pos + 1;
        if (p3Pos > 1)
            p3Pos = 0;
        CharacterPictures[0].sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        PlayerPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite = Characters[p1Pos].GetComponentInChildren<SpriteRenderer>().sprite;
        //NextButtons[2].GetComponent<Button>().image.overrideSprite = NextButtonClick[2];
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
        //NextButtons[3].GetComponent<Button>().image.overrideSprite = NextButtonClick[3];

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
        //NextButtons[0].GetComponent<Button>().image.overrideSprite = NextButtonsHover[0];
    }
    void ButtonHoverP2()
    {
        //NextButtons[1].GetComponent<Button>().image.overrideSprite = NextButtonsHover[1];

    }
    void ButtonHoverP3()
    {
        //NextButtons[2].GetComponent<Button>().image.overrideSprite = NextButtonsHover[2];

    }
    void ButtonHoverP4()
    {
        //NextButtons[3].GetComponent<Button>().image.overrideSprite = NextButtonsHover[3];

    }

    void ButtonHoverExitP1()
    {
        //NextButtons[0].GetComponent<Button>().image.overrideSprite = original[0];
    }
    void ButtonHoverExitP2()
    {
        //NextButtons[1].GetComponent<Button>().image.overrideSprite = original[1];
    }
    void ButtonHoverExitP3()
    {
        //NextButtons[2].GetComponent<Button>().image.overrideSprite = original[2];
    }
    void ButtonHoverExitP4()
    {
        //NextButtons[3].GetComponent<Button>().image.overrideSprite = original[3];
    }
}
