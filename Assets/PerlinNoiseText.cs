using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PerlinNoiseText : MonoBehaviour {

    public Text title;

    public float value = 5;
    Vector3 offset;

    // Use this for initialization
    void Start()
    {


        offset = new Vector3(
    Random.Range(-value, 30),
    Random.Range(-value, 30),
    Random.Range(-value, 30));

    }

    // Update is called once per frame
    void Update()
    {


        float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        //float sampleY = Mathf.PerlinNoise(Time.time, offset.y);

        //title.GetComponent<RectTransform>().sizeDelta = new Vector2((sampleX * (value * 2)) + 130, (sampleY * (value)) + 40);
        title.color = new Color(title.color.r, title.color.g, title.color.b, sampleX + .25f);


    }
}
