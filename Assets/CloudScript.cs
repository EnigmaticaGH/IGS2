using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {

    public float speed = 5;
    float bufferHeight;
    public GameObject[] Clouds;
    Vector3[] startPos;
    string cloud;
    int CloudNum;

	// Use this for initialization
	void Start () {

        startPos = new Vector3[Clouds.Length];

        for (int i = 0; i < Clouds.Length; i++)
        {
            startPos[i] = new Vector3(Clouds[i].transform.position.x, Clouds[i].transform.position.y, Clouds[i].transform.position.z);
        }
	
	}
	
	// Update is called once per frame
	void Update () {





	}


    public void CloudEnd(string name)
    {
        //Debug.Log(name);
        cloud = name;
        for (int i = 1; i <= Clouds.Length; i++)
        {
            if (cloud.Contains(i.ToString()))
            {
                //Debug.LogError(i);
                CloudNum = i - 1;
                Reset(CloudNum);
            }
        }
    }

    void Reset(int num)
    {
        //Debug.Log("Hi");
        bufferHeight = Random.Range(2, 5);
  
        Clouds[num].transform.position = new Vector3(-30, startPos[CloudNum].y, startPos[CloudNum].z);


    }
}
