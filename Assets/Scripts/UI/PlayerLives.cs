using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerLives : MonoBehaviour
{

    public Transform canvas1;
    public GameObject[] HeadPlayers;
    public Image[] HeadBackgrounds;
    public Sprite[] Backgrounds;
    public Transform[] SpawnUI;
    public Text player1Lives;
    public Text player2Lives;
    public Text player3Lives;
    public Text player4Lives;
    public Text[] DamageText;
    private int ControllerNumber;

    int[] Lives = new int[4];
    float[] Damage = new float[4];
    private bool[] spawnHeads = new bool[4];
    int pPos1 = 0;
    int pPos2 = 0;
    int pPos3 = 0;
    int pPos4 = 0;
    int FunTracker = 0;
    int activePlayers = 0;

    //public CharacterMenuController Control;

    GameObject[] Controllers;
    ControllerNumber[] cnumbers;
    DeathControl[] damages;

    void Start()
    {

        Controllers = GameObject.FindGameObjectsWithTag("Player");
        cnumbers = new ControllerNumber[Controllers.Length];
        damages = new DeathControl[Controllers.Length];
        for (int i = 0; i < Controllers.Length; i++)
        {
            cnumbers[i] = Controllers[i].GetComponent<ControllerNumber>();
            damages[i] = Controllers[i].GetComponent<DeathControl>();
        }

        //playerLives = new List<List<int>>();
        player1Lives.gameObject.SetActive(false);
        player2Lives.gameObject.SetActive(false);
        player3Lives.gameObject.SetActive(false);
        player4Lives.gameObject.SetActive(false);


        activePlayers = CharacterMenuController.playerSize.Count;

        //Debug.Log(activePlayers);

        for (int i = 0; i < HeadBackgrounds.Length; i++)
        {
            HeadBackgrounds[i].enabled = false;
        }


        for (int i = 0; i < activePlayers; i++)
        {
            //Debug.Log(i);
            spawnHeads[ControllerNumber] = false;
            switch (i)
            {
                case 0:
                    GameObject temp;
                    temp = (GameObject)Instantiate(HeadPlayers[CharacterMenuController.playerINDEX_Pos[0]], new Vector2(SpawnUI[i].transform.position.x, SpawnUI[i].transform.position.y), Quaternion.identity);
                    //temp.transform.parent = canvas1.transform;
                    temp.transform.SetParent(canvas1, true);
                    temp.transform.localScale = new Vector3(2, 2, 2);
                    break;
                case 1: 
                    GameObject temp1;
                    temp1 = (GameObject)Instantiate(HeadPlayers[CharacterMenuController.playerINDEX_Pos[1]], new Vector2(SpawnUI[i].transform.position.x, SpawnUI[i].transform.position.y), Quaternion.identity);
                    temp1.transform.SetParent(canvas1, true);
                    temp1.transform.localScale = new Vector3(2, 2, 2);

                    break;
                case 2: 
                    GameObject temp2;
                    temp2 = (GameObject)Instantiate(HeadPlayers[CharacterMenuController.playerINDEX_Pos[2]], new Vector2(SpawnUI[i].transform.position.x, SpawnUI[i].transform.position.y), Quaternion.identity);
                    temp2.transform.SetParent(canvas1, true);
                    temp2.transform.localScale = new Vector3(2, 2, 2);

                    break;
                case 3: 
                    GameObject temp3;
                    temp3 = (GameObject)Instantiate(HeadPlayers[CharacterMenuController.playerINDEX_Pos[3]], new Vector2(SpawnUI[i].transform.position.x, SpawnUI[i].transform.position.y), Quaternion.identity);
                    temp3.transform.SetParent(canvas1, true);
                    temp3.transform.localScale = new Vector3(2, 2, 2);
                    break;
                    
            }
        }

        Invoke("Players", .1f);



        player1Lives.gameObject.SetActive(true);
        //Debug.Log(player1.GetComponent<DeathControl>().lives);
        if (activePlayers > 1)
            player2Lives.gameObject.SetActive(true);
        if (activePlayers > 2)
            player3Lives.gameObject.SetActive(true);
        if (activePlayers > 3)
            player4Lives.gameObject.SetActive(true);



    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ControllerNumber && i < PlayerTracker.players.Length; i++)
        {
            Lives[i] = PlayerTracker.players[i].GetComponent<DeathControl>().getLives();
        }

        //numberOfLives = PlayerTracker.players[0].GetComponent<DeathControl>().getNumberOfLives();

        foreach (GameObject controller in Controllers)
        {

            //DamageText[0].text = "" + (Controllers.GetDamage() * 100).toString("0.00%");
        }


        player1Lives.text = "" + PlayerTracker.players[0].GetComponent<DeathControl>().getLives();
        //Debug.Log("Damage: " + (Damage * 100) + "%");
        
        HeadBackgrounds[0].enabled = true;
        HeadBackgrounds[0].sprite = Backgrounds[CharacterMenuController.playerINDEX_Pos[0]];
        
        if (activePlayers > 1)
        {
            player2Lives.text = "" + PlayerTracker.players[1].GetComponent<DeathControl>().getLives();
            
            HeadBackgrounds[1].enabled = true;
            HeadBackgrounds[1].sprite = Backgrounds[CharacterMenuController.playerINDEX_Pos[1]];

            if (activePlayers > 2)
            {
                player3Lives.text = "" + PlayerTracker.players[2].GetComponent<DeathControl>().getLives();
                HeadBackgrounds[2].enabled = true;
                HeadBackgrounds[2].sprite = Backgrounds[CharacterMenuController.playerINDEX_Pos[2]];
            }


            if (activePlayers > 3)
            {
                player4Lives.text = "" + PlayerTracker.players[3].GetComponent<DeathControl>().getLives();
                HeadBackgrounds[3].enabled = true;
                HeadBackgrounds[3].sprite = Backgrounds[CharacterMenuController.playerINDEX_Pos[3]];
            }
        }
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
        /*int lifes;

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

        }*/


        for (int i = 0; i < ControllerNumber; i++)
        {
            //Debug.Log(i);
            //Debug.Log(ControllerNumber);
            //Debug.Log(Lives[i]); //This is one ahead for some reason so have to account for that later on also - 1 because we use 0
            /*if (spawnHeads[i] == false)
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

                }*/


                //Debug.Log(i);


            }


        }

    }

