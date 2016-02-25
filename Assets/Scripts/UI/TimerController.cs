using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour {

    public GameObject LevelSetup;
    public Text timer;

    LevelSetupController TimerAmt;
    WinEventReceiver Win;

    float time;

    float seconds, minutes;

    bool unlimited = false;

	// Use this for initialization
	void Start () {

        Win = GetComponent<WinEventReceiver>();

        TimerAmt = GetComponent<LevelSetupController>();
        
        timer = GetComponent<Text>();

        time = LevelSetup.GetComponent<LevelSetupController>().GetTime();
        if (time == 100) 
        {
            timer.text = "~:~~";
            unlimited = true;
        }
        minutes = time;
        seconds = 0;
        Debug.Log(time);
	}
	
	// Update is called once per frame
	void Update () {

        //minutes = (int)(Time.timeSinceLevelLoad / 60f);
        //seconds = (int)(Time.timeSinceLevelLoad % 60f);
        if(seconds <= 0 && minutes >= 1 && unlimited == false)
        {
            seconds = 59;
            minutes = minutes - 1;
        }
            
        seconds -= Time.deltaTime;

        //timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        if (time == 100)
        {
            timer.text = "~:~~";
        }else
            timer.text = minutes.ToString() + ":" + seconds.ToString("00");

        if (minutes == 0 && seconds == 0)
        {
            Win.OnWin("Hello");
        }
	
	}
}