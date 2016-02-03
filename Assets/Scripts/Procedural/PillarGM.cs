using UnityEngine;
using System.Collections;

public class PillarGM : MonoBehaviour {

    public GameObject pillar;
    public GameObject pillar2;
    public GameObject pillar3;
    public GameObject Gate;
    public GameObject VasesGroup;

    //Honestly don't know why I use this variable, mainly for modification of positioning
    Vector3 newLocation;
    Vector3 newLocation2;
    Vector3 newLocation3;
    Vector3 newLocation4; 
    float randomNumber;
    int randomRot;

	// Use this for initialization
	void Start () {

        for (int z = 0; z <= 100; z = z + 5)
        {
            Instantiate(Gate, new Vector3(Gate.transform.position.x + z, Gate.transform.position.y, Gate.transform.position.z), Quaternion.Euler(270, 90, 0));
        }

        for (int i = 0; i <= 100; i++)
        {
            randomNumber = Random.Range(1, 10);
            randomRot = Random.Range(2,5);
           
            newLocation = new Vector3(pillar.transform.position.x + i, pillar.transform.position.y, pillar.transform.position.z);
            newLocation2 = new Vector3(pillar2.transform.position.x + i, pillar2.transform.position.y, pillar2.transform.position.z);
            newLocation3 = new Vector3(pillar3.transform.position.x + i, pillar3.transform.position.y, pillar3.transform.position.z);
            newLocation4 = new Vector4(VasesGroup.transform.position.x + i, VasesGroup.transform.position.y, VasesGroup.transform.position.z + 1);
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
            else if (randomNumber == 4)
            {
                //Instantiate(pillar, new Vector3(newLocation.x, newLocation.y, newLocation.z - .5f), Quaternion.Euler(270, 40, 0));
            }
            else if (randomNumber == 5)
            {
                //Instantiate(pillar2, new Vector3(newLocation2.x, newLocation2.y, newLocation2.z - .5f), Quaternion.Euler(270, 40, 0));
            }
            else if (randomNumber == 6)
            {
                //Instantiate(pillar3,  new Vector3(newLocation3.x, newLocation3.y, newLocation3.z - .5f), Quaternion.Euler(270, 40, 0));
            }
            else if (randomNumber == 7)
            {
                Instantiate(VasesGroup, newLocation4, Quaternion.Euler(0, randomRot * 10, 0));
            }

         }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
