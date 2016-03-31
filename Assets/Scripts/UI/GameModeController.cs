using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeController : MonoBehaviour {

    public delegate void GameMode();
    public static event GameMode StockTen;
    public static event GameMode StockFive;
    public static event GameMode StockThree;
    public static event GameMode Timed;

    public delegate void TimeMode();
    public static event TimeMode Five;
    public static event TimeMode Three;
    public static event TimeMode Two;
    public static event TimeMode Unlimited;

    public Text GameModeText;
    public Text TimeAmount;
    //private int Time;
    public string[] GameModes;
    public string[] Times;
    public bool LeftButton = false;
    public bool RightButton = false;
    int i = 0;
    int strPosTime = 0;

	// Use this for initialization
	void Start () {


        
        GameModeText = GameModeText.GetComponent<Text>();
        GameModeText.text = GameModes[0];
        TextController(0);
        TimeAmount = TimeAmount.GetComponent<Text>();
        TimeAmount.text = Times[0];

	
	}

    void OnEnable()
    {
        CursorMovement.OnClickedLeft += CursorCheckLeft;
        CursorMovement.OnClickedRight += CursorCheckRight;
        CursorMovement.OnClickedRightTime += CursorRightTime;
        CursorMovement.OnClickedLeftTime += CursorLeftTime;
    }

    void OnDisable()
    {
        CursorMovement.OnClickedLeft -= CursorCheckLeft;
        CursorMovement.OnClickedRight -= CursorCheckRight;
        CursorMovement.OnClickedRightTime -= CursorRightTime;
        CursorMovement.OnClickedLeftTime += CursorLeftTime;

    }
	 
	// Update is called once per frame
	void Update () {

        
	
	}

     public void TextController(int j) 
    {
        switch (j)
        {
            case 0:
                GameModeText.text = GameModes[j]; //Stock 10 deaths 
                TimeAmount.gameObject.SetActive(true);
                StockTen();
                break;
            case 1:
                GameModeText.text = GameModes[j]; //Stock 5 deaths
                TimeAmount.gameObject.SetActive(true);
                StockFive();
                break;
            case 2:
                GameModeText.text = GameModes[j]; //Stock 3 deaths
                TimeAmount.gameObject.SetActive(true);
                StockThree();
                break;
            case 3:
                GameModeText.text = GameModes[j]; //Timed
                TimeAmount.gameObject.SetActive(true);
                Timed();
                break;
            default:
                break;



        }
    }

    public void TimeController(int b) 
    {
        switch (b)
        {
            case 0:
                TimeAmount.text = Times[b]; //5:00
                Five();
                break;
            case 1:
                TimeAmount.text = Times[b]; //3:00
                Three();
                break;
            case 2:
                TimeAmount.text = Times[b]; //2:00
                Two();
                break;
            case 3:
                TimeAmount.text = Times[b]; //Unlimited
                Unlimited();
                break;
            default:
                break;



        }
    }

    void CursorCheckLeft() 
    {
        i = i - 1;
        if (i <= -1)
            i = 3;
        TextController(i);
        Debug.Log("Left Button Detected -- VIA GAME MODE CONTROLLER");
        
    }
    void CursorCheckRight()
    {
        i = i + 1;
        if (i >= 4)
            i = 0;
        TextController(i);
        Debug.Log("Right Button Detected -- VIA GAME MODE CONTROLLER");
    }

    void CursorRightTime() 
    {
        strPosTime = strPosTime + 1;
        if (strPosTime >= 4)
            strPosTime = 0;
        TimeController(strPosTime);
        Debug.Log("Right Button Detected -- VIA GAME MODE CONTROLLER");
    }

    void CursorLeftTime() 
    {
        strPosTime = strPosTime - 1;
        if (strPosTime <= -1)
            strPosTime = 3;
        TimeController(strPosTime);
        Debug.Log("Left Button Detected -- VIA GAME MODE CONTROLLER");
    }
}
