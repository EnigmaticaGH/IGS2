﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinEventReceiver : MonoBehaviour
{
    public GameObject winCanvas;
    public Text winText;

    void Awake()
    {
        WinEventSender.WinEvent += OnWin;
        winCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        WinEventSender.WinEvent -= OnWin;
    }

    void OnWin(string sender)
    {
        Time.timeScale = 0;
        winText.text = "Player " + sender + " WINS";
        winCanvas.SetActive(true);
    }

    public void Button1_Click()
    {
        Application.LoadLevel(0);
    }
}
