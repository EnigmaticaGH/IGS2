using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffectsStartTextUI : MonoBehaviour {

    public Text title;
    public GameObject size;

    public float value = 5;
    //Vector3 offset;

    // Use this for initialization
    void Start()
    {


        //offset = new Vector3(
        //Random.Range(-value, 30),
        //Random.Range(-value, 30),
        //Random.Range(-value, 30));

    }

    // Update is called once per frame
    void Update()
    {


        //float sampleX = Mathf.PerlinNoise(Time.time, offset.x);
        //float sampleY = Mathf.PerlinNoise(Time.time, offset.y);
        //float sampleZ = Mathf.PerlinNoise(Time.time, offset.z);

        //Vector2 position = new Vector2 ((sampleX * (value * 2)) + 130, (sampleY * (value)) + 40);

        //size.transform.localScale = new Vector3(Mathf.Clamp(Mathf.Sin(Time.time * .5f) + .6f, .6f, 1.2f), 1, 1);

        //Invoke("Effect", 2);

        

        //title.GetComponent<RectTransform>().sizeDelta = Vector2.MoveTowards(transform.position, position, value);

    }

    void Effect()
    {
        //title.GetComponent<Text>().fontSize = 39 + Random.Range(0, 1);
    }
}
