using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextInput : MonoBehaviour {

    public GameObject player;

    InputField input;
    InputField.SubmitEvent se;
    DeathControl Lives;
    int godMode = 0;

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
        if (arg0.ToString().Contains("God mode"))
        {
            Debug.Log(arg0);
            player.GetComponent<DeathControl>().lives = 9999999;
            //Lives.lives = 99999999;
            /*
            for (int i = 0; i < arg0.Length; i++)
            {
                if (arg0[i].ToString() == "1")
                {
                    Debug.Log("1 found");
                }
            }*/
        }
    }
}
