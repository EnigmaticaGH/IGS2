﻿using UnityEngine;
using System.Collections;

public class lightFader : MonoBehaviour {

    public float duration = 1;
    public Light light1;

	// Use this for initialization
	void Start () {

        light1 = GetComponent<Light>();
	
	}
	
	// Update is called once per frame
	void Update () {

        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 2F + 0.5F; //Was orginially Mathf.Cos(phi) * .5f Switched to * 2 to increase max and min value range
        light1.intensity = amplitude + 3;

	
	}
}
