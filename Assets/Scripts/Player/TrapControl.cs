using UnityEngine;
using System.Collections;

public class TrapControl : MonoBehaviour
{
    [System.Serializable]
    public struct Trap
    {
        public string Name;
        public string Button;
        public float Cooldown;
        [HideInInspector]
        public bool Activated;
        [HideInInspector]
        public GameObject[] Objects;
    }
    public Trap[] traps;
    public delegate void TrapInit(string[] names);
    public static event TrapInit TrapInitEvent;
    public delegate void TrapStatus(string status, int index, float time);
    public static event TrapStatus TrapStatusEvent;

    private Vector3 posX;
    private Vector3 pos;
    public GameObject block;
    public GameObject wall;
    public GameObject bullet;

    private DeathControl playerLife;

    // Use this for initialization
    void Start()
    {
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
            names[i] = traps[i].Name;
        }

        TrapInitEvent(names);

        playerLife = GameObject.Find("Player").GetComponent<DeathControl>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < traps.Length; i++)
        {
            if (Input.GetButton(traps[i].Button) && !traps[i].Activated)
            {
                StartCoroutine("Trap" + i + "Activate");
            }
        }

        posX = new Vector3(transform.position.x, 1, 0);
        pos = transform.position;
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
        TrapStatusEvent("Active", 0, traps[0].Cooldown);
        traps[0].Activated = true;

        traps[0].Objects[0].SetActive(true);
        traps[0].Objects[0].transform.position = pos + Vector3.right * 2.5f + Vector3.up * 0.25f;

        TrapStatusEvent("Cooldown", 0, traps[0].Cooldown);
        yield return new WaitForSeconds(traps[0].Cooldown);
        TrapStatusEvent("Ready", 0, traps[0].Cooldown);
        ResetObject(traps[0].Objects[0]);
        traps[0].Activated = false;
    }
    //confine player
    IEnumerator Trap1Activate()
    {
        float netCooldownTime;
        TrapStatusEvent("Active", 1, traps[1].Cooldown);
        traps[1].Activated = true;

        traps[1].Objects[0].transform.position = posX + Vector3.right * 2f + Vector3.up * -5f;
        traps[1].Objects[1].transform.position = posX + Vector3.left * 2f + Vector3.up * -3.5f;
        traps[1].Objects[0].SetActive(true);
        traps[1].Objects[1].SetActive(true);

        //Shoot up from the bottom of the screen quickly
        for(int i = 0; i < 20; i++)
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
        if (posX.x < traps[1].Objects[0].transform.position.x 
            && posX.x > traps[1].Objects[1].transform.position.x
            && pos.y < traps[1].Objects[1].transform.position.y + 2.5f)
        {
            netCooldownTime = traps[1].Cooldown - playerLife.respawnTime;
            playerLife.Kill();
            yield return new WaitForSeconds(playerLife.respawnTime);
            traps[1].Objects[0].SetActive(false);
            traps[1].Objects[1].SetActive(false);
        }
        else
        {
            netCooldownTime = traps[1].Cooldown;
        }
        //start trap cooldown
        TrapStatusEvent("Cooldown", 1, traps[1].Cooldown);
        yield return new WaitForSeconds(netCooldownTime);
        TrapStatusEvent("Ready", 1, traps[1].Cooldown);
        traps[1].Objects[0].SetActive(false);
        traps[1].Objects[1].SetActive(false);
        traps[1].Activated = false;
    }
    //Shoot bullet from left to right
    IEnumerator Trap2Activate()
    {
        TrapStatusEvent("Active", 2, traps[2].Cooldown);
        traps[2].Activated = true;

        traps[2].Objects[0].SetActive(true);
        traps[2].Objects[0].transform.position = posX + Vector3.left * 10f + Vector3.down * 0.5f;

        for (int i = 0; i < 200; i++)
        {
            traps[2].Objects[0].transform.position += Vector3.right * 0.2f;
            yield return new WaitForFixedUpdate();
        }
        TrapStatusEvent("Cooldown", 2, traps[2].Cooldown);
        yield return new WaitForSeconds(traps[2].Cooldown);
        TrapStatusEvent("Ready", 2, traps[2].Cooldown);
        ResetObject(traps[2].Objects[0]);
        traps[2].Activated = false;
    }
}
