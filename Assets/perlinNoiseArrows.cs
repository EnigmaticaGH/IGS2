using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class perlinNoiseArrows : MonoBehaviour {

    public Image arrow;


    public float value = 2;
    Vector3 offset;


	// Use this for initialization
	void Start () {

        offset = new Vector3(
Random.Range(-1, 10),
Random.Range(-1, 10),
Random.Range(2, 5));
	
	}
	
	// Update is called once per frame
	void Update () {

        float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time, offset.z);
        //sampleX = Mathf.Clamp(0, .3f);

        arrow.GetComponent<RectTransform>().sizeDelta = new Vector2((sampleX * (value * 2)) + 215, (sampleY * (value / 2)) + 100);

	
	}
}
