using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class perlinNoiseCamera : MonoBehaviour {

    public float value = 5;
    public Image LOGO;
    Vector3 offset;

    // Use this for initialization
    void Start()
    {

        offset = new Vector3(
    Random.Range(-10, 10),
    Random.Range(-10, 10),
    Random.Range(-10, 10));

    }

    // Update is called once per frame
    void Update()
    {

        float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        float sampleZ = Mathf.PerlinNoise(Time.time, offset.z);

        //Camera.main.backgroundColor = new Color(sampleX + 2, sampleY, sampleZ);
        LOGO.color = new Color(sampleX + .25f, sampleY + .3f, sampleZ + .25f);

    }
}
