using UnityEngine;
using System.Collections;

public class PillarGM : MonoBehaviour {

    public GameObject pillar;
    public GameObject pillar2;
    public GameObject pillar3;
    public GameObject Gate;

    Vector3 newLocation;
    Vector3 newLocation2;
    Vector3 newLocation3;
    float randomNumber;

	// Use this for initialization
	void Start () {

        for (int z = 0; z <= 100; z = z + 5)
        {
            Instantiate(Gate, new Vector3(Gate.transform.position.x + z, Gate.transform.position.y, Gate.transform.position.z), Quaternion.Euler(270, 90, 0));
        }

        for (int i = 0; i <= 100; i++)
        {
            randomNumber = Random.Range(1, 10);
            newLocation = new Vector3(pillar.transform.position.x + i, pillar.transform.position.y, pillar.transform.position.z);
            newLocation2 = new Vector3(pillar2.transform.position.x + i, pillar2.transform.position.y, pillar2.transform.position.z);
            newLocation3 = new Vector3(pillar3.transform.position.x + i, pillar3.transform.position.y, pillar3.transform.position.z);
            if (randomNumber == 1)
            {
                 Instantiate(pillar, newLocation, Quaternion.Euler(270, 40, 0));
            }
            else if (randomNumber == 2)
             {
                 Instantiate(pillar2, newLocation2, Quaternion.Euler(270, 40, 0));
             }
             else if (randomNumber == 3)
             {
                 Instantiate(pillar3, newLocation3, Quaternion.Euler(270, 40, 0));
             }

         }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
