using UnityEngine;
using System.Collections;

public class WallColorFader : MonoBehaviour {

    Renderer rend;

    IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return StartCoroutine("FadeBack");
        }
      
    }
    IEnumerator FadeBack()
    {
        yield return new WaitForSeconds(4);
        for (float g = 0f; g <= 1; g += 0.1f)
        {
            Color c = rend.material.color;
            c.a = g;
            rend.material.color = c;
            
        }
    }

	// Use this for initialization
	void Start () {

        rend = GetComponent<Renderer>();
        StartCoroutine("Fade");
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FadeBacker()
    {
        StartCoroutine("FadeBack");
    }

    void Fader()
    {
        StartCoroutine("Fade");
    }
}
