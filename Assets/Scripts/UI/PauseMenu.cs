using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public string pauseButton;
    public Canvas pauseScreen;
    private bool paused;
    
    // Use this for initialization
    void Start()
    {
        paused = TogglePause(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown(pauseButton + "_" + i))
            {
                paused = TogglePause(!paused);
            }
        }
    }

    public bool TogglePause(bool isPaused)
    {
        //Time.timeScale = isPaused ? 0 : 1; //Had to comment this holy crap it was hard to figure out why the time scale wasnt working while paused lolol
        pauseScreen.enabled = isPaused;
        return isPaused;
    }

    public bool Paused()
    {
        return paused;
    }
}
