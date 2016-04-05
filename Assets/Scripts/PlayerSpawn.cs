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

    //private int[] playerLives;
    //GameModeController Gm;

    int PlayersAmt = 0;
    // Use this for initialization

    void OnEnable()
    {
        GameModeController.StockTen += stockTen;
        GameModeController.StockFive += stockFive;
        GameModeController.StockThree += stockThree;
        GameModeController.Timed += timed;
    }

    void OnDisable()
    {
        GameModeController.StockTen -= stockTen;
        GameModeController.StockFive -= stockFive;
        GameModeController.StockThree -= stockThree;
        GameModeController.Timed -= timed;
    }

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
        string p1n = Characters[p1Pos].name;
        string p2n = Characters[p2Pos].name;
        string p3n = Characters[p3Pos].name;
        string p4n = Characters[p4Pos].name;


        //Debug.Log(p1Pos);
        //PlayersAmt = CursorController.ControllerAmount; //Gets 2 because Cursor Controller is used in multiple scenes
        PlayersAmt = CharacterMenuController.ControllerNumber; //USed character menu controller since it's only called once

        //Debug.Log(PlayersAmt);

        if (PlayersAmt >= 1)
        {
            //Used for creating players 
            //p1 spawn character[] gameobject and p1 selection from main menu
            //Spawn at location of spawn point in each scene
            //After I spawn object I need to make sure the controller numbers are connected correctly 
            GameObject p1;
            Characters[p1Pos].name = "Player 1";
            p1 = (GameObject)Instantiate(Characters[p1Pos], new Vector3(player1Spawn.transform.position.x, player1Spawn.transform.position.y, player1Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            //Debug.Log(player1Spawn.transform.position);
            p1.GetComponent<ControllerNumber>().controllerNumber = 1; //Make sure controller is connected to correct player
        }
        if (PlayersAmt >= 2)
        {
            GameObject p2;
            Characters[p2Pos].name = "Player 2";
            Characters[p2Pos].GetComponent<ControllerNumber>().controllerNumber = 2;
            p2 = (GameObject)Instantiate(Characters[p2Pos], new Vector3(player2Spawn.transform.position.x, player2Spawn.transform.position.y, player2Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            p2.GetComponent<ControllerNumber>().controllerNumber = 2;
        }
        if (PlayersAmt >= 3)
        {
            Characters[p3Pos].name = "Player 3";
            GameObject p3;
            p3 = (GameObject)Instantiate(Characters[p3Pos], new Vector3(player3Spawn.transform.position.x, player3Spawn.transform.position.y, player3Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            //Characters[p3Pos].GetComponent<ControllerNumber>().controllerNumber = 3;
            p3.GetComponent<ControllerNumber>().controllerNumber = 3;
            Debug.Log("Spawn p3");
        }
        if (PlayersAmt == 4)
        {
            Characters[p4Pos].name = "Player 4";
            GameObject p4;
            p4 = (GameObject)Instantiate(Characters[p4Pos], new Vector3(player4Spawn.transform.position.x, player4Spawn.transform.position.y, player4Spawn.transform.position.z), Quaternion.identity); //Need to effect this for menu system
            //Characters[p4Pos].GetComponent<ControllerNumber>().controllerNumber = 4;
            p4.GetComponent<ControllerNumber>().controllerNumber = 4;
            Debug.Log("Spawn p4");
        }
        Characters[p1Pos].name = p1n;
        Characters[p2Pos].name = p2n;
        Characters[p3Pos].name = p3n;
        Characters[p4Pos].name = p4n;

        PlayerTracker.AddPlayers();
        //
    }

    void stockTen()
    {

    }
    void stockFive()
    {

    }
    void stockThree()
    {

    }
    void timed()
    {

    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
