using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnZones : MonoBehaviour {

    public GameObject[] startingObjects = new GameObject[10];
    [SerializeField]
    public static List<GameObject> ObjectsZone = new List<GameObject>();
    //private GameObject[] newObjects = new GameObject[50];
    public int Zone = 0;
    int i = 0;
    int StartCount = 0;
    int NewCount = 0;
    public bool start = false;
    int zoneBlocks;
    int playerZone;
    int count = 0;
    public int UFO = 0;

    public List<int> zonePlayers = new List<int>();

    [SerializeField]
    public static List<string> Name_Objects;
    string[] Objects =
        {
            "Player",
            "STARTCUBE_",
            "Powerup"
        };


    private string[] Obj = new string[]
    {
        "Player",
        "STARTCUBE_"
    };

    public bool Safe = true;
    public bool ForeignObject = false;


    void FixedUpdate()
    {
        UFO = ObjectsZone.Count;
    }


    void OnTriggerEnter(Collider col)
    {

        

        if (start == false)
        {
            StartCoroutine(startSomeObject(col));
        }

            if (start == true)
            {
                ObjectsZone.Add(col.gameObject);
                ListCheck(ObjectsZone.Count);


                if (col.tag == "Player")
                    zonePlayers.Add(1);

                Checker(col);

                if (col.name.Contains("STARTCUBE_" + Zone))
                {
                    NewCount++;
                    if (NewCount == StartCount)
                    {
                        Safe = true;
                    }
                    NewObjects(NewCount);
                }

                if (col.name.Contains("STARTCUBE_" + Zone) == false)
                {
                    if (col.tag == "player")
                        ;
                    else
                    {
                        zoneBlocks++;
                        SafeCheck();
                    }

                }

            }
        }
    
    

    void OnTriggerStay(Collider col)
    {
       if(col.tag == "Player")
           Checker(col);
        

       
    }

    IEnumerator startSomeObject(Collider col)
    {
        if (col.tag == "Block")
        {

            startingObjects[i] = col.gameObject;
            startingObjects[i].name = startingObjects[i].name + "STARTCUBE_" + Zone;
            i++;


        }
        yield return new WaitForSeconds(.1f);

        start = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (start == true)
        {

                ObjectsZone.Remove(col.gameObject);
                ListCheck(ObjectsZone.Count);




            CheckerExit(col);

            if (col.name.Contains("STARTCUBE_" + Zone))
            {
                NewCount--;
                
                NewObjects(NewCount);
            }

            if (col.name.Contains("STARTCUBE_" + Zone) == false)
            {
                if ((col.tag == "Player") || (col.CompareTag("Powerup") && col.name == "DropPowerup(Clone)") || (col.CompareTag("Powerup") && col.name == "GrenadePowerup(Clone)"))
                    ;
                else
                {
                    zoneBlocks--;
                    SafeCheck();
                }
            }
        }
    }


    void ListCheck(int count)
    {
        int newObjects = count;

        //Debug.LogError(count);

        if (count == 0)
        {
            //Debug.LogError("ZERO OBJECTS IN ZONE------ " + Zone);
        }

        if (count > 0)
        {
            //Debug.LogError("ZONE HAS MORE AN OBJECT IN IT " + Zone);
        }
    }

    

    void StartingCubes(GameObject cube)
    {
        if (cube.tag == "Block")
        {
            count++;
        }
    }

    void SafeCheck()
    {
        //Debug.Log(zoneBlocks);
        if (zoneBlocks == 0)
        {
            ForeignObject = false;
        }
        if (zoneBlocks > 0)
        {
            ForeignObject = true;
        }
        if (ForeignObject == true)
        {
            Safe = false;
        }
        if (ForeignObject == false)
        {
            if (NewCount == StartCount)
                Safe = true;
        }
    }

    void StartingObjects()
    {
        StartCount = i;
        start = true;
        NewCount = StartCount;
    }

    void NewObjects(int count)
    {
        if (count < StartCount)
            Safe = false;
        if (count == StartCount)
        {
            Safe = true;
        }
    }

    void Checker(Collider col)
    {

        if (col.gameObject.tag.Contains(Obj[0]))
        {
            Safe = false;
        }
        else
            Safe = true;
            

    }

    void CheckerExit(Collider col)
    {
        if (col.gameObject.tag.Contains(Obj[0]))
        {
            Safe = true;
        }

    }


}
