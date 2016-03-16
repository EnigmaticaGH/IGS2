using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPause : MonoBehaviour {

    /// PlayerPause
    /// ****Handles spawning Pause prefab**** 
    /// ****Button control on pause menu******Not easy GetAxis was different to use but finally got it

    public Object PauseCanvas;
    public Button[] PauseButtons;

    int ControllerNumber = 0;
    int count = 0;
    public bool start1 = false;

    int menuCount;

    void Awake()
    {
        ControllerNumber = CharacterMenuController.ControllerNumber;
        //start1 = GetComponent<PauseMenu>().TogglePause(false);
    }
    // Use this for initialization
    void Start()
    {


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

        /*if (Input.GetButton("Start_" + ControllerNumber ))
        {
            start = true;
            count++;
            Object temp;
            temp = Instantiate(PauseCanvas, transform.position, Quaternion.identity);
            if(count > 1)
            {
                count = 0;
                start = false;
                Destroy(temp);
            }

            if (Input.GetButton("DPad_XAxis_" + ControllerNumber))
            {
                menuCount++;

                ColorBlock temp1;
                temp1 = PauseButtons[0].GetComponent<Button>().colors;
                temp1.normalColor = Color.white;
                PauseButtons[0].GetComponent<Button>().colors = temp1;
                temp1.normalColor = Color.yellow;
                PauseButtons[1].GetComponent<Button>().colors = temp1;

                if (menuCount > 1)
                    Invoke("resetButton", .5f); 

            }
        }*/

        //******Need to fix start bool************* 

        if (Input.GetButton("Start_" + 1) && (count == 0))
        {
            count++;
            start1 = true;
            Debug.Log(start1);


            if (count > 1)
            {
                StartCoroutine("resetCount");
                start1 = false;
                Debug.LogError("Count");
                Object temp;
                temp = Instantiate(PauseCanvas, transform.position, Quaternion.identity);
                Debug.Log(start1);

                Destroy(temp);
                //Invoke("resetCount", .25f);

            }
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
                Debug.LogError("Hello" + menuCount);

                if (menuCount == 1)
                {
                    Debug.LogError("Hello");
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
                    Debug.Log("I wish i worked");
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

        Debug.Log("Hi");
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
