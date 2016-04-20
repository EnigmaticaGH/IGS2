using UnityEngine;
using System.Collections;

public class EndCloudLeft : MonoBehaviour {

    CloudScript Cloud;

    void Start()
    {
        Cloud = GameObject.Find("CloudController").GetComponent<CloudScript>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Cloud")
        {
            Cloud.CloudEndLeft(col.gameObject.name);
        }
    }
}
