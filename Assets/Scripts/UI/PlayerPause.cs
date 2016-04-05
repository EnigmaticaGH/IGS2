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

    //int ControllerNumber = 0;
    int count = 0;
    public bool start1 = false;

    int menuCount;

    PauseMenu Pause;

    void Awake()
    {
        PauseCanvas.enabled = false;
        //ControllerNumber = CharacterMenuController.ControllerNumber;
    }
    // Use this for initialization
    void Start()
    {

        Pause = GameObject.Find("UIController").GetComponent<PauseMenu>();
        
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


        if (Pause.Paused())
        {
            start1 = true;
        }

        if (Pause.Paused() == false)
        {
            start1 = false;
        }

        //******Need to fix start bool************* 

        if (Input.GetButton("Start_" + 1) && (count == 0))
        {
            count++;
            start1 = true;
            PauseCanvas.enabled = start1;

            //float velocity = Input.GetAxis("DPad_XAxis_1") * speed;

        }
        if (start1)
        {
            if (Input.GetButton("A_1"))
            {
                SceneManager.LoadScene(0); //Menu
            }
            if (Input.GetAxis("DPad_YAxis_1") == -1 && menuCount == 0)
            {
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
                if (Input.GetButton("A_1"))
                {
                    SceneManager.LoadScene(1); //Character Menu
                }
                if (Input.GetAxis("DPad_YAxis_1") == 1)
                {
                    StartCoroutine("resetThisButton1");
                }
            }
        }
    }


    IEnumerator resetCount()
    {
        yield return new WaitForSeconds(.25f);
        count = 0;
        Debug.Log("Reset Count");
        start1 = false;
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
