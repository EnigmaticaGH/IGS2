using UnityEngine;
using System.Collections;

public class WinEventSender : MonoBehaviour
{
    private string sender;
    public delegate void WinDelegate(string sender);
    public static event WinDelegate WinEvent;

    void Awake()
    {
        DeathControl.OnDeath += FearsWin;
    }

    void OnDestroy()
    {
        DeathControl.OnDeath -= FearsWin;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Win"))
        {
            sender = name + " " + GetComponent<ControllerNumber>().controllerNumber.ToString();
            WinEvent(sender);
        }
    }

    void FearsWin(float respawnTime)
    {
        WinEvent("Fear");
    }
}
