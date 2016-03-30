using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLives : MonoBehaviour
{

    public Canvas Canvas;
    public GameObject player1;
    public Text player1Lives;
    public GameObject[] HeadPlayers;
    public GameObject player2;
    public Text player2Lives;
    public GameObject player3;
    public Text player3Lives;
    public GameObject player4;
    public Text player4Lives;
    private int ControllerNumber;
    List<List<int>> playerLives;

    GameObject[] UIHeadsP1 = new GameObject[10];
    GameObject[] UIHeadsP2 = new GameObject[10];
    GameObject[] UIHeadsP3 = new GameObject[10];
    GameObject[] UIHeadsP4 = new GameObject[10];

    int[] Lives = new int[4];
    private bool[] spawnHeads = new bool[4];
    int dis = 0;
    int pPos1 = 0;
    int pPos2 = 0;
    int pPos3 = 0;
    int pPos4 = 0;
    int FunTracker = 0;
    int numberOfLives = 0;
    int OffsetX = -370;
    int OffsetY = -200;
    int OffsetSecY = -230;
    int Length = 20;
    int spacing = 30;

    void Awake()
    {
        playerLives = new List<List<int>>();
        player1Lives.gameObject.SetActive(false);
        player2Lives.gameObject.SetActive(false);
        player3Lives.gameObject.SetActive(false);
        player4Lives.gameObject.SetActive(false);

        

        pPos1 = CharacterMenuController.p1Pos; //Use this for spawning selected characters
        pPos2 = CharacterMenuController.p2Pos; //Use this for spawning selected characters
        pPos3 = CharacterMenuController.p3Pos; //Use this for spawning selected characters
        pPos4 = CharacterMenuController.p4Pos; //Use this for spawning selected characters

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            //Debug.Log(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("Xbox"))
            {
                ControllerNumber++;
                //Debug.Log("Controller " + ControllerNumber);
            }

            if (Input.GetJoystickNames()[j].Contains("XBOX"))
            {
                ControllerNumber++;
                //Debug.Log("Controller " + ControllerNumber);
            }

            //Debug.Log(ControllerNumber);

        }


        /*player1Lives.gameObject.SetActive(true);
        //Debug.Log(player1.GetComponent<DeathControl>().lives);
        if (CharacterMenuController.ControllerNumber > 1)
            player2Lives.gameObject.SetActive(true);
        if (CharacterMenuController.ControllerNumber > 2)
            player3Lives.gameObject.SetActive(true);
        if (CharacterMenuController.ControllerNumber > 3)
            player4Lives.gameObject.SetActive(true);*/



    }

    // Use this for initialization
    void Start()
    {


        for (int i = 0; i < ControllerNumber; i++)
        {
            //Debug.Log(i);
            spawnHeads[ControllerNumber] = false;
        }

        Invoke("Players", .1f);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ControllerNumber; i++)
        {
            Lives[i] = PlayerTracker.players[i].GetComponent<DeathControl>().getLives();
        }

        //numberOfLives = PlayerTracker.players[0].GetComponent<DeathControl>().getNumberOfLives();

        /*player1Lives.text = "Player 1 Lives: " + PlayerTracker.players[0].GetComponent<DeathControl>().getLives();
        //Debug.Log(player1.GetComponent<DeathControl>().getLives());
        if (CharacterMenuController.ControllerNumber > 1)
            player2Lives.text = "Player 2 Lives: " + PlayerTracker.players[1].GetComponent<DeathControl>().getLives();
        if (CharacterMenuController.ControllerNumber > 2)
            player3Lives.text = "Player 3 Lives " + PlayerTracker.players[2].GetComponent<DeathControl>().getLives();
        if (CharacterMenuController.ControllerNumber > 3)
            player4Lives.text = "Player 4 Lives " + PlayerTracker.players[3].GetComponent<DeathControl>().getLives();*/




    }

    public void death(int controllerNumber, int lives)
    {
        ControllerNumber = controllerNumber;
        //Debug.Log(ControllerNumber);
        Lives[controllerNumber - 1] = lives;
        //Debug.Log(lives);
        FunTracker++;
        //playerLives[controllernumber][lives];
        Players();
    }


    void Players()
    {
        //Debug.Log(Lives[ControllerNumber - 1]);
        //int b = 0;
        int lifes;

        if (FunTracker >= 1)
        {
            lifes = Lives[ControllerNumber - 1]; //If controllerNumber == 1  then that is equals lives[0]
            if (lifes == 10)
                Lives[ControllerNumber - 1] = 9;
            //Debug.Log(lifes);
            if (lifes < 0)
            {
                ;
            }
            else
            {
                if ((ControllerNumber - 1) == 0) 
                {
                    Debug.Log(lifes);
                    UIHeadsP1[lifes].gameObject.SetActive(false);
                }
                if ((ControllerNumber - 1) == 1)
                    UIHeadsP2[lifes].gameObject.SetActive(false);
                if ((ControllerNumber - 1) == 2)
                    UIHeadsP3[lifes].gameObject.SetActive(false);
                if ((ControllerNumber - 1) == 3)
                    UIHeadsP4[lifes].gameObject.SetActive(false);
                FunTracker = 0;
            }

        }


        for (int i = 0; i < ControllerNumber; i++)
        {
            //Debug.Log(i);
            //Debug.Log(ControllerNumber);
            //Debug.Log(Lives[i]); //This is one ahead for some reason so have to account for that later on also - 1 because we use 0
            if (spawnHeads[i] == false)
            {
                for (int b = 0; b < Lives[i]; b++)
                {
                    switch (i)
                    {
                        case 0:
                            spawnHeads[i] = true;
                            if (b == 0)
                                dis = 0;
                            else
                                dis += spacing;
                            if (b == 5)
                                dis = 0;
                            if (b <= 4)
                            {
                                UIHeadsP1[b] = Instantiate(HeadPlayers[pPos1], new Vector2(OffsetX + dis, OffsetY), Quaternion.identity) as GameObject;
                                UIHeadsP1[b].transform.SetParent(Canvas.transform, false);
                            }
                            if (b >= 5)
                            {
                                UIHeadsP1[b] = Instantiate(HeadPlayers[pPos1], new Vector2(OffsetX + dis, OffsetSecY), Quaternion.identity) as GameObject;
                                UIHeadsP1[b].transform.SetParent(Canvas.transform, false);
                            }
                            break;
                        case 1:
                            spawnHeads[i] = true;
                            if (b == 0)
                                dis = 0;
                            else
                                dis += spacing;
                            if (b == 5)
                                dis = 0;
                            if (b <= 4)
                            {
                                UIHeadsP2[b] = Instantiate(HeadPlayers[pPos2], new Vector2((OffsetX + (spacing * 5) + Length) + dis, OffsetY), Quaternion.identity) as GameObject;
                                UIHeadsP2[b].transform.SetParent(Canvas.transform, false);
                            }
                            if (b >= 5)
                            {
                                UIHeadsP2[b] = Instantiate(HeadPlayers[pPos2], new Vector2((OffsetX + (spacing * 5) + Length) + dis, OffsetSecY), Quaternion.identity) as GameObject;
                                UIHeadsP2[b].transform.SetParent(Canvas.transform, false);
                            }
                            break;
                        case 2:
                            spawnHeads[i] = true;
                            if (b == 0)
                                dis = 0;
                            else
                                dis += spacing;
                            if (b == 5)
                                dis = 0;
                            if (b <= 4)
                            {
                                UIHeadsP3[b] = Instantiate(HeadPlayers[pPos3], new Vector2((OffsetX + ((spacing * 5) * 2) + Length) + dis, OffsetY), Quaternion.identity) as GameObject;
                                UIHeadsP3[b].transform.SetParent(Canvas.transform, false);
                            }
                            if (b >= 5)
                            {
                                UIHeadsP3[b] = Instantiate(HeadPlayers[pPos3], new Vector2((OffsetX + ((spacing * 5) * 2) + Length) + dis, OffsetSecY), Quaternion.identity) as GameObject;
                                UIHeadsP3[b].transform.SetParent(Canvas.transform, false);
                            }
                            break;
                        case 3:
                            spawnHeads[i] = true;
                            if (b == 0)
                                dis = 0;
                            else
                                dis += spacing;
                            if (b == 5)
                                dis = 0;
                            if (b <= 4)
                            {
                                UIHeadsP4[b] = Instantiate(HeadPlayers[pPos4], new Vector2((OffsetX + ((spacing * 5) * 3) + Length) + dis, OffsetY), Quaternion.identity) as GameObject;
                                UIHeadsP4[b].transform.SetParent(Canvas.transform, false);
                            }
                            if (b >= 5)
                            {
                                UIHeadsP4[b] = Instantiate(HeadPlayers[pPos4], new Vector2((OffsetX + ((spacing * 5) * 3) + Length) + dis, OffsetSecY), Quaternion.identity) as GameObject;
                                UIHeadsP4[b].transform.SetParent(Canvas.transform, false);
                            }
                            break;
                        default:
                            break;
                    }
                    //Debug.Log(b);

                }


                //Debug.Log(i);


            }


        }

    }
}
