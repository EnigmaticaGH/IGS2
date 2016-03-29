using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    public bool LoadScene = false;
    public Text StartText;
    public Image Highlight;
    public Image HighlightMode;
    public bool SelectedLevel = false;
    private bool SelectedGameMode = false;
    public bool Mode = false;
    public bool Time = false;
    public bool StartCD = false;
    public Text TimeText;
    public Text LevelText;
    public Image LevelImage;
    public Text ModeText;
    public Text DetailText;
    public string[] LevelNames = { "Cloudy", "Outback", "Mechanic" };
    public string[] Details = { "Throw clouds at each other!", "Hit up the desert or the saloon in this outback thriller!", "Wow!! lots of moving stuff cooooolllll!" };
    public Sprite[] LevelImages;
    public Transform LocationMODE;
    public Transform LocationTIME;
    public Transform LocationSTART;
    Vector3 startingLoc;
    public GameObject Arrow;

    int i = 0;
    public int placeHolder = 0;
    int selected = 0;
    GameModeController GameController;
    int ControllerSpot = 0;
    int cd = 0;
    int TimerSpot = 0;
    int tracker = 0;
    int mode = 0;
    int time = 0;
    Color textColor;

    // Use this for initialization
    void Start()
    {
        startingLoc = Arrow.transform.position;

        Highlight.gameObject.SetActive(false);

        //HighlightMode.gameObject.SetActive(false);

        LevelText = LevelText.GetComponent<Text>();

        LevelText.text = LevelNames[0];

        GameController = GameObject.Find("Game Mode").GetComponent<GameModeController>();

        textColor = TimeText.color;

        ModeText.color = Color.white;

        TimeText.color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        if (!SelectedGameMode)
        {
            if (!SelectedLevel)
            {
                Highlight.gameObject.SetActive(false);
                if ((Input.GetAxisRaw("L_XAxis_1") == 1 || Input.GetAxisRaw("DPad_XAxis_1") == 1) && (i == 0))
                {
                    i++;
                    placeHolder++;
                    if (placeHolder >= LevelNames.Length)
                        placeHolder = 0;
                    LevelText.text = LevelNames[placeHolder];
                    LevelImage.sprite = LevelImages[placeHolder];
                    DetailText.text = Details[placeHolder];

                    Invoke("ResetDatI", 1);
                    Invoke("ResetDatColor", .2f);

                }
                if ((Input.GetAxisRaw("L_XAxis_1") == -1 || Input.GetAxisRaw("DPad_XAxis_1") == -1) && (i == 0))
                {
                    i++;
                    placeHolder--;
                    if (placeHolder < 0)
                        placeHolder = LevelNames.Length - 1;
                    LevelImage.sprite = LevelImages[placeHolder];
                    LevelText.text = LevelNames[placeHolder];
                    DetailText.text = Details[placeHolder];

                    Invoke("ResetDatI", 1);
                    Invoke("ResetDatColor", .2f);
                }


                if (((Input.GetButtonUp("A_1")) || ((Input.GetAxisRaw("L_YAxis_1") == -1)) || (Input.GetAxisRaw("DPad_YAxis_1") == -1)) && (selected <= 2))
                {
                    selected++;
                    SelectedLevel = true;
                    mode++;
                    time++;
                    Arrow.transform.position = LocationMODE.transform.position;
                    //LevelImage.color = Color.red;
                    Invoke("ResetMode", .5f);
                    Highlight.gameObject.SetActive(true);

                }
        }

            if (SelectedLevel)
            {
                if (Mode == false)
                {
                    if ((Input.GetAxisRaw("L_XAxis_1") == 1 || Input.GetAxisRaw("DPad_XAxis_1") == 1) && (cd == 0))
                    {
                        cd++;
                        ControllerSpot++;
                        if (ControllerSpot >= GameController.GameModes.Length)
                        {
                            ControllerSpot = 0;
                        }
                        GameController.TextController(ControllerSpot);
                        Invoke("ResetDatCD", .2f);
                    }
                    if ((Input.GetAxisRaw("L_XAxis_1") == -1 || Input.GetAxisRaw("DPad_XAxis_1") == -1) && (cd == 0))
                    {
                        cd++;
                        ControllerSpot--;
                        if (ControllerSpot < 0)
                        {
                            ControllerSpot = GameController.GameModes.Length;
                        }
                        GameController.TextController(ControllerSpot);
                        Invoke("ResetDatCD", .2f);
                    }

                    if ((Input.GetButtonUp("A_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || (Input.GetAxisRaw("DPad_YAxis_1") == -1)) && mode == 0)
                    {
                        Mode = true;
                        Time = false;
                        time++;              
                        ModeText.color = textColor;
                        Arrow.transform.position = LocationTIME.transform.position;
                        Invoke("ResetTime", .5f);
                    }

                    if ((Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && mode == 0)
                    {
                        Mode = false;
                        SelectedLevel = false;
                        ResetDatColor();
                        ResetDatSelected();
                        Arrow.transform.position = startingLoc;
                        ModeText.color = Color.white;
                    }

                }


                if (Mode)
                {
                    if(Time == false)
                    {
                        if ((Input.GetAxis("L_XAxis_1") == 1 || Input.GetAxisRaw("DPad_XAxis_1") == 1) && (cd == 0))
                        {
                            cd++;
                            tracker++;
                            Debug.Log(tracker);
                            if (tracker > GameController.Times.Length)
                            {
                                tracker = 0;
                            }
                            GameController.TimeController(tracker);
                            Invoke("ResetDatCD", .2f);
                        }
                        if ((Input.GetAxis("L_XAxis_1") == -1 || Input.GetAxisRaw("DPad_XAxis_1") == -1) && (cd == 0))
                        {
                            cd++;
                            tracker--;
                            Debug.Log(tracker);
                            if (tracker < 0)
                            {
                                tracker = GameController.Times.Length;
                            }
                            GameController.TimeController(tracker);
                            Invoke("ResetDatCD", .2f);
                        }

                        if ((Input.GetButtonUp("A_1") || Input.GetAxisRaw("L_YAxis_1") == -1 || Input.GetAxisRaw("DPad_YAxis_1") == -1) && time == 0)
                        {
                            Time = true;
                            Invoke("ResetMode", .5f);
                            Arrow.transform.position = LocationSTART.transform.position;
                            TimeText.color = textColor;
                        }

                        if ((Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && time == 0)
                        {
                            Time = false;
                            Mode = false;
                            mode++;
                            Invoke("ResetMode", .5f);
                            ModeText.color = Color.white;
                            Arrow.transform.position = LocationMODE.transform.position;
                        }
                    }


                    if (Time)
                    {
                        StartCD = true;

                        if (StartCD)
                        {
                            if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("A_1") && cd == 0)
                            {
                                LoadScene = true;
                                cd++;
                                Debug.Log("Count");
                                //SceneManager.LoadScene(placeHolder + 3); //+ 3 for main menu and character selection and setup
                            }

                            if (Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1)
                            {
                                Time = false;
                                Mode = true;
                                StartCD = false;
                                time++;
                                Invoke("ResetTime", .5f);
                                TimeText.color = Color.white;
                                Arrow.transform.position = LocationTIME.transform.position;
                            }
                        }
                    }
                }
            }


        }

    }

    public int GetSceneNumber()
    {
        return placeHolder;
    }

    public bool GetLoadScene()
    {
        return LoadScene;
    }

    void ResetMode()
    {
        mode = 0;
    }

    void ResetTime()
    {
        time = 0;
    }

    void ResetDatI()
    {
        i = 0;
    }

    void ResetDatSelected()
    {
        selected = 0;
    }

    void ResetDatTracker()
    {
        tracker = 0;
    }

    void ResetDatTextColor()
    {
        ModeText.color = Color.yellow;
    }

    void ResetDatCD()
    {
        cd = 0;
    }

    void ResetDatColor()
    {
        LevelImage.color = Color.white;
        Highlight.gameObject.SetActive(false);
    }
}
