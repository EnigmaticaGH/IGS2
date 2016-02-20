using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterMenuButton : MonoBehaviour {

    Button Button1;

    void Start()
    {
        GetComponent<Button>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Cursor")
        {
            Debug.LogError("Button Collision Detected");
            
        }
    }
}
