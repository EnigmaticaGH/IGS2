using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioScript : MonoBehaviour {

    public GameObject Audio;

    public AudioSource Source;

    bool stop = false;

    void OnLevelWasLoaded(int level)
    {
        if (level >= 3)
        {
            stop = true;
            Debug.Log(level);
        }


        if (level == 0)
        {
            if (Source.isPlaying)
                Source.Stop();
        }
        

    }

	// Use this for initialization
	void Start () {
        if (!Source.isPlaying)
            Source.Play();


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
