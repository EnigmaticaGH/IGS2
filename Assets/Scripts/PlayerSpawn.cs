using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Vector3 player1Spawn;
    public Vector3 player2Spawn;
    // Use this for initialization
    void Start()
    {
        GameObject p1 = (GameObject)Instantiate(player1, player1Spawn, Quaternion.identity);
        GameObject p2 = (GameObject)Instantiate(player2, player2Spawn, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
