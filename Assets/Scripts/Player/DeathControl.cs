﻿using UnityEngine;
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
    private Transform trapBlock;
    private Transform trapBullet;

    void Start()
    {
        startPosition = transform.position;
        player = GetComponent<Rigidbody>();
        Debug.LogError(playerShieldBool);
        trapBlock = GameObject.Find("Player").GetComponent<TrapControl>().block.GetComponent<Transform>();
        trapBullet = GameObject.Find("Player").GetComponent<TrapControl>().bullet.GetComponent<Transform>(); 
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
            
        }
        else if ((c.CompareTag("Bullet")) && (GameObject.Find("Companion").GetComponent<companionScript>().playerShield))
        {
            Debug.LogError(GameObject.Find("Companion").GetComponent<companionScript>().playerShield);
            //DestroyImmediate(GameObject.Find("Player").GetComponent<TrapControl>().block, true);
            //DestroyImmediate(GameObject.Find("Player").GetComponent<TrapControl>().bullet, true);
            //GameObject.Find("Player").GetComponent<TrapControl>().block.transform.position = new Vector3(0, -100, 0);
            //GameObject.Find("Player").GetComponent<TrapControl>().bullet.transform.position = new Vector3(0, -100, 0);
            //Destroy(trapBlock);
            //Destroy(trapBullet);
            trapBullet.transform.position = new Vector3(0, -100, 0);
            trapBlock.transform.position = new Vector3(0, -100, 0);
            /*
             * ****Doesn't work with Insitated objects************
             */

        }
            
    }
}
