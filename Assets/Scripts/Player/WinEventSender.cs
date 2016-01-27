using UnityEngine;
using System.Collections;

public class WinEventSender : MonoBehaviour
{
    private string sender;
    public delegate void WinDelegate(string sender);
    public static event WinDelegate WinEvent;

    void Start()
    {
        sender = GetComponent<Movement>().controllerNumber.ToString();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Win"))
            WinEvent(sender);
    }
}
