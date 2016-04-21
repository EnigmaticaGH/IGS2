using UnityEngine;
using System.Collections;

public class MoonController : MonoBehaviour {


    public GameObject[] Moons;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < Moons.Length; i++)
        {
            Moons[i].SetActive(false);
        }

        int randomNum = Random.Range(0, 2);

        if (randomNum == 0)
            Moons[0].SetActive(true);

        if (randomNum == 1)
            Moons[1].SetActive(true);

        if (randomNum == 2)
            Moons[2].SetActive(true);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
