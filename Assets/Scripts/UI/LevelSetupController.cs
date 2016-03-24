using UnityEngine;
using System.Collections;

public class LevelSetupController : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;

    public GameObject[] players;
    
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
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DeathControl>().numberOfLives = 10;
        }
    }

    void StockFive() 
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DeathControl>().numberOfLives = 5;
        }
    }

    void StockThree() 
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DeathControl>().numberOfLives = 3;
        }
    }

    void Timed()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DeathControl>().numberOfLives = 1;
        }
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
