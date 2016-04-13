using UnityEngine;
using System.Collections;

public class SpawnZonesController : MonoBehaviour {

    public GameObject[] SpawnZonesOBJ;

    SpawnZones[] SafeZ = new SpawnZones[10];

    bool[] safe = new bool[10];

    //Vector2 safeLocation;

    float safeX = 0;
    float safeY = 0;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < SpawnZonesOBJ.Length; i++)
        {
            SafeZ[i] = SpawnZonesOBJ[i].GetComponent<SpawnZones>();

        }
	
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < SpawnZonesOBJ.Length; i++)
        {
            safe[i] = SafeZ[i].Safe;
            //Debug.Log(safe[i]);
            //Debug.Log(SpawnZonesOBJ);
            if (safe[i] == false)
            {
                //Debug.Log(safe[i]);
            }

            if (safe[i])
            {
                SafeSpawnX(SpawnZonesOBJ[i].transform.position.x + Random.RandomRange(-1, 1));
                SafeSpawnY(SpawnZonesOBJ[i].transform.position.y + .5f);
            }
        }
	
	}

    public void SafeSpawnX(float x)
    {
        safeX = x;
        return;
    }

    public float GetSafeSpawnX()
    {
        return safeX;
    }

    public float SafeSpawnY(float y)
    {
        safeY = y;
        return y;
    }

    public float GetSafeSpawnY()
    {
        return safeY;
    }
}
