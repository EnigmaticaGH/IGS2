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

    void Start()
    {
        startPosition = transform.position;
        player = GetComponent<Rigidbody>();
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

    IEnumerator Respawn(float respawnTime)
    {
        player.velocity = Vector3.zero;
        player.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        GetComponent<MeshRenderer>().enabled = true;
        player.isKinematic = false;
        transform.position = startPosition;
        doneRespawning = true;
        if(OnRespawn != null)
            OnRespawn();
    }

    void OnTriggerEnter(Collider c)
    {

    }
}
