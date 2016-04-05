using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupControl : MonoBehaviour
{
    public static GameObject[] powerupSpawns;
    private List<GameObject> powerups;
    private GameObject currentPowerup;

    // Use this for initialization
    void Start()
    {
        powerupSpawns = GameObject.FindGameObjectsWithTag("PowerupSpawn");
        powerups = new List<GameObject>();
        foreach(GameObject g in Resources.LoadAll<GameObject>("Powerups"))
        {
            powerups.Add((GameObject)Instantiate(g, powerupSpawns[0].transform.position, Quaternion.identity));
        }
        foreach(GameObject g in powerups)
        {
            g.SetActive(false);
        }
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnPowerup()
    {
        int numPowerups = powerups.Count;
        int numSpawns = powerupSpawns.Length;
        while(true)
        {
            bool powerupAlreadySpawned = false;
            yield return new WaitForSeconds(Random.Range(8, 16));
            foreach (GameObject g in powerups)
            {
                powerupAlreadySpawned = powerupAlreadySpawned || g.activeSelf;
            }
            if (!powerupAlreadySpawned)
            {
                int powerup = Random.Range(0, numPowerups);
                int position = Random.Range(0, numSpawns);
                powerups[powerup].transform.position = 
                    powerupSpawns[position].transform.position;
                powerups[powerup].SetActive(true);
            }
        }
    }
}
