﻿using UnityEngine;
using System.Collections;

public class DeathControl : MonoBehaviour
{
    private Transform upperBounds;
    private Transform lowerBounds;
    public delegate void OutOfLivesEvent(string sender);
    public static event OutOfLivesEvent OutOfLives;
    public delegate void DeathEvent(float respawnTime, string sender);
    public static event DeathEvent OnDeath;
    public delegate void RespawnEvent(string sender);
    public static event RespawnEvent OnRespawn;
    private Rigidbody player;
    public float respawnTime;
    private bool doneRespawning;
    //private Vector3 startPosition;
    public float Damage { get; private set; }

    public float invincibilityTime;
    public int maxHealth;
    private int health;
    public int numberOfLives;
    public int lives = 0;
    private bool invincible;
    private bool outOfLives;

    PlayerLives Lives;
    int controllerNumber;
    private ControllerNumber num;

    SpawnZonesController Zones;

    
    

    void Start()
    {
        Zones = GameObject.Find("SpawnZones").GetComponent<SpawnZonesController>();
        Lives = GameObject.Find("UI").GetComponentInChildren<PlayerLives>();
        num = GetComponent<ControllerNumber>();
        controllerNumber = num.GetComponent<ControllerNumber>().controllerNumber;
        //Debug.Log(controllerNumber);
        //startPosition = transform.position;
        player = GetComponent<Rigidbody>();
        if (lives < 1)
            lives = numberOfLives;
        health = maxHealth;
        invincible = false;
        outOfLives = false;
        upperBounds = GameObject.Find("UpperBounds").transform;
        lowerBounds = GameObject.Find("LowerBounds").transform;
        Damage = 0;
        doneRespawning = true;
    }

    void Update()
    {
        bool outOfBounds = transform.position.y < lowerBounds.position.y - 4 || transform.position.y > upperBounds.position.y + 4
            || transform.position.x < lowerBounds.position.x - 4 || transform.position.x > upperBounds.position.x + 4;
        if (outOfBounds && doneRespawning)
        {
            Kill();
        }
    }

    public void Hurt(int damage)
    {
        if (!invincible && !outOfLives)
        {
            health -= damage;
            Debug.Log("Player hurt for " + damage + " damage. Health: " + health);
            if (health <= 0) Kill();
            StartCoroutine(InvincibilityFrame(invincibilityTime));
        }
    }

    public void Kill()
    {
        if (doneRespawning)
        {
            health = maxHealth;
            if (--lives <= 0)
                RemoveFromGame();
            else
                Lives.death(controllerNumber, lives);
        }

        Debug.Log("Player killed. Lives: " + lives);
        if (doneRespawning && !outOfLives)
        {
            //if (OnDeath != null)
            //    OnDeath(respawnTime, name);
            GetComponent<GrabBlock>().OnDeath();
            GetComponent<StunParticles>().Die();
            GetComponent<DeathParticles>().Die();

            StartCoroutine(Respawn(respawnTime));
            doneRespawning = false;
        }
    }

    public void GodMode(int lifes)
    {
        lives = lifes;
    }

    public void Spawn()
    {
        transform.position = new Vector3(Zones.GetSafeSpawnX(), Zones.GetSafeSpawnY(), 0);
    }

    public float GetDamage()
    {
        return Damage;
    }

    IEnumerator Respawn(float respawnTime)
    {
        player.velocity = Vector3.zero;
        player.isKinematic = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<DynamicCollider>().Disable();
        GetComponent<PlayerAbilities>().RemovePowerup();
        yield return new WaitForSeconds(respawnTime);
        Damage = 0;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        player.isKinematic = false;
        player.useGravity = true;
        Spawn(); //New spawn system
        doneRespawning = true;
        GetComponent<DynamicCollider>().Enable();
        if (OnRespawn != null)
            OnRespawn(name);
    }

    void RemoveFromGame()
    {
        doneRespawning = true;
        outOfLives = true;
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

    public int getLives()
    {
        return lives;
    }

    public int getNumberOfLives()
    {
        return numberOfLives;
    }
    public void AddDamage(float damage)
    {
        Damage += damage;
    }
}
