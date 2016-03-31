using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioScript : MonoBehaviour {

    public GameObject Audio;

    bool stop = false;

    void OnLevelWasLoaded(int level)
    {
        if (level >= 3)
        {
            stop = true;
            Debug.Log(level);
        }

    }

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {

        if (stop == false)
            DontDestroyOnLoad(Audio);
        else if (stop)
        {
            Destroy(Audio);
        }
	
	}
}
