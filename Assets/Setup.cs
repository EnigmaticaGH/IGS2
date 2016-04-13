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
    public string[] LevelNames = { "Cloudy"};
    public string[] Details = { "Throw clouds at each other!"};
    public Sprite[] LevelImages;
    //Vector3 startingLoc;
    public GameObject[] SelectedObjects;

    public AudioSource Source;
    public AudioClip Click;
    public AudioClip Scroll;
    public AudioClip Back;

    int i = 0;
    public int placeHolder = 0;
    int selected = 0;
    GameModeController GameController;
    int ControllerSpot = 0;
    int cd = 0;
    //int TimerSpot = 0;
    int tracker = 0;
    int mode = 0;
    int time = 0;
    Color textColor;

    // Use this for initialization
    void Start()
    {

        for(int i = 0; i < SelectedObjects.Length; i++)
        {
            SelectedObjects[i].SetActive(false);
        }

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
                if (Input.GetButtonUp("B_1"))
                {
                    Source.clip = Back;
                    Source.Play();
                    SceneManager.LoadScene(1);
                }

                SelectedObjects[0].SetActive(true);

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
                    Source.clip = Scroll;
                    Source.Play();

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
                    Source.clip = Scroll;
                    Source.Play();

                    Invoke("ResetDatI", 1);
                    Invoke("ResetDatColor", .2f);
                }


                if (((Input.GetButtonUp("A_1")) || ((Input.GetAxisRaw("L_YAxis_1") == -1)) || (Input.GetAxisRaw("DPad_YAxis_1") == -1)) && (selected <= 2))
                {
                    selected++;
                    SelectedLevel = true;
                    mode++;
                    time++;
                    Source.clip = Click;
                    Source.Play();
                    //LevelImage.color = Color.red;
                    Invoke("ResetMode", .5f);
                    Highlight.gameObject.SetActive(true);

                }
        }

            if (SelectedLevel)
            {
                SelectedObjects[0].SetActive(false);

                if (Mode == false)
                {
                    SelectedObjects[1].SetActive(true);

                    if ((Input.GetAxisRaw("L_XAxis_1") == 1 || Input.GetAxisRaw("DPad_XAxis_1") == 1) && (cd == 0))
                    {
                        cd++;
                        ControllerSpot++;
                        if (ControllerSpot >= GameController.GameModes.Length)
                        {
                            ControllerSpot = 0;
                        }
                        Source.clip = Scroll;
                        Source.Play();
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
                        Source.clip = Scroll;
                        Source.Play();
                        GameController.TextController(ControllerSpot);
                        Invoke("ResetDatCD", .2f);
                    }

                    if ((Input.GetButtonUp("A_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || (Input.GetAxisRaw("DPad_YAxis_1") == -1)) && mode == 0)
                    {
                        Mode = true;
                        Time = false;
                        time++;
                        Source.clip = Click;
                        Source.Play();
                        ModeText.color = textColor;
                        Invoke("ResetTime", .5f);
                    }

                    if ((Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && mode == 0)
                    {
                        Source.clip = Back;
                        Source.Play();
                        Mode = false;
                        SelectedLevel = false;
                        SelectedObjects[1].SetActive(false);
                        ResetDatColor();
                        ResetDatSelected();
                        ModeText.color = Color.white;
                    }

                }


                if (Mode)
                {
                    SelectedObjects[1].SetActive(false);

                    if (Time == false)
                    {
                        SelectedObjects[2].SetActive(true);

                        if ((Input.GetAxis("L_XAxis_1") == 1 || Input.GetAxisRaw("DPad_XAxis_1") == 1) && (cd == 0))
                        {
                            Source.clip = Scroll;
                            Source.Play();
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
                            Source.clip = Scroll;
                            Source.Play();
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
                            Source.clip = Click;
                            Source.Play();
                            Time = true;
                            Invoke("ResetMode", .5f);
                            TimeText.color = textColor;
                        }

                        if ((Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1) && time == 0)
                        {
                            Source.clip = Back;
                            Source.Play();
                            Time = false;
                            Mode = false;
                            mode++;
                            SelectedObjects[2].SetActive(false);
                            Invoke("ResetMode", .5f);
                            ModeText.color = Color.white;
                        }
                    }


                    if (Time)
                    {
                        SelectedObjects[2].SetActive(false);

                        StartCD = true;

                        if (StartCD)
                        {
                            SelectedObjects[3].SetActive(true);

                            if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("A_1") && cd == 0)
                            {
                                Source.clip = Click;
                                Source.Play();
                                SelectedObjects[3].SetActive(false);
                                LoadScene = true;
                                cd++;
                                Debug.Log("Count");
                                //SceneManager.LoadScene(placeHolder + 3); //+ 3 for main menu and character selection and setup
                            }

                            if (Input.GetButtonUp("B_1") || Input.GetAxisRaw("L_YAxis_1") == 1 || Input.GetAxisRaw("DPad_YAxis_1") == 1)
                            {
                                Source.clip = Back;
                                Source.Play();
                                Time = false;
                                Mode = true;
                                StartCD = false;
                                SelectedObjects[3].SetActive(false);
                                time++;
                                Invoke("ResetTime", .5f);
                                TimeText.color = Color.white;
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
