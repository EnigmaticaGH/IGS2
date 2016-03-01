using UnityEngine;
using System.Collections;

public class lightsMenu : MonoBehaviour {

    public Light[] BottomLights;

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

        float sampleX = Mathf.PerlinNoise(Time.time * value, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time * value, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time / value, offset.z);

        for(int i = 0; i < BottomLights.Length; i++)
        {
            BottomLights[i].color = new Color(sampleX, sampleY, sampleZ);
            BottomLights[i].intensity = (sampleX + 6);

        }

	
	}
}
