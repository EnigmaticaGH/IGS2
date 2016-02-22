using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeController : MonoBehaviour {

    public Text GameModeText;
    public string[] GameModes;
    public bool LeftButton = false;
    public bool RightButton = false;
    int i = 0;

	// Use this for initialization
	void Start () {

        GameModeText = GameModeText.GetComponent<Text>();
        GameModeText.text = GameModes[0];
	
	}

    void OnEnable()
    {
        CursorMovement.OnClickedLeft += CursorCheckLeft;
        CursorMovement.OnClickedRight += CursorCheckRight;
    }

    void OnDisable()
    {
        CursorMovement.OnClickedLeft -= CursorCheckLeft;
        CursorMovement.OnClickedRight -= CursorCheckRight;
    }
	
	// Update is called once per frame
	void Update () {

        //LeftButton = GetComponent<CursorMovement>().LeftButton;
        //RightButton = GetComponent<CursorMovement>().RightButton;

        /*if(CursorMovement.OnClickedLeft())
            Debug.Log("Left Button Detected -- VIA GAME MODE CONTROLLER");
        if (RightButton)
            Debug.Log("Right Button Detected -- VIA GAME MODE CONTROLLER");*/

        
	
	}

    void TextController(int j) 
    {
        switch (j)
        {
            case 0:
                GameModeText.text = GameModes[j]; //Stock 10 deaths 
                break;
            case 1:
                GameModeText.text = GameModes[j]; //Stock 5 deaths
                break;
            case 2:
                GameModeText.text = GameModes[j]; //Stock 3 deaths
                break;
            case 3:
                GameModeText.text = GameModes[j]; //Timed
                break;
            default:
                break;



        }
    }

    void CursorCheckLeft() 
    {
        i = i - 1;
        if (i == -1)
            i = 3;
        TextController(i);
        Debug.Log("Left Button Detected -- VIA GAME MODE CONTROLLER");
        
    }
    void CursorCheckRight()
    {
        i = i + 1;
        if (i == 4)
            i = 0;
        TextController(i);
        Debug.Log("Right Button Detected -- VIA GAME MODE CONTROLLER");
    }
}
