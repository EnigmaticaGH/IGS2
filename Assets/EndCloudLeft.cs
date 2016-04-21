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
        if (col.name.Contains("LEFT"))
        {
            Cloud.CloudEndLeft(col.gameObject.name);
        }
    }
}
