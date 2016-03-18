using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    public Text StartText;
    public Image Highlight;
    public Image HighlightMode;
    public bool SelectedLevel = false;
    public bool SelectedGameMode = false;
    public Text LevelText;
    public Image LevelImage;
    public Image LevelHighlight;
    public Text ModeText;
    public Text DetailText;
    public string[] LevelNames = { "Cloudy", "Outback", "Mechanic" };
    public string[] Details = { "Throw clouds at each other!", "Hit up the desert or the saloon in this outback thriller!", "Wow!! lots of moving stuff cooooolllll!" };
    public Sprite[] LevelImages;
    int i = 0;
    int placeHolder = 0;
    int selected = 0;
    GameModeController GameController;
    int ControllerSpot = 0;
    int cd = 0;
    int TimerSpot = 0;
    int tracker = 0;

    // Use this for initialization
    void Start()
    {
        Highlight.gameObject.SetActive(false);

        HighlightMode.gameObject.SetActive(false);

        LevelText = LevelText.GetComponent<Text>();

        LevelText.text = LevelNames[0];

        GameController = GameObject.Find("Game Mode").GetComponent<GameModeController>();

        LevelHighlight.gameObject.SetActive(false);

        StartText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (!SelectedGameMode)
        {
            StartText.gameObject.SetActive(false);
            HighlightMode.gameObject.SetActive(false);
            if (!SelectedLevel)
            {
                Highlight.gameObject.SetActive(true);
                if (Input.GetAxis("L_XAxis_1") == 1 && (i == 0))
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
                if (Input.GetAxis("L_XAxis_1") == -1 && (i == 0))
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

            }

            if (SelectedLevel)
            {
                HighlightMode.gameObject.SetActive(true);
                Highlight.gameObject.SetActive(false);
                //StartText.gameObject.SetActive(true);
                if (Input.GetAxis("L_XAxis_1") == 1 && (cd == 0))
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
                if (Input.GetAxis("L_XAxis_1") == -1 && (cd == 0))
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

                if (Input.GetAxis("L_YAxis_1") == 1 && (cd == 0))
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
                if (Input.GetAxis("L_YAxis_1") == -1 && (cd == 0))
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

            }
        }

        if (Input.GetButtonDown("Start_1"))
        {
            SceneManager.LoadScene(placeHolder + 3); //+ 2 for main menu and character selection and setup
        }


        if (Input.GetButtonUp("A_1") && (selected <= 2))
        {
            selected++;
            SelectedLevel = true;
            //LevelImage.color = Color.red;
            //LevelHighlight.gameObject.SetActive(true);
            if (selected == 2)
            {
                SelectedLevel = false;
                Invoke("ResetDatColor", .01f);
                Invoke("ResetDatSelected", .2f);
            }
        }

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
        LevelHighlight.gameObject.SetActive(false);
    }
}
