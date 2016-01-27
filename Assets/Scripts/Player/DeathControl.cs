using UnityEngine;
using System.Collections;

public class DeathControl : MonoBehaviour
{
    public delegate void DeathEvent(float respawnTime);
    public static event DeathEvent OnDeath;
    public delegate void RespawnEvent();
    public static event RespawnEvent OnRespawn;
    private Rigidbody player;
    public float bottomOfLevel; //The y position of the bottom of the level
    public float respawnTime;
    private bool doneRespawning = true;
    private Vector3 startPosition;
    private bool playerShieldBool = false;


    void Start()
    {
        startPosition = transform.position;
        player = GetComponent<Rigidbody>();
        //Debug.LogError(playerShieldBool);
    }

    void Update()
    {
        if (transform.position.y < bottomOfLevel && doneRespawning)
        {
            if(OnDeath != null)
                OnDeath(respawnTime);
            StartCoroutine(Respawn(respawnTime));
            doneRespawning = false;
        }
    }


    public void Kill()
    {
        if(doneRespawning)
        {
            if (OnDeath != null)
                OnDeath(respawnTime);
            StartCoroutine(Respawn(respawnTime));
            doneRespawning = false;
        }
    }

    IEnumerator Respawn(float respawnTime)
    {
        player.velocity = Vector3.zero;
        player.isKinematic = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        player.isKinematic = false;
        transform.position = startPosition;
        doneRespawning = true;
        if(OnRespawn != null)
            OnRespawn();
    }

    void OnTriggerEnter(Collider c)
    {
       
        if ((c.CompareTag("Bullet")) && (!GameObject.Find("Companion").GetComponent<companionScript>().playerShield))
        {
            Kill();
            GetComponent<TrapControl>().block.GetComponent<Collider>().enabled = true;
            GetComponent<TrapControl>().block.GetComponent<Collider>().isTrigger = true;
            GetComponent<TrapControl>().bullet.GetComponent<Collider>().enabled = true;
            GetComponent<TrapControl>().bullet.GetComponent<Collider>().isTrigger = true;
            
        }
        else if ((c.CompareTag("Bullet")) && (GameObject.Find("Companion").GetComponent<companionScript>().playerShield))
        {

            GetComponent<TrapControl>().block.GetComponent<Collider>().enabled = false;
            GetComponent<TrapControl>().bullet.GetComponent<Collider>().enabled = false;

        }
            
    }
}
