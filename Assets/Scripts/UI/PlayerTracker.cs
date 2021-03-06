﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerTracker : MonoBehaviour {
    private static int numberOfPlayers;
    private static List<GameObject> tempPlayers;
    public static GameObject[] players;
    public delegate void Win(string sender);
    public static event Win WinEvent;

    public static float levelBoundsX;
    public static float levelBoundY;
    public static float bottomOfLevel;

    void Awake()
    {
        DeathControl.OutOfLives += KO;
        levelBoundsX = 25;
        levelBoundY = 25;
        bottomOfLevel = -5;
    }

    void Start()
    {
        //AddPlayers();
    }

    public static void AddPlayers()
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
        SmashCamera.InitalizePlayers(players);
        PlayerLives.InitalizePlayers(players);
    }

    void OnDestroy()
    {
        DeathControl.OutOfLives -= KO;
        AbilityRegistry.Reset();
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
                    WinEvent(g.name.Substring(0, g.name.Length - 7));
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
        g.transform.position = Vector3.up * 2;
    }
}
