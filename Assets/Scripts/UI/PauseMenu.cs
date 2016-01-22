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
        if (Input.GetButtonDown(pauseButton))
        {
            paused = TogglePause(!paused);
        }
    }

    bool TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        pauseScreen.enabled = isPaused;
        return isPaused;
    }
}
