using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player1Spawn;
    public GameObject player2Spawn;

    public GameObject[] Characters;


    int PlayersAmt = 0;
    // Use this for initialization
    void Start()
    {

        int p1Pos = 0;
        int p2Pos = 0;
        p1Pos = CharacterMenuController.p1Pos; //Use this for spawning selected characters
        p2Pos = CharacterMenuController.p2Pos; //Use this for spawning selected characters
        Debug.Log(p1Pos);
        //PlayersAmt = CursorController.ControllerAmount; //Gets 2 because Cursor Controller is used in multiple scenes
        PlayersAmt = CharacterMenuController.ControllerNumber; //USed character menu controller since it's only called once
        Debug.Log(PlayersAmt);

        if (PlayersAmt == 1)
        {
            GameObject p1;
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Debug.Log(player1Spawn.transform.position);
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1;
            Debug.Log("Spawn p1");
        }
        else if (PlayersAmt == 2)
        {
            GameObject p1;
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1;
            Debug.Log("Spawn p1");

            GameObject p2;
            p2 = (GameObject)Instantiate(Characters[p2Pos], new Vector3(player2Spawn.transform.position.x, player2Spawn.transform.position.y, player2Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 2;
            Debug.Log("Spawn p2");
        }

        /*for (int i = 0; i < PlayersAmt; i++) 
        {
            if (i == 0) 
            {
                GameObject p1;
                p1 = (GameObject)Instantiate(Characters[p1Pos], player1Spawn, Quaternion.identity); //Need to effect this for menu system
                Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1;
                Debug.Log("Spawn p1");
            }
            if (i == 1)
            {
                GameObject p2;
                p2 = (GameObject)Instantiate(Characters[p2Pos], player2Spawn, Quaternion.identity); //Need to effect this for menu system
                Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 2;
                Debug.Log("Spawn p2");
            }*/

        /*if (i == 3)
        {
            GameObject p1;
            p1 = (GameObject)Instantiate(player1, player1Spawn, Quaternion.identity); //Need to effect this for menu system
            Debug.Log("Spawn p1");
        }*/



        //GameObject p1 = (GameObject)Instantiate(player1, player1Spawn, Quaternion.identity); //Need to effect this for menu system
        //GameObject p2 = (GameObject)Instantiate(player2, player2Spawn, Quaternion.identity);
        PlayerTracker.AddPlayers();
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
