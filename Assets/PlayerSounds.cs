using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour 
{

    public AudioSource Source;

    public AudioClip Death;

    public AudioClip CloudPop;

    public AudioClip Punch;

    public AudioClip Jump;

    public void Sound(string Action)
    {
        if (Action == "Jump")
        {
            Source.clip = Jump;
            Source.loop = false;
            Source.Play();
        }


        if (Action == "Death")
        {
            Source.clip = Death;
            Source.loop = false;
            Source.Play();
        }

        if (Action == "Pop")
        {
            Source.clip = CloudPop;
            Source.loop = false;
            Source.Play();
        }
    }
}
