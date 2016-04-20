using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {

    public float speed = 5;
    //float bufferHeight;
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

    public void CloudEndLeft(string name)
    {
        cloud = name;
        for (int i = 1; i <= Clouds.Length; i++)
        {
            if (cloud.Contains(i.ToString()))
            {
                CloudNum = i;
                Debug.Log(CloudNum);
                ResetLeft(CloudNum);
            }
        }
    }

    public void CloudEnd(string name)
    {
        cloud = name;
        for (int i = 1; i <= Clouds.Length; i++)
        {
            if (cloud.Contains(i.ToString()))
            {
                CloudNum = i;
                Debug.Log(i);
                Reset(CloudNum);
            }
        }
    }

    void ResetLeft(int num)
    {
        Clouds[num].transform.position = new Vector3(startPos[CloudNum].x + 20, startPos[CloudNum].y, startPos[CloudNum].z);
    }

    void Reset(int num)
    {
  
        Clouds[num].transform.position = new Vector3(startPos[CloudNum].x - 20, startPos[CloudNum].y, startPos[CloudNum].z);

    }
}
