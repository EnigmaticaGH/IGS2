using UnityEngine;
using System.Collections;

public class PlayerTracker : MonoBehaviour {
    private int numberOfPlayers;
    private GameObject[] players;
    public delegate void Win(string sender);
    public static event Win WinEvent;

    void Awake()
    {
        DeathControl.OutOfLives += KO;
    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers = players.Length;
    }

    void OnDestroy()
    {
        DeathControl.OutOfLives -= KO;
    }

    void KO(string whoDied)
    {
        Destroy(GameObject.Find(whoDied), 0.5f);
        if (--numberOfPlayers == 1)
        {
            WinEvent(players[0].name);
        }
        if (--numberOfPlayers == 0)
        {
            WinEvent("Nobody");
        }
    }
}
