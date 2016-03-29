using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class StartMovie : MonoBehaviour {

    public MovieTexture movie;
    private AudioSource audio;

	// Use this for initialization
	void Start () {

        GetComponent<RawImage>().texture = movie as MovieTexture;
        movie.Play();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
