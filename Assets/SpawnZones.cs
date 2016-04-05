using UnityEngine;
using System.Collections;

public class SpawnZones : MonoBehaviour {

    public GameObject[] startingObjects = new GameObject[10];
    //private GameObject[] newObjects = new GameObject[50];
    public int Zone = 0;
    int i = 0;
    int StartCount = 0;
    int NewCount = 0;
    bool start = false;
    int zoneBlocks;
    int playerZone;
    int count = 0;


    private string[] Obj = new string[]
    {
        "Player"
    };

    public bool Safe = true;
    public bool ForeignObject = false;

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
    void OnTriggerEnter(Collider col)
    {
        if(start == false)
        {

            startingObjects[i] = col.gameObject;
            startingObjects[i].name = startingObjects[i].name + "STARTCUBE_" + Zone;
            i++;
            Invoke("StartingObjects",.1f); //Let it loop thru for a milisecond
        }

        if (start == true)
        {

            if(col.tag == "Player")
                playerZone++;

            Checker(col);

            if (col.name.Contains("STARTCUBE_" + Zone))
            {
                NewCount++;
                //Debug.Log(col.name);
                if (NewCount == StartCount)
                {
                    Safe = true;
                }
                NewObjects();
            }
            
            if(col.name.Contains("STARTCUBE_" + Zone) == false)
            {
                //Debug.Log("Foreign Object detected" + col.name);
                zoneBlocks++;
                ForeignObject = true;
                SafeCheck();
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (start == true)
        {
            if (col.tag == "Player")
                playerZone--;

            Checker(col);

            if (col.name.Contains("STARTCUBE_" + Zone))
            {
                NewCount--;
                //Debug.Log(col.name);
                NewObjects();
            }

            if (col.name.Contains("STARTCUBE_" + Zone) == false)
            {
                zoneBlocks--;
                ForeignObject = false;
                SafeCheck();
            }
        }
    }
    

    void StartingCubes(GameObject cube)
    {
        if (cube.tag == "Block")
        {
            count++;
            //Debug.Log(count);
        }
    }

    void SafeCheck()
    {
        if (ForeignObject == true)
        {
            Safe = false;
        }
        if (ForeignObject == false)
        {
            if(NewCount == StartCount)
                Safe = true;
        }
    }

    void StartingObjects()
    {
        StartCount = i;
        start = true;
        NewCount = StartCount;
        //Debug.Log(StartCount +" " +transform.gameObject.name);
    }

    void NewObjects()
    {
        if (NewCount < StartCount)
            Safe = false;
        if (NewCount >= StartCount)
        {
            Safe = true;
        }
        //Debug.Log(NewCount);
        //Debug.Log(Safe + transform.gameObject.name);
    }

    void Checker(Collider col)
    {

        if (col.gameObject.tag.Contains(Obj[0]))
        {
            //Debug.Log(playerZone);

            if (playerZone == 0)
                Safe = true;
            Safe = false;
        }
            

    }
}
