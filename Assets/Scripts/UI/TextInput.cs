using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextInput : MonoBehaviour {

    public GameObject[] player;
    public Light Sun;

    InputField input;
    InputField.SubmitEvent se;

    int GodMode = 99999;

	// Use this for initialization
	void Start () {

        input = gameObject.GetComponent<InputField>();
        se = new InputField.SubmitEvent();
        se.AddListener(SumbitInput);
        input.onEndEdit = se;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SumbitInput(string arg0)
    {
        Debug.Log(arg0);
        if (arg0.ToString().ToLower().Contains("god mode"))
        {
            Debug.Log(arg0);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<DeathControl>().GodMode(GodMode);
            }
        }

        if (arg0.ToString().ToLower().Contains("night"))
        {
            Sun.color = Color.black;
        }

        if (arg0.ToString().ToLower().Contains("day"))
        {
            Sun.color = Color.white;
        }

    }
}
