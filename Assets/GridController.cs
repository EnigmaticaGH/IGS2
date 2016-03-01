using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public GameObject[] Grid;
    Color color;

    public float value = 5;
    Vector3 offset;

	// Use this for initialization
	void Start () {

        offset = new Vector3(
            Random.Range(-1, 10),
            Random.Range(-1, 10),
            Random.Range(-1, 10));
        

        /*for (int i = 0; i < Grid.Length; i++)
        {
            color = Grid[i].GetComponent<Renderer>().color;
        }*/
	
	}
	
	// Update is called once per frame
	void Update () {

        float sampleX = Mathf.PerlinNoise(Time.time / value, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time * value, offset.z);

        for (int i = 0; i < Grid.Length; i++)
        {
            color = Grid[i].GetComponent<Renderer>().material.color;
            color = new Color(sampleX, sampleY, sampleZ);
            Grid[i].GetComponent<Renderer>().material.color = color;
        }
	
	}

    /*IEnumerator Color() 
    {

    }*/
}
