using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinEventReceiver : MonoBehaviour
{
    public GameObject winCanvas;
    public Text winText;
    private bool won;

    void Awake()
    {
        PlayerTracker.WinEvent += OnWin;
        winCanvas.SetActive(false);
        won = false;
    }

    void OnDestroy()
    {
        PlayerTracker.WinEvent -= OnWin;
    }

    public void OnWin(string sender)
    {
        Time.timeScale = 0;
        winText.text = sender + " WINS";
        winCanvas.SetActive(true);
        won = true;
    }

    void Update()
    {
        if (won && (Input.GetButtonDown("A_1") || Input.GetButtonDown("A_2") || Input.GetButtonDown("A_3") || Input.GetButtonDown("A_4")))
        {
            Button1_Click();
        }
    }

    public void Button1_Click()
    {
        //Application.LoadLevel(0);
        SceneManager.LoadScene(1);
    }
}
