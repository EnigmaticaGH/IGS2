using UnityEngine;
using System.Collections;

public class companionScript : MonoBehaviour {
    
    public Transform companion;
    public Transform enemy;
    public float speed;
    public bool moveTowardsPlayer = false;
    public bool moveTowardsEnemy = false;
    public bool enemyFound = false;
    
    int randomMovementTimer;
    float randomNumber;
    bool mt = false;
    GameObject[] enemies;
    Vector3 followingRange;
    Vector3 enemyRange;

    void Awake()
    {
        //Use this stragery for finding GameObjects within the scene and adjust the companion to move towards the closest one. 


        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //This will register all enemies within the scene at Awake time
        //Durning runtime we'll have to add the enemies to the array 

       for(int i = 0; i <= enemies.Length; i++)
       {
           //enemies[i] = GameObject.FindGameObjectWithTag("Enemy");

          // Debug.LogError(enemies[i].transform.position);
       }



       

        
        

        

        
    }

    void Main()
    {

        randomMovementTimer = Random.Range(1, 3); //Random timer to adjust movement times
        
        if ((moveTowardsPlayer) && (moveTowardsEnemy == false))
        {
            Invoke("companionMovement", randomMovementTimer);


            /*
             *
              Works without corotine when moveTowardsEnemy is called movementAround player is stopped unitl ball finds enemy position and returns back to location
             * 
             * InvokeRepeating doesn't work instead used Invoke and set the movementTimer to random number
             *
             *************** Adjust speed while following player******************
             *
             //BUG FIXED * ******* BUG : When you press key to find enemy and while companion is on way back to player and you press key again the companion will go to enemy's position and stop*****************
             //BUG FIXED * ****** BUG CONT': Happens when you click key mutiple times *************
             * Adjust key for ability of companion finding an enemy set to a cooldown reduction, 1/20/16 you can click the key after the companion returns to player's location. 
             * 
             */
            //Replace this with corotine to enable actions
            //Once InvokeRepeating is called then it is continued to be called
            //Actions such as searching for an enemy can cause bug when timer goes off durning searching excution
            

        }
    }

    void Update()
    {

    }

    void companionMovement()
    {
        Debug.LogError("Companion movement called");
        
        mt = true;

        randomNumber = Random.Range(1.5f, 2.5f); //Movement distance behind player 

        Debug.Log(randomNumber);

    }

    void enemyLocationCheck()
    {
         enemyRange = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);

         for (int i = 0; i < enemies.Length; i++)
         {
             /*int small;
             if (enemies[i] < small)
             {
                 small = enemies[i];
             }*/
             /*float locationX;
             //float highestLocationX;
             locationX = enemies[i].transform.position.x;

             if(locationX < 0)
                 locationX = locationX * -1;

             //locationX = highestLocationX;

             float playerLocationX;
             playerLocationX = player.transform.position.x;

             float locationClosest;
             locationClosest = (locationX - player.transform.position.x);*/


         }

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
           
            followingRange = new Vector3(companion.position.x - randomNumber, transform.position.y, companion.position.z);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followingRange, step);
            if ((transform.position.x) == (companion.position.x - randomNumber))
            {
                mt = false;
                enemyFound = false;
                randomNumber = 0;
                Debug.Log(step);
                Main();
            }
        }
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Enemy")
        {
            Debug.Log("Collision Detected!");
            enemyFound = true;
            moveTowardsEnemy = false;
            moveTowardsPlayer = true;
        }
    }
}