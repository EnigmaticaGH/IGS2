﻿using UnityEngine;
using System.Collections;

public class companionScript : MonoBehaviour {
    
    public Transform player;
    public Transform enemy;
    public GameObject portal;
    public GameObject shield;
    public float speed;
    public bool moveTowardsPlayer = false;
    public bool moveTowardsEnemy = false;
    public bool enemyFound = false;
    public bool isGrounded = false;
    public bool isAlive = true;
    public bool playerShield = false;

    //private DeathControl playerLife;
    
    
    int randomMovementTimer;
    float randomNumber;
    bool mt = false;
    GameObject[] enemies;
    Vector3 followingRange;
    Vector3 enemyRange;
    int a = 0; //Controls portal timer
    int b = 0; //Controls shield timer
    int portalDuration= 3; //Duration
    int shieldTimer = 5; //Duration
    //Object portal1;
    

    void Awake()
    {
        //Use this stragery for finding GameObjects within the scene and adjust the companion to move towards the closest one. 

        enemyLocationCheck();

        
        //This will register all enemies within the scene at Awake time
        //Durning runtime we'll have to add the enemies to the array 

        for (int i = 0; i <= enemies.Length; i++)
        {
            //enemies[i] = GameObject.FindGameObjectWithTag("Enemy");

            // Debug.LogError(enemies[i].transform.position);
        }
       //Debug.Log("Hello");   
          
    }

    void Start()
    {
        shield.SetActive(false);

        isAlive = true;

        isGrounded = false;

        //playerLife = GameObject.Find("Player").GetComponent<DeathControl>();
    }

    void Main()
    {

        randomMovementTimer = Random.Range(1, 3); //Random timer to adjust movement times

        if ((moveTowardsPlayer) && (moveTowardsEnemy == false) && (enemyFound == false))
        {
            Invoke("companionMovement", randomMovementTimer);
          


        }

        if (enemyFound)
        {
            //Debug.LogError("OMG IT WORKED EJHENRENRUER");
            Invoke("companionMovement", .0001f);
        }


      
    }

    void companionMovement()
    {
        //Debug.LogError("Companion movement called");
        
        mt = true;

        randomNumber = Random.Range(.5f, 1.0f); //Movement distance behind player 

        //Debug.Log(randomNumber);

    }

    void enemyLocationCheck()
    {
        //Calculates enemies within scene at runtime
        //Using there x location and compares there value
        /*
         * *************This method will only work around the smallest x value meaning it doesn't go by closest to players x position*************
         * 
         * **********UPDATE : MAKE THIS WORK FOR CLOSEST TO PLAYER****************
         * *******1/22/16 GOES BY SMALLEST X VALUE NEEDS TO WORK CORSPONDING TO PLAYER/COMPANION'S POSITION*******
         */

         enemyRange = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
         enemies = GameObject.FindGameObjectsWithTag("Fear");
         
         float enemyX = 0;
         float smallest = enemies[0].transform.position.x;

         //Debug.Log(smallest);
        /*
         * *********Commented out because eneimies are disabled currently************** 1/25/2016
         */

         for (int i = 0; i < enemies.Length; i++)
         {
             Vector3 enemyLocaion = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y, enemies[i].transform.position.z);

             if (enemies[i].transform.position.x < 0)
             {
                 enemyX = enemies[i].transform.position.x;
                 enemyX = enemyX * -1;
             }
                 
             if ((enemies[i].transform.position.x < smallest) && (enemies[i].transform.position.x > 0))
             {
                 smallest = enemies[i].transform.position.x;

                 if (enemies[i].transform.position.x < 0)
                     enemyX = enemies[i].transform.position.x * -1;
                 
                 if (enemyX < smallest)
                     smallest = enemyX;

             }
             else if (enemies[i].transform.position.x < 0)
             {
                 enemyX = enemies[i].transform.position.x * -1;
                 if (enemyX < smallest)
                 {
                     smallest = enemyX;
                 }
             }

             //Debug.Log(enemies[i].transform.position);
         }

         //Debug.Log(smallest);
        
    }

    void Update()
    {
        /*
         ******Check if player is grounded, if player isn't grounded then he a portal can't be placed.
         */

        if ((Input.GetButton("X_1") || Input.GetKeyUp(KeyCode.M)) && (isGrounded))
        {
            a++;
            if (a == 1)
            {
                Instantiate(portal, new Vector3(player.transform.position.x, 0, 0), Quaternion.identity);
                
                Invoke("portalTimer", portalDuration); //Non-coroutine so totally uncool but works for now ****Pretty much cool down reduction for portal usuage.***
            }
           //if((a > 1) && (isGrounded))
               

            //Debug.LogError(a);
               
        }

        if ((Input.GetButton("B_1")) || (Input.GetKeyUp(KeyCode.N)))
        {
            b++;
            if (b == 1)
            {
                playerShield = true;

                //shield.transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
                //Instantiate(shield, new Vector3(player.position.x, player.position.y, 0), Quaternion.identity);
                shield.SetActive(true);

                Invoke("shieldTimerFN", shieldTimer);
            }
        }

      
    }

    void shieldTimerFN()
    {
        b = 0;

        playerShield = false;

        shield.SetActive(false);

        //shield.transform.position = new Vector3(0, -200, 0);
        //Destroy(shield);
    }

    void portalTimer()
    {
        a = 0;
    }

    void FixedUpdate()
    {
        

        if ((Input.GetButton("Y_1") || Input.GetKey(KeyCode.Alpha1)) && enemyFound == false)
        {
            moveTowardsEnemy = true;
            moveTowardsPlayer = false;
        }   

        if (moveTowardsEnemy)
        {
            enemyLocationCheck();
            transform.position = Vector3.MoveTowards(transform.position, enemyRange, speed * Time.deltaTime);
        }

        if (mt && (moveTowardsEnemy == false))
        {
           
            followingRange = new Vector3(player.position.x - randomNumber, transform.position.y, player.position.z);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followingRange, step);
            if ((transform.position.x) == (player.position.x - randomNumber))
            {
                mt = false;
                enemyFound = false;
                randomNumber = 0;
                //Debug.Log(step);
                Main();
            }
        }
        
    }

    void OnTriggerEnter(Collider c)
    {
        if ((c.gameObject.name == "Fear") || (c.CompareTag("Fear")))
        {
            Debug.Log("Collision Detected!");
            if(moveTowardsEnemy)
                enemyFound = true;
            moveTowardsEnemy = false;
            moveTowardsPlayer = true;
            Main();
        }
    }

    public void MovementStateChange(string state)
    {
        isGrounded = state == "GROUND";
        //Debug.LogError("IS PLAYER GROUNDED:" + isGrounded);
    }
}



/*
 *
  Works without corotine when moveTowardsEnemy is called movementAround player is stopped unitl ball finds enemy position and returns back to location
 * 
 * InvokeRepeating doesn't work instead used Invoke and set the movementTimer to random number
 *
 *************** Adjust speed while following player******************
 * 
 */
