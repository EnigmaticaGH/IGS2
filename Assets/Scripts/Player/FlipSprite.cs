﻿using UnityEngine;
using System.Collections;

public class FlipSprite : MonoBehaviour
{
    private Rigidbody player;
    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.velocity.x) > 0.05f)
            transform.localScale = new Vector3(1 * Mathf.Sign(player.velocity.x), 1, 1);
    }
}
