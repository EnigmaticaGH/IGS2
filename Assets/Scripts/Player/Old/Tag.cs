using UnityEngine;
using System.Collections;

public class Tag : MonoBehaviour {

    public Transform[] fears;
    private ControllerNumber[] controllers;
    private Transform[] allPlayers;
    private int playerController;

    void Start()
    {
        controllers = new ControllerNumber[fears.Length + 1];
        allPlayers = new Transform[fears.Length + 1];
        playerController = controllers.Length - 1;
        controllers[playerController] = GetComponent<ControllerNumber>();
        allPlayers[playerController] = transform;
        for (int i = 0; i < fears.Length; i++)
        {
            controllers[i] = fears[i].GetComponent<ControllerNumber>();
            allPlayers[i] = fears[i];
        }
    }
	
	void Update ()
    {
	    for(int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i].position.x > allPlayers[playerController].position.x)
            {
                int tmp = controllers[i].controllerNumber;
                controllers[i].controllerNumber = controllers[playerController].controllerNumber;
                controllers[playerController].controllerNumber = tmp;
                allPlayers[playerController].position += Vector3.right;
            }
        }
	}
}
