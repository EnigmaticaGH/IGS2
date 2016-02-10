using UnityEngine;
using System.Collections;

public class DeathControl : MonoBehaviour
{
    public delegate void OutOfLivesEvent(string sender);
    public static event OutOfLivesEvent OutOfLives;
    public delegate void DeathEvent(float respawnTime);
    public static event DeathEvent OnDeath;
    public delegate void RespawnEvent();
    public static event RespawnEvent OnRespawn;
    private Rigidbody player;
    public float bottomOfLevel; //The y position of the bottom of the level
    public float respawnTime;
    private bool doneRespawning = true;
    private Vector3 startPosition;

    public float invincibilityTime;
    public int maxHealth;
    private int health;
    public int numberOfLives;
    private int lives;
    private bool invincible;

    void Start()
    {
        startPosition = transform.position;
        player = GetComponent<Rigidbody>();
        lives = numberOfLives;
        health = maxHealth;
        invincible = false;
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

    public void Hurt(int damage)
    {
        if (!invincible)
        {
            health -= damage;
            Debug.Log("Player hurt for " + damage + " damage. Health: " + health);
            if (health <= 0) Kill();
            StartCoroutine(InvincibilityFrame(invincibilityTime));
        }
    }

    void Kill()
    {
        health = maxHealth;
        if (--lives < 0)
            RemoveFromGame();
        Debug.Log("Player killed. Lives: " + lives);
        if (doneRespawning)
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

    void RemoveFromGame()
    {
        Debug.Log(name + " KOed!");
        if(OutOfLives != null)
            OutOfLives(name);
    }

    IEnumerator InvincibilityFrame(float t)
    {
        invincible = true;

        yield return new WaitForSeconds(t);

        invincible = false;
    }
}
