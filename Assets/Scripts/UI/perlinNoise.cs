using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class perlinNoise : MonoBehaviour {

    public Image background;


    public float value = 5;
    Vector3 offset;

	// Use this for initialization
	void Start () {

        offset = new Vector3(
    Random.Range(-1, 10),
    Random.Range(-1, 10),
    Random.Range(-1, 10));
	
	}
	
	// Update is called once per frame
	void Update () {

        float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time, offset.z);

        background.color = new Color(sampleX, sampleY, sampleZ);
	
	}
}
