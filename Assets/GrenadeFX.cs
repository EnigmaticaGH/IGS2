using UnityEngine;
using System.Collections;

public class GrenadeFX : MonoBehaviour {

    public AudioSource GrenadeSource;

    public AudioClip Explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ExplosionFN()
    {
        GrenadeSource.clip = Explosion;
        GrenadeSource.Play();
    }
}
