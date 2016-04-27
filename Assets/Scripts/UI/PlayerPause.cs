using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPause : MonoBehaviour {

    /// PlayerPause
    /// ****Handles spawning Pause prefab**** 
    /// ****Button control on pause menu******Not easy GetAxis was different to use but finally got it

    public Canvas PauseCanvas;
    public Button[] PauseButtons;

    public AudioSource Source;
    public AudioClip Click;
    public AudioClip Scroll;
    public AudioClip Back;

    //int ControllerNumber = 0;
    int count = 0;
    public bool start1 = false;
    public bool restart = false;
    public bool quit = false;

    int menuCount;

    //PauseMenu Pause;

    void Awake()
    {
        PauseCanvas.enabled = false;
        //ControllerNumber = CharacterMenuController.ControllerNumber;
    }
    // Use this for initialization
    void Start()
    {

        //Pause = GameObject.Find("UIController").GetComponent<PauseMenu>();
        
        //DevField = DevField.Addcmponent<GameObject>;
        for (int i = 0; i < PauseButtons.Length; i++)
        {
            ColorBlock temp;
            temp = PauseButtons[0].GetComponent<Button>().colors;
            temp.normalColor = Color.yellow;
            PauseButtons[0].GetComponent<Button>().colors = temp;

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(count);

        for (int i = 1; i <= 4; i++)
        {
            if (start1 == false)
            {

                if ((Input.GetButtonDown("Start_" + i)) && (count == 0))
                {
                    //Time.timeScale = 0;
                    Source.clip = Click;
                    Source.Play();
                    count++;
                    start1 = true;
                    PauseCanvas.enabled = start1;
                    StartCoroutine("resetCount");
                }

            }

            if (start1)
            {

                if ((Input.GetButtonDown("Start_" + i) || Input.GetButtonUp("B_" + i)) && (count == 0))
                {
                    Time.timeScale = 1;
                    Source.clip = Back;
                    Source.Play();
                    Debug.Log("Hello");
                    start1 = false;
                    PauseCanvas.enabled = false;
                    StartCoroutine("resetThisButton1");

                }

                if (Input.GetButton("A_" + i))
                {
                    Source.clip = Click;
                    Source.Play();
                    Time.timeScale = 1;
                    count++;
                    start1 = false;
                    PauseCanvas.enabled = false;
                }
                if (Input.GetAxis("DPad_YAxis_" + i) == -1 && count == 0)
                {
                    Source.clip = Scroll;
                    Source.Play();
                    count++;
                    restart = true;
                    start1 = false;
                    quit = false;
                    StartCoroutine("resetCount");
                    ColorBlock temp1;
                    temp1 = PauseButtons[0].GetComponent<Button>().colors;
                    temp1.normalColor = Color.white;
                    PauseButtons[0].GetComponent<Button>().colors = temp1;
                    temp1.normalColor = Color.yellow;
                    PauseButtons[1].GetComponent<Button>().colors = temp1;

                }
            }

                if (restart)
                {

                    if ((Input.GetButtonDown("Start_" + i) || Input.GetButtonUp("B_" + i)))
                    {
                        Time.timeScale = 1;
                        Source.clip = Back;
                        Source.Play();
                        Debug.Log("Hello");
                        start1 = false;
                        restart = false;
                        count++;
                        StartCoroutine("resetCount");
                        menuCount = 0;
                        quit = false;
                        PauseCanvas.enabled = false;
                        StartCoroutine("resetThisButton1");

                    }
                    if (Input.GetButton("A_" + i))
                    {
                        Source.clip = Click;
                        Source.Play();
                        Time.timeScale = 1;
                        SceneManager.LoadScene(1); //Character Menu
                    }
                    if (Input.GetAxis("DPad_YAxis_" + i) == 1 & count == 0)
                    {
                        Source.clip = Scroll;
                        Source.Play();
                        menuCount = 0;
                        quit = false;
                        restart = false;
                        start1 = true;
                        count++;
                        StartCoroutine("resetCount");
                        ColorBlock temp;
                        temp = PauseButtons[0].GetComponent<Button>().colors;
                        temp.normalColor = Color.yellow;
                        PauseButtons[0].GetComponent<Button>().colors = temp;
                        temp.normalColor = Color.white;
                        PauseButtons[1].GetComponent<Button>().colors = temp;
                    }

                    if (Input.GetAxis("DPad_YAxis_" + i) == -1 && count == 0)
                    {
                        count++;

                        StartCoroutine("resetCount");
                        restart = false;
                        ColorBlock temp;
                        temp = PauseButtons[0].GetComponent<Button>().colors;
                        temp.normalColor = Color.white;
                        PauseButtons[0].GetComponent<Button>().colors = temp;
                        temp.normalColor = Color.white;
                        PauseButtons[1].GetComponent<Button>().colors = temp;
                        temp.normalColor = Color.yellow;
                        PauseButtons[2].GetComponent<Button>().colors = temp;
                        quit = true;
                    }
                }

                if (quit)
                {
                    if ((Input.GetButtonDown("Start_" + i) || Input.GetButtonUp("B_" + i)))
                    {
                        Time.timeScale = 1;
                        Source.clip = Back;
                        Source.Play();
                        Debug.Log("Hello");
                        start1 = false;
                        restart = false;
                        count = 0;
                        menuCount = 0;
                        quit = false;
                        PauseCanvas.enabled = false;
                        StartCoroutine("resetThisButton1");
                    }

                    if (Input.GetButton("A_" + i))
                    {
                        Source.clip = Click;
                        Source.Play();
                        Time.timeScale = 1;
                        SceneManager.LoadScene(0); //Character Menu
                    }

                    if (Input.GetAxis("DPad_YAxis_" + i) == 1 && count == 0)
                    {
                        quit = false;
                        restart = true;
                        count++;
                        StartCoroutine("resetCount");
                        Source.clip = Scroll;
                        Source.Play();
                        ColorBlock temp;
                        temp = PauseButtons[0].GetComponent<Button>().colors;
                        temp.normalColor = Color.white;
                        PauseButtons[0].GetComponent<Button>().colors = temp;
                        temp.normalColor = Color.yellow;
                        PauseButtons[1].GetComponent<Button>().colors = temp;
                        temp.normalColor = Color.white;
                        PauseButtons[2].GetComponent<Button>().colors = temp;
                    }
                }
          
        }


    }


    IEnumerator resetCount()
    {
        yield return new WaitForSeconds(.25f);
        count = 0;
        Debug.Log("Reset Count");
    }

    IEnumerator resetThisButton1()
    {

        ColorBlock temp;
        temp = PauseButtons[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        PauseButtons[0].GetComponent<Button>().colors = temp;
        temp.normalColor = Color.white;
        PauseButtons[1].GetComponent<Button>().colors = temp;
        PauseButtons[2].GetComponent<Button>().colors = temp;

        menuCount = 0;
        yield return null; //Button Cooldown


    }

}
