using UnityEngine;
using System.Collections;

public class PlayWalk : MonoBehaviour {

    public AudioSource Source;

    public AudioClip Walk;

	// Use this for initialization
	void Start () {

        Source = GetComponent<AudioSource>();
	
	}

    public void Play()
    {
        Source.clip = Walk;
        
        Source.Play();
    }
}
