using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerTracker : MonoBehaviour {
    private int numberOfPlayers;
    private List<GameObject> tempPlayers;
    private GameObject[] players;
    public delegate void Win(string sender);
    public static event Win WinEvent;

    void Awake()
    {
        DeathControl.OutOfLives += KO;
    }

    void Start()
    {
        tempPlayers = new List<GameObject>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.transform.parent == null)
            {
                tempPlayers.Add(g);
            }
        }
        players = tempPlayers.ToArray();
        numberOfPlayers = players.Length;
        Debug.Log("There are " + numberOfPlayers + " players.");
    }

    void OnDestroy()
    {
        DeathControl.OutOfLives -= KO;
    }

    void KO(string whoDied)
    {
        Disable(GameObject.Find(whoDied));
        numberOfPlayers--;
        if (numberOfPlayers == 1)
        {
            foreach(GameObject g in players)
            {
                if (g.GetComponentInChildren<SpriteRenderer>().enabled)
                    WinEvent(g.name);
            }
        }
        else if (numberOfPlayers <= 0)
        {
            WinEvent("Nobody");
        }
    }

    void Disable(GameObject g)
    {
        g.GetComponent<Rigidbody>().velocity = Vector3.zero;
        g.GetComponent<Rigidbody>().isKinematic = true;
        g.GetComponentInChildren<SpriteRenderer>().enabled = false;
        g.GetComponent<DynamicCollider>().Disable();
    }
}
