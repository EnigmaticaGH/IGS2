using UnityEngine;
using System.Collections;

public class SkyboxParallax : MonoBehaviour
{
    public SmashCamera cam;
    private Rigidbody body;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.velocity = cam.Velocity / 4;
    }
}
