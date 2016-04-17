using UnityEngine;
using System.Collections;

public class TrapControl : MonoBehaviour
{
    [System.Serializable]
    public struct Trap
    {
        public string Name;
        public string Button;
        [HideInInspector]
        public string ControllerNumber;
        public float Cooldown;
        [HideInInspector]
        public bool Activated;
        [HideInInspector]
        public GameObject[] Objects;
    }
    public Trap[] traps;
    public delegate void TrapInit(string[] names, string name);
    public static event TrapInit TrapInitEvent;
    public delegate void TrapStatus(string sender, string status, int index, float time);
    public static event TrapStatus TrapStatusEvent;

    private Vector3 posX;
    private Vector3 pos;
    public GameObject block;
    public GameObject wall;
    public GameObject bullet;
    public GameObject[] parkour;
    private GameObject temp;

    public GameObject player;
    private DeathControl playerLife;
    private Rigidbody playerRB;
    

    private ControllerNumber controller;

    void Awake()
    {
        //DeathControl.OnRespawn += ResetAllTraps;
    }

    void OnDestroy()
    {
        //DeathControl.OnRespawn -= ResetAllTraps;
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<ControllerNumber>();
        for (int i = 0; i < traps.Length; i++)
        {
            traps[i].Activated = false;
        }

        

        traps[0].Objects = new GameObject[1]
        {
            (GameObject)Instantiate(block, Vector3.zero, Quaternion.identity)
        };
        traps[0].Objects[0].SetActive(false);

        traps[1].Objects = new GameObject[2]
        {
            (GameObject)Instantiate(wall, Vector3.zero, Quaternion.identity),
            (GameObject)Instantiate(wall, Vector3.zero, Quaternion.identity)
        };
        traps[1].Objects[0].SetActive(false);
        traps[1].Objects[1].SetActive(false);

        traps[2].Objects = new GameObject[1]
        {
            (GameObject)Instantiate(bullet, Vector3.zero, Quaternion.identity)
        };
        traps[2].Objects[0].SetActive(false);

        string[] names = new string[traps.Length];
        for(int i = 0; i < traps.Length; i++)
        {
            traps[i].ControllerNumber = controller.controllerNumber.ToString();
            names[i] = traps[i].Button +  " - " + traps[i].Name;
        }

        TrapInitEvent(names, name);

        playerLife = player.GetComponent<DeathControl>();
        playerRB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < traps.Length; i++)
        {
            traps[i].ControllerNumber = controller.controllerNumber.ToString();
            if (Input.GetButton(traps[i].Button + "_" + traps[i].ControllerNumber) && !traps[i].Activated)
            {
                StartCoroutine("Trap" + i + "Activate");
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && !traps[0].Activated)
        {
            StartCoroutine("Trap0Activate");
        }
        if (Input.GetKeyDown(KeyCode.W) && !traps[1].Activated)
        {
            StartCoroutine("Trap1Activate");
        }
        if (Input.GetKeyDown(KeyCode.E) && !traps[2].Activated)
        {
            StartCoroutine("Trap2Activate");
        }
        if (Input.GetKeyDown(KeyCode.R) && !traps[3].Activated)
        {
            StartCoroutine("Trap3Activate");
        }

        posX = new Vector3(player.transform.position.x, 1, 0);
        pos = player.transform.position;

        //Companion shield script to active or deactive collider of walls depending on shield bool
        if ((GameObject.Find("Companion").GetComponent<companionScript>().playerShield == false))
        {
            traps[1].Objects[0].GetComponent<Collider>().enabled = true;
            traps[1].Objects[1].GetComponent<Collider>().enabled = true;

        }
        else if ((GameObject.Find("Companion").GetComponent<companionScript>().playerShield))
        {
            traps[1].Objects[0].GetComponent<Collider>().enabled = false;
            traps[1].Objects[1].GetComponent<Collider>().enabled = false;
            //Debug.LogError("Player Sheild ON COLLIDER ARE SUPPOSED TO BE OFF ON WALLS");
        } 

    }

    void ResetObject(GameObject g)
    {
        if(g.GetComponent<Rigidbody>() != null)
        {
            g.GetComponent<Rigidbody>().velocity = Vector3.zero;
            g.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        g.transform.rotation = Quaternion.identity;
        g.SetActive(false);
    }

    //Spawn block in front of player
    IEnumerator Trap0Activate()
    {
        float playerHeight = 1;
        float spawnHeight = 5;
        float distance = spawnHeight - playerHeight;
        float gravity = Mathf.Abs(Physics.gravity.y);
        float timeToFall = Mathf.Sqrt(2 * distance / gravity);
        float distanceTraveledByPlayer = playerRB.velocity.x * timeToFall;

        Vector3 targetPos = pos + Vector3.right * distanceTraveledByPlayer;
        TrapStatusEvent(name, "Active", 0, traps[0].Cooldown);
        traps[0].Activated = true;

        traps[0].Objects[0].SetActive(true);
        traps[0].Objects[0].transform.position = targetPos + Vector3.up * spawnHeight;

        TrapStatusEvent(name, "Cooldown", 0, traps[0].Cooldown);
        yield return new WaitForSeconds(traps[0].Cooldown);
        TrapStatusEvent(name, "Ready", 0, traps[0].Cooldown);
        ResetObject(traps[0].Objects[0]);
        traps[0].Activated = false;
    }
    //confine player
    IEnumerator Trap1Activate()
    {
        TrapStatusEvent(name,"Active", 1, 0);
        traps[1].Activated = true;
        traps[1].Objects[0].transform.rotation = Quaternion.Euler(-90, 90, 0);
        traps[1].Objects[1].transform.rotation = Quaternion.Euler(-90, 90, 0);
        traps[1].Objects[0].transform.position = posX + Vector3.right * 2f + Vector3.up * -5f;
        traps[1].Objects[1].transform.position = posX + Vector3.left * 2f + Vector3.up * -3.5f;
        traps[1].Objects[0].SetActive(true);
        traps[1].Objects[1].SetActive(true);

        //Shoot up from the bottom of the screen quickly
        for(int i = 0; i < 10; i++)
        {
            traps[1].Objects[0].transform.position += Vector3.up * 0.25f;
            traps[1].Objects[1].transform.position += Vector3.up * 0.25f;
            yield return new WaitForFixedUpdate();
        }
        //stay in position for a bit
        for(int i = 0; i < 20; i++) { yield return new WaitForFixedUpdate(); }
        //slowly move in on the player
        for(int i = 0; i < 144; i++)
        {
            traps[1].Objects[0].transform.position += Vector3.left * 0.01f;
            traps[1].Objects[1].transform.position += Vector3.right * 0.01f;
            yield return new WaitForFixedUpdate();
        }
        //if the player is caught inside when the walls have finished collapsing,
        //kill the player
        /*
         ***********Will have to adjust this for player's shield****************
         ***********HOLY CRAP FIX THIS ROMAN IT ISN'T DAT HARD************
         */

        if (posX.x < traps[1].Objects[0].transform.position.x
            && posX.x > traps[1].Objects[1].transform.position.x)
        {
            playerLife.Hurt(1);
        }
       
        
        //not working right - fixed collider for wall when player's sheild equals false
        
        //start trap cooldown
        TrapStatusEvent(name, "Cooldown", 1, traps[1].Cooldown);
        yield return new WaitForSeconds(traps[1].Cooldown);
        TrapStatusEvent(name, "Ready", 1, 0);
        traps[1].Objects[0].SetActive(false);
        traps[1].Objects[1].SetActive(false);
        traps[1].Activated = false;
    }
    //Shoot bullet from left to right
    IEnumerator Trap2Activate()
    {
        TrapStatusEvent(name, "Active", 2, traps[2].Cooldown);
        traps[2].Activated = true;

        traps[2].Objects[0].SetActive(true);
        traps[2].Objects[0].transform.position = posX + Vector3.left * 10f + Vector3.down * 0.5f;

        for (int i = 0; i < 200; i++)
        {
            traps[2].Objects[0].transform.position += Vector3.right * 0.14f;
            yield return new WaitForFixedUpdate();
        }
        TrapStatusEvent(name, "Cooldown", 2, traps[2].Cooldown);
        yield return new WaitForSeconds(traps[2].Cooldown);
        TrapStatusEvent(name, "Ready", 2, traps[2].Cooldown);
        ResetObject(traps[2].Objects[0]);
        traps[2].Activated = false;
    }

    IEnumerator Trap3Activate()
    {
        TrapStatusEvent(name, "Active", 3, traps[3].Cooldown);
        traps[3].Activated = true;

        int randomNumber = Random.Range(0, 9);
        Debug.Log("Trap 3 activated" + randomNumber);
        int randomParkourLayout = Random.Range(0, parkour.Length);

       

        if (randomNumber == 0)
        {
            temp = Instantiate(parkour[0], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 1)
        {
            temp = Instantiate(parkour[0], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 2)
        {
            temp = Instantiate(parkour[0], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 3)
        {
            temp = Instantiate(parkour[1], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 4)
        {
            temp = Instantiate(parkour[1], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 5)
        {
            temp = Instantiate(parkour[1], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 6)
        {
            temp = Instantiate(parkour[2], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 7)
        {
            temp = Instantiate(parkour[2], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 8)
        {
            temp = Instantiate(parkour[3], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }
        else if (randomNumber == 9)
        {
            temp = Instantiate(parkour[3], new Vector3(player.transform.position.x + 10, parkour[randomParkourLayout].transform.position.y, player.transform.position.z), Quaternion.identity) as GameObject;
        }

        TrapStatusEvent(name, "Cooldown", 3, traps[3].Cooldown);
        yield return new WaitForSeconds(traps[3].Cooldown);
        TrapStatusEvent(name, "Ready", 3, traps[3].Cooldown);
        traps[3].Activated = false;
        Destroy(temp);
    }

    void ResetAllTraps()
    {
        foreach(Trap t in traps)
        {
            foreach(GameObject g in t.Objects)
            {
                g.SetActive(false);
            }
        }
    }
}
