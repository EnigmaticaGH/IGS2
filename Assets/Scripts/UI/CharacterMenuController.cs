using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class CharacterMenuController : MonoBehaviour
{
    
    
    
    public static List<int> playerSize = new List<int>();

    public Image[] AButton;
    public Image[] ReadyStamp;
    public string[] ActiveControllers;
    public static int ControllerNumber;
    public Image[] CharacterPictures;
    public Text[] CharacterNames;
    public string[] Names;
    public GameObject[] pictureLocations;
    public GameObject[] playerCursors;
    public Image[] PlayerReady;
    public Image[] PlayerDC;
    public Text[] PressStart;
    public Image[] ogArrowUP;
    public Image[] ogArrowDOWN;
    public Sprite activeArrowUP;
    public Sprite activeArrowDOWN;
    Sprite ogSpriteUp;
    Sprite ogSpriteDown;

    public GameObject[] Characters;
    public GameObject StartButton;
    public Text loadingText;
    bool loadScene = false;

    public static int[] playerINDEX_Pos = new int[4];


    public static int p1Pos = 0;
    public static int p2Pos = 1;
    public static int p3Pos = 2;
    public static int length = 0;
    public static int p4Pos = 3;

    int stickResetP1 = 0;
    int AResetP1 = 0;
    int stickResetP2 = 0;
    int AResetP2 = 0;
    int stickResetP3 = 0;
    int AResetP3 = 0;
    int stickResetP4 = 0;
    int AResetP4 = 0;

    int startCount1 = 0;
    int startCount2 = 0;
    int startCount3 = 0;
    int startCount4 = 0;

    public int[] startCount = new int[4];

    public bool[] activePlayers = new bool[4];




    int z = 0;
    int j = 0;
    int r = 0;

    public bool p1Ready;
    public bool p2Ready;
    public bool p3Ready;
    public bool p4Ready;

    public bool[] playerReady = new bool[4];

    public AudioSource Audio;
    public AudioClip Click;
    public AudioClip Scroll;
    public AudioClip Back;
    public AudioClip StartFX;

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            for (int b = 0; b < 4; b++)
            {
                playerINDEX_Pos[b] = b;
            }
                p1Pos = 0;
            p2Pos = 1;
            p3Pos = 2;
            p4Pos = 3;
            ControllerNumber = 0;

            for (int j = 0; j < playerSize.Count; j++)
            {
                playerSize.Remove(1);
            }
        }

    }


    void Awake()
    {
        loadingText.enabled = false;
        playerReady = new bool[4];
        startCount = new int[4];
        p1Pos = 0;
        p2Pos = 1;
        p3Pos = 2;
        p4Pos = 3;
        //ActiveControllers[Input.GetJoystickNames().Length]
        ControllerNumber = 0;

        ogSpriteUp = ogArrowUP[0].sprite;
        ogSpriteDown = ogArrowDOWN[0].sprite;
    }

    // Use this for initialization
    void Start()
    {

        StartButton.SetActive(false);

        for (int i = 0; i < CharacterPictures.Length; i++)
        {
            CharacterPictures[i].sprite = Characters[i].GetComponentInChildren<SpriteRenderer>().sprite; //Awesome this works 
            CharacterNames[i].text = Names[i];
        }

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
            PressStart[i].enabled = true;
            AButton[i].GetComponent<Image>().gameObject.SetActive(false);
            activePlayers[i] = false;
        }


    }

    void Update()
    {
        //Debug.Log(playerSize.Count + "-____________________");
        for (int i = 1; i <= 4; i++)
        {
            Debug.Log(i);
            if (activePlayers[i - 1] == false)
            {
                if ((Input.GetButtonUp("Start_" + i) || Input.GetButtonUp("A_" + i)) && startCount[i - 1] == 0)
                {
                    Debug.Log(i);
                    startCount[i - 1]++;
                    PlayerDC[i - 1].enabled = false;
                    AButton[i - 1].gameObject.SetActive(true);
                    PressStart[i - 1].enabled = false;
                    AButton[i - 1].gameObject.SetActive(true);
                    activePlayers[i - 1] = true;
                    playerSize.Add(1);
                    Audio.clip = Click;
                    Audio.Play();
                    Invoke("ResetStart" + i, .25f);

                }
                if (Input.GetButtonUp("B_" + i) && startCount[i - 1] == 0)
                {
                    Audio.clip = Back;
                    Audio.Play();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
            }
            if (activePlayers[i - 1])
            {
                if (playerReady[i - 1] == false)
                {
                    PlayerReady[i - 1].enabled = false;
                    if ((Input.GetAxisRaw("L_YAxis_" + i) == 1 || Input.GetAxisRaw("DPad_YAxis_" + i) == -1) && (stickResetP1 == 0))
                    {
                        stickResetP1++;
                        //LeftStickUpP1();
                        Audio.clip = Scroll;
                        Audio.Play();
                        LeftStickUp(i);
                        ogArrowDOWN[i - 1].sprite = activeArrowDOWN;
                        Invoke("StickReset", .5f);
                    }

                    if ((Input.GetAxisRaw("L_YAxis_" + i) == -1 || Input.GetAxisRaw("DPad_YAxis_" + i) == 1) && (stickResetP1 == 0))
                    {
                        stickResetP1++;
                        Audio.clip = Scroll;
                        Audio.Play();
                        //LeftStickDownP1();
                        LeftStickDown(i);
                        ogArrowUP[i - 1].sprite = activeArrowUP;
                        Invoke("StickReset", .5f);
                    }

                    if ((Input.GetButtonDown("Start_" + i) || Input.GetButtonDown("B_" + i)) && startCount[i - 1] == 0)
                    {
                        Audio.clip = Back;
                        Audio.Play();
                        startCount[i - 1]++;
                        PlayerReady[i - 1].enabled = true;
                        PlayerDC[i - 1].enabled = true;
                        AButton[i - 1].gameObject.SetActive(false);
                        PressStart[i - 1].enabled = true;
                        AButton[i - 1].gameObject.SetActive(false);
                        activePlayers[i - 1] = false;
                        playerSize.Remove(1);
                        Invoke("ResetStart" + i, .5f);
                    }

                }

                if (Input.GetButtonDown("A_" + i) && (z <= AResetP1))
                {
                    Audio.clip = Click;
                    Audio.Play();
                    AResetP1++;
                    p1Ready = true;
                    ReadyStamp[i - 1].gameObject.SetActive(true);
                    PlayerReady[i - 1].enabled = true;
                    playerReady[i - 1] = true;
                    AButton[i - 1].gameObject.SetActive(false);
                    Debug.Log(i);

                    if (playerSize.Count == 1)
                        if (playerReady[0])
                            StartButton.SetActive(true);
                    if (playerSize.Count == 2)
                        if (playerReady[0] && playerReady[1])
                            StartButton.SetActive(true);
                    if (playerSize.Count == 3)
                        if (playerReady[0] && playerReady[1] && playerReady[2])
                            StartButton.SetActive(true);
                    if (playerSize.Count == 2)
                        if (playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3])
                            StartButton.SetActive(true);
                    
                    //Debug.Log("i == 1" + z + "Player 1 Ready = true" + p1Ready);
                    if (AResetP1 == 2)
                    {
                        Audio.clip = Click;
                        Audio.Play();
                        AButton[i - 1].gameObject.SetActive(true);
                        p1Ready = false;
                        ReadyStamp[i - 1].gameObject.SetActive(false);
                        PlayerReady[i - 1].enabled = false;
                        playerReady[i - 1] = false;
                        Invoke("AResetP1FC", .25f);
                        StartButton.SetActive(false);

                        //Debug.Log("i == 2" + z + "Player 1 Ready = false" + p1Ready);
                    }
                }

                if (playerReady[i - i])
                {
                    AButton[i - 1].gameObject.SetActive(false);
                    if (Input.GetButtonUp("B_" + i) && AResetP1 == 1)
                    {
                        Audio.clip = Back;
                        Audio.Play();
                        AResetP1++;
                        startCount1++;
                        Invoke("ResetStart1",.25f);
                        p1Ready = false;
                        ReadyStamp[i - 1].gameObject.SetActive(false);
                        PlayerReady[i - 1].enabled = false;
                        AButton[i - 1].gameObject.SetActive(true);
                        StartButton.SetActive(false);
                        playerReady[i - 1] = false;
                        Invoke("AResetP1FC", .25f);
                    }

                    if (Input.GetButtonDown("Start_" + i))
                    {
                        Audio.clip = StartFX;
                        Audio.Play();
                        loadingText.enabled = true;
                        StartCoroutine("LoadNewScene");
                        //SceneManager.LoadScene(2);
                    }

                }
            }
        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }
        #region OLD

        /*          if (activePlayers[0] == false)
            {
                if (Input.GetButtonDown("Start_1") && (startCount1 == 0))
                {
                    startCount1++;
                    PlayerReady[0].enabled = false;
                    PlayerDC[0].enabled = false;
                    AButton[0].gameObject.SetActive(true);
                    PressStart[0].enabled = false;
                    AButton[0].gameObject.SetActive(true);
                    activePlayers[0] = true;
                    playerSize.Add(1);
                    Invoke("ResetStart1", .25f);

                }
                if (Input.GetButtonDown("B_1"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
            }
        if (activePlayers[0])
        {
            if (p1Ready == false)
            {
                if ((Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == -1) && (stickResetP1 == 0))
                {
                    stickResetP1++;
                    LeftStickUpP1();
                    ogArrowDOWN[0].sprite = activeArrowDOWN;
                    Invoke("StickReset", .5f);
                }

                if ((Input.GetAxisRaw("L_YAxis_1") == -1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && (stickResetP1 == 0))
                {
                    stickResetP1++;
                    LeftStickDownP1();
                    ogArrowUP[0].sprite = activeArrowUP;
                    Invoke("StickReset", .5f);
                }

                if ((Input.GetButtonDown("Start_1") || Input.GetButtonDown("B_1")) && startCount1 == 0)
                {
                    startCount1++;
                    PlayerReady[0].enabled = true;
                    PlayerDC[0].enabled = true;
                    AButton[0].gameObject.SetActive(false);
                    PressStart[0].enabled = true;
                    AButton[0].gameObject.SetActive(false);
                    activePlayers[0] = false;
                    playerSize.Remove(0);
                    Invoke("ResetStart1", .25f);
                }

            }

            if (Input.GetButtonDown("A_1") && (z <= AResetP1))
            {
                AResetP1++;
                p1Ready = true;
                ReadyStamp[0].gameObject.SetActive(true);
                PlayerReady[0].enabled = true;
                StartButton.SetActive(true);
                //Debug.Log("i == 1" + z + "Player 1 Ready = true" + p1Ready);
                if (AResetP1 == 2)
                {
                    AButton[0].gameObject.SetActive(true);
                    p1Ready = false;
                    ReadyStamp[0].gameObject.SetActive(false);
                    PlayerReady[0].enabled = false;
                    Invoke("AResetP1FC", .25f);
                    StartButton.SetActive(false);
                    //Debug.Log("i == 2" + z + "Player 1 Ready = false" + p1Ready);
                }
            }

            if (p1Ready)
            {
                AButton[0].gameObject.SetActive(false);
                if (Input.GetButtonDown("B_1") && AResetP1 == 1)
                {
                    AResetP1++;
                    p1Ready = false;
                    ReadyStamp[0].gameObject.SetActive(false);
                    PlayerReady[0].enabled = false;
                    AButton[0].gameObject.SetActive(true);
                    StartButton.SetActive(false);
                    Invoke("AResetP1FC", .25f);
                }

                if (Input.GetButtonDown("Start_1"))
                {
                    SceneManager.LoadScene(2);
                }

            }
        }

        if (activePlayers[1] == false)
        {
            if ((Input.GetButtonDown("Start_2") || Input.GetButtonDown("B_2")) && startCount2 == 0)
            {
                startCount2++;
                PlayerReady[1].enabled = false;
                PlayerDC[1].enabled = false;
                AButton[1].gameObject.SetActive(true);
                PressStart[1].enabled = false;
                AButton[1].gameObject.SetActive(true);
                activePlayers[1] = true;
                playerSize.Add(0);
                Invoke("ResetStart2", .25f);

            }
        }

        if (activePlayers[1])
        {
            if (p2Ready)
            {
                AButton[1].gameObject.SetActive(false);
                if (Input.GetButtonDown("B_2") && AResetP2 == 1)
                {
                    AResetP2++;
                    p1Ready = false;
                    ReadyStamp[1].gameObject.SetActive(false);
                    PlayerReady[1].enabled = false;
                    AButton[1].gameObject.SetActive(true);
                    Invoke("AResetP2FC", .25f);
                }
            }

            if (p2Ready == false)
            {
                if ((Input.GetAxisRaw("L_YAxis_2") == 1 || Input.GetAxisRaw("DPad_YAxis_2") == -1) && (stickResetP2 == 0))
                {
                    stickResetP2++;
                    LeftStickUpP2();
                    ogArrowDOWN[1].sprite = activeArrowDOWN;
                    Invoke("StickReset", .5f);
                }

                if ((Input.GetAxisRaw("L_YAxis_2") == -1 || Input.GetAxisRaw("DPad_YAxis_2") == 1) && (stickResetP2 == 0))
                {
                    stickResetP2++;
                    LeftStickDownP2();
                    ogArrowUP[1].sprite = activeArrowUP;
                    Invoke("StickReset", .5f);
                }

                if (Input.GetButtonDown("Start_2") && startCount2 == 0)
                {
                    startCount2++;
                    PlayerReady[1].enabled = true;
                    PlayerDC[1].enabled = true;
                    AButton[1].gameObject.SetActive(false);
                    PressStart[1].enabled = true;
                    AButton[1].gameObject.SetActive(false);
                    activePlayers[1] = false;
                    playerSize.Remove(0);
                    Invoke("ResetStart2", .25f);
                }
            }

            if (Input.GetButtonDown("A_2") && (z <= AResetP2))
            {
                AResetP2++;
                p2Ready = true;
                PlayerReady[1].enabled = true;
                ReadyStamp[1].gameObject.SetActive(true);
                if (AResetP2 == 2)
                {
                    AButton[1].gameObject.SetActive(true);
                    p2Ready = false;
                    PlayerReady[1].enabled = false;
                    ReadyStamp[1].gameObject.SetActive(false);
                    Invoke("AResetP2FC", .5f);
                }
            }
        }

        if (activePlayers[2])
        {
            if (p3Ready)
            {
                AButton[2].gameObject.SetActive(false);
                if (Input.GetButtonDown("B_3") && AResetP3 == 1)
                {
                    AResetP3++;
                    p1Ready = false;
                    ReadyStamp[2].gameObject.SetActive(false);
                    PlayerReady[2].enabled = false;
                    AButton[2].gameObject.SetActive(true);
                    Invoke("AResetP3FC", .25f);
                }
            }

            if (p3Ready == false)
            {
                if ((Input.GetAxisRaw("L_YAxis_3") == 1 || Input.GetAxisRaw("DPad_YAxis_3") == -1) && (r == stickResetP3))
                {
                    stickResetP3++;
                    LeftStickUpP3();
                    ogArrowDOWN[2].sprite = activeArrowDOWN;
                    Invoke("StickReset", .5f);
                }

                if ((Input.GetAxisRaw("L_YAxis_3") == -1 || Input.GetAxisRaw("DPad_YAxis_3") == 1) && (r == stickResetP3))
                {
                    stickResetP3++;
                    LeftStickDownP3();
                    ogArrowUP[2].sprite = activeArrowUP;
                    Invoke("StickReset", .5f);
                }


                if (Input.GetButtonDown("Start_3") && startCount3 == 0)
                {
                    startCount3++;
                    PlayerReady[2].enabled = true;
                    PlayerDC[2].enabled = true;
                    AButton[2].gameObject.SetActive(false);
                    PressStart[2].enabled = true;
                    AButton[2].gameObject.SetActive(false);
                    activePlayers[2] = false;
                    playerSize.Remove(0);
                    Invoke("ResetStart3", .25f);
                }
            }

            if (Input.GetButtonDown("A_3") && (AResetP3 <= 2))
            {
                AResetP3++;
                p2Ready = true;
                PlayerReady[2].enabled = true;
                ReadyStamp[2].gameObject.SetActive(true);
                Debug.Log("i == 1" + z + "Player 3 Ready = true" + p3Ready);
                if (AResetP3 == 2)
                {
                    AButton[2].gameObject.SetActive(true);
                    p2Ready = false;
                    PlayerReady[2].enabled = false;
                    ReadyStamp[2].gameObject.SetActive(false);
                    Invoke("AResetP3FC", .5f);
                }
            }
        }

        if (activePlayers[2] == false)
        {
            if ((Input.GetButtonDown("Start_3") || Input.GetButtonDown("B_3")) && startCount3 == 0)
            {
                startCount3++;
                PlayerReady[2].enabled = false;
                PlayerDC[2].enabled = false;
                AButton[2].gameObject.SetActive(true);
                PressStart[2].enabled = false;
                AButton[2].gameObject.SetActive(true);
                playerSize.Add(0);
                Invoke("ResetStart3", .25f);
            }
        }


        if (activePlayers[3])
        {
            if (p4Ready)
            {
                AButton[3].gameObject.SetActive(false);
                if (Input.GetButtonDown("B_4") && AResetP4 == 1)
                {
                    AResetP4++;
                    p4Ready = false;
                    ReadyStamp[3].gameObject.SetActive(false);
                    PlayerReady[3].enabled = false;
                    AButton[3].gameObject.SetActive(true);
                    Invoke("AResetP4FC", .25f);
                }
            }
            if (p4Ready == false)
            {
                if ((Input.GetAxisRaw("L_YAxis_4") == 1 || Input.GetAxisRaw("DPad_YAxis_4") == -1) && (stickResetP4 == 0))
                {
                    stickResetP4++;
                    LeftStickUpP4();
                    ogArrowDOWN[3].sprite = activeArrowDOWN;
                    Invoke("StickReset", .5f);
                }

                if ((Input.GetAxisRaw("L_YAxis_4") == -1 || Input.GetAxisRaw("DPad_YAxis_4") == 1) && (stickResetP4 == 0))
                {
                    stickResetP4++;
                    LeftStickDownP4();
                    ogArrowUP[3].sprite = activeArrowUP;
                    Invoke("StickReset", .5f);
                }
                if (Input.GetButtonDown("Start_4") && startCount4 == 0)
                {
                    startCount4++;
                    PlayerReady[3].enabled = true;
                    PlayerDC[3].enabled = true;
                    AButton[3].gameObject.SetActive(false);
                    PressStart[3].enabled = true;
                    AButton[3].gameObject.SetActive(false);
                    activePlayers[3] = false;
                    playerSize.Remove(0);
                    Invoke("ResetStart4", .25f);
                }
            }

            if (Input.GetButtonDown("A_4") && (AResetP4 <= 2))
            {
                AResetP4++;
                p4Ready = true;
                PlayerReady[3].enabled = true;
                ReadyStamp[3].gameObject.SetActive(true);
                Debug.Log("i == 1" + z + "Player 4 Ready = true" + p4Ready);
                if (AResetP4 == 2)
                {
                    AButton[3].gameObject.SetActive(true);
                    p4Ready = false;
                    PlayerReady[3].enabled = false;
                    ReadyStamp[3].gameObject.SetActive(false);
                    Invoke("AResetP4FC", .5f);
                }
            }
        }

        if (activePlayers[3] == false)
        {
            if ((Input.GetButtonDown("Start_4") || Input.GetButtonDown("B_4")) && startCount4 == 0)
            {
                startCount4++;
                PlayerReady[3].enabled = false;
                PlayerDC[3].enabled = false;
                AButton[3].gameObject.SetActive(true);
                PressStart[3].enabled = false;
                AButton[3].gameObject.SetActive(true);
                playerSize.Add(0);
                Invoke("ResetStart4", .25f);
            }
        }

   */

        #endregion

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

    void AResetP1FC()
    {
        AResetP1 = 0;
    }
    void AResetP2FC()
    {
        AResetP2 = 0;
    }
    void AResetP3FC()
    {
        AResetP3 = 0;
    }
    void AResetP4FC()
    {
        AResetP4 = 0;
    }

    #region Old Functions


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

    #endregion

    void LeftStickDown(int i)
    {
        i = i - 1;
        playerINDEX_Pos[i] = playerINDEX_Pos[i] - 1;
        if (playerINDEX_Pos[i] < 0)
            playerINDEX_Pos[i] = Characters.Length - 1;
        CharacterPictures[i].sprite = Characters[playerINDEX_Pos[i]].GetComponentInChildren<SpriteRenderer>().sprite;
        CharacterNames[i].text = Names[playerINDEX_Pos[i]];

    }

    void LeftStickUp(int i)
    {
        i = i - 1;
        playerINDEX_Pos[i] = playerINDEX_Pos[i] + 1;
        if (playerINDEX_Pos[i] > Characters.Length - 1)
            playerINDEX_Pos[i] = 0;
        CharacterPictures[i].sprite = Characters[playerINDEX_Pos[i]].GetComponentInChildren<SpriteRenderer>().sprite;
        CharacterNames[i].text = Names[playerINDEX_Pos[i]];

    }

    void ResetStart1()
    {
        startCount[0] = 0;
    }
    void ResetStart2()
    {
        startCount[1] = 0;
    }
    void ResetStart3()
    {
        startCount[2] = 0;
    }
    void ResetStart4()
    {
        startCount[3] = 0;
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(.000001f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(2); //FIX THIS
        //SceneManager.LoadScene(scene);


        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done. //FIX THIS
        while (!async.isDone)
        {
            yield return null;
        }

    }


}