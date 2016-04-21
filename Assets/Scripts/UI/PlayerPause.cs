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
    void Update()
    {

        for (int i = 1; i <= 4; i++)
        {
            if (start1 == false)
            {

                if ((Input.GetButtonDown("Start_" + i)) && (count == 0))
                {
                    Time.timeScale = 0;
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
                    count++;
                    start1 = false;
                    PauseCanvas.enabled = start1;
                    StartCoroutine("resetCount");
                }

                if (Input.GetButton("A_" + i))
                {
                    Source.clip = Click;
                    Source.Play();
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0); //Menu
                }
                if (Input.GetAxis("DPad_YAxis_" + i) == -1 && menuCount == 0)
                {
                    Source.clip = Scroll;
                    Source.Play();
                    menuCount++;

                    if (menuCount == 1)
                    {
                        ColorBlock temp1;
                        temp1 = PauseButtons[0].GetComponent<Button>().colors;
                        temp1.normalColor = Color.white;
                        PauseButtons[0].GetComponent<Button>().colors = temp1;
                        temp1.normalColor = Color.yellow;
                        PauseButtons[1].GetComponent<Button>().colors = temp1;
                    }

                }

                if (menuCount == 1)
                {
                    if (Input.GetButton("A_" + i))
                    {
                        Source.clip = Click;
                        Source.Play();
                        Time.timeScale = 1;
                        SceneManager.LoadScene(1); //Character Menu
                    }
                    if (Input.GetAxis("DPad_YAxis_" + i) == 1)
                    {
                        Source.clip = Scroll;
                        Source.Play();
                        StartCoroutine("resetThisButton1");
                    }
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
        menuCount = 0;
        yield return null; //Button Cooldown


    }

}
