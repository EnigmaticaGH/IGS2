using UnityEngine;
using System.Collections;

public class UILevelSelect : MonoBehaviour {

    public Transform RotationPoint;
    public GameObject[] Levels;
    public float speed = 10;
    int b = 0;

    Vector3 pos1;
    Vector3 pos2;

	// Use this for initialization
	void Start () {

        pos1 = new Vector3(Levels[0].transform.position.x, Levels[0].transform.position.y, Levels[0].transform.position.z);
        pos2 = new Vector3(Levels[1].transform.position.x, Levels[1].transform.position.y, Levels[1].transform.position.z);

	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            for (int i = 0; i < Levels.Length; i++)
            {
                if (pos1 != pos2)
                {
                    Levels[i].transform.RotateAround(RotationPoint.transform.position, Vector3.up, Time.deltaTime * speed);
                }
            }

        }
	
	}

    void rotateLevel()
    {

        
            //Levels[0].transform.RotateAround(RotationPoint.transform.position, Vector3.up, Time.deltaTime * speed);

    }
}
