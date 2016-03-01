using UnityEngine;
using System.Collections;

public class perlinNoiseCamera : MonoBehaviour {

    public float value = 5;
    Vector3 offset;

    // Use this for initialization
    void Start()
    {

        offset = new Vector3(
    Random.Range(0, 30),
    Random.Range(0, 30),
    Random.Range(0, 30));

    }

    // Update is called once per frame
    void Update()
    {

        float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time, offset.z);

        Camera.main.backgroundColor = new Color(sampleX + 2, sampleY, sampleZ);

    }
}
