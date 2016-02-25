using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public Vector3 player1Spawn;
    public Vector3 player2Spawn;
    // Use this for initialization
    void Start()
    {
        player.name = "Derek Player 1";
        player.GetComponent<ControllerNumber>().controllerNumber = 1;
        GameObject p1 = (GameObject)Instantiate(player, player1Spawn, Quaternion.identity);
        player.name = "Generic Player 2";
        player.GetComponent<ControllerNumber>().controllerNumber = 2;
        GameObject p2 = (GameObject)Instantiate(player, player2Spawn, Quaternion.identity);
        PlayerTracker.AddPlayers();
        //
    }

    // Update is called once per frame
    void Update()
    {

    }
}
