using UnityEngine;
using System.Collections;

public class EndCloud : MonoBehaviour {

    CloudScript Cloud;

    void Start()
    {
        Cloud = GameObject.Find("CloudController").GetComponent<CloudScript>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Cloud")
        {
            Cloud.CloudEnd(col.gameObject.name);
        }
    }
}
