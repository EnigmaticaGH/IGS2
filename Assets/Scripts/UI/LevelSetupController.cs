using UnityEngine;
using System.Collections;

public class LevelSetupController : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    
    [SerializeField]
    public static float TimerAmount = 5f; //Made this five because first time in Timer[] elements 

    void OnEnable() 
    {
        //Game Mode Events
        GameModeController.StockTen += StockTen;
        GameModeController.StockFive += StockFive;
        GameModeController.StockThree += StockThree;
        GameModeController.Timed += Timed;

        //Timer Events 
        GameModeController.Five += Timer5;
        GameModeController.Three += Timer3;
        GameModeController.Two += Timer2;
        GameModeController.Unlimited += TimerUnlimited;
    }

    void OnDisable() 
    {
        //Game Mode Events
        GameModeController.StockTen -= StockTen;
        GameModeController.StockFive -= StockFive;
        GameModeController.StockThree -= StockThree;
        GameModeController.Timed -= Timed;


        //Timer Events 
        GameModeController.Five -= Timer5;
        GameModeController.Three -= Timer3;
        GameModeController.Two -= Timer2;
        GameModeController.Unlimited -= TimerUnlimited;
    }

    void StockTen() 
    {
        player1.GetComponent<DeathControl>().numberOfLives = 10;
        player2.GetComponent<DeathControl>().numberOfLives = 10;
    }

    void StockFive() 
    {

        player1.GetComponent<DeathControl>().numberOfLives = 5;
        player2.GetComponent<DeathControl>().numberOfLives = 5;
    }

    void StockThree() 
    {
        player1.GetComponent<DeathControl>().numberOfLives = 3;
        player2.GetComponent<DeathControl>().numberOfLives = 3;
    }

    void Timed()
    {
        player1.GetComponent<DeathControl>().numberOfLives = 10000;  //Lives are unlimited in timed *** KEEP track of deaths of each player*****
        player2.GetComponent<DeathControl>().numberOfLives = 10000; //Lives are unlimited in timed *** KEEP track of deaths of each player*****
    }

    void Timer5() 
    {
        TimerAmount = 5; // 5 * 60
    }

    void Timer3()
    {
        TimerAmount = 3;
    }

    void Timer2()
    {
        TimerAmount = 2;
    }

    void TimerUnlimited()
    {
        TimerAmount = 100; //Just a high variable I'll use in TimerController to justify Timer
    }
    
	// Use this for initialization
	void Start () {

        //Lives = Lives.GetComponent<GameObject>(). //GetComponent<DeathControl>();

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float GetTime()
    {
        return TimerAmount;
    }
}
