using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    //Used for switching gameplayer object to collect character selection from menu
    //I think this an apporiate route to take considering all the prefabs are the same scripts 
    //just different controller numbers
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    //This for the most part helps with players falling through the level of spawn
    //Instead the congustion of gameplay is the middle
    public GameObject player1Spawn;
    public GameObject player2Spawn;
    public GameObject player3Spawn;
    public GameObject player4Spawn;

    //*****IMPORTANT******
    /******Used for character spawning*******
     *****Characters are in same order as main menu Character Screen***** 
     *
     * ****Need to setup a measure to insure they are
     */
    public GameObject[] Characters;



    int PlayersAmt = 0;
    // Use this for initialization
    void Awake()
    {
        int p1Pos = 0;
        int p2Pos = 0;
        int p3Pos = 0;
        int p4Pos = 0;
        p1Pos = CharacterMenuController.p1Pos; //Use this for spawning selected characters
        p2Pos = CharacterMenuController.p2Pos; //Use this for spawning selected characters
        p3Pos = CharacterMenuController.p3Pos; //Use this for spawning selected characters
        p4Pos = CharacterMenuController.p4Pos; //Use this for spawning selected characters

        Debug.Log(p1Pos);
        //PlayersAmt = CursorController.ControllerAmount; //Gets 2 because Cursor Controller is used in multiple scenes
        PlayersAmt = CharacterMenuController.ControllerNumber; //USed character menu controller since it's only called once
        Debug.Log(PlayersAmt);

        if (PlayersAmt == 1)
        {
            //Used for creating players 
            //p1 spawn character[] gameobject and p1 selection from main menu
            //Spawn at location of spawn point in each scene
            //After I spawn object I need to make sure the controller numbers are connected correctly 
            GameObject p1;
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Debug.Log(player1Spawn.transform.position);
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1; //Make sure controller is connected to correct player
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
        else if (PlayersAmt == 3) 
        {
            GameObject p1;
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1;
            Debug.Log("Spawn p1");

            GameObject p2;
            p2 = (GameObject)Instantiate(Characters[p2Pos], new Vector3(player2Spawn.transform.position.x, player2Spawn.transform.position.y, player2Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 2;
            Debug.Log("Spawn p2");

            GameObject p3;
            p3 = (GameObject)Instantiate(Characters[p3Pos], new Vector3(player3Spawn.transform.position.x, player3Spawn.transform.position.y, player3Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p3Pos].GetComponent<ControllerNumber>().controllerNumber = 3;
            Debug.Log("Spawn p2");
        }
        else if (PlayersAmt == 4)
        {
            GameObject p1;
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 1;
            Debug.Log("Spawn p1");

            GameObject p2;
            p2 = (GameObject)Instantiate(Characters[p2Pos], new Vector3(player2Spawn.transform.position.x, player2Spawn.transform.position.y, player2Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p1Pos].GetComponent<ControllerNumber>().controllerNumber = 2;
            Debug.Log("Spawn p2");

            GameObject p3;
            p3 = (GameObject)Instantiate(Characters[p3Pos], new Vector3(player3Spawn.transform.position.x, player3Spawn.transform.position.y, player3Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p3Pos].GetComponent<ControllerNumber>().controllerNumber = 3;
            Debug.Log("Spawn p2");

            GameObject p4;
            p4 = (GameObject)Instantiate(Characters[p3Pos], new Vector3(player4Spawn.transform.position.x, player4Spawn.transform.position.y, player4Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            Characters[p3Pos].GetComponent<ControllerNumber>().controllerNumber = 4;
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
