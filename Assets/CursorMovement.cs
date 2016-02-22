﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CursorMovement : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction OnClickedLeft;
    public static event ClickAction OnClickedRight;

    public Rigidbody rb;
    private float CursorSpeed = 5;
    public int ControllerNumber;
    public Button btn;
    public bool LeftButton = false;
    public bool RightButton = false;
    int i = 0;

	// Use this for initialization
	void Start () 
    {

        rb = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float horizontal = Input.GetAxis("L_XAxis_" + ControllerNumber);
        float vertical = Input.GetAxis("L_YAxis_" + ControllerNumber);

        rb.velocity = new Vector3(horizontal * CursorSpeed, -vertical * CursorSpeed, 0);


    // ...

    // 6 - Make sure we are not outside the camera bounds
    var dist = (transform.position - Camera.main.transform.position).z;

    var leftBorder = Camera.main.ViewportToWorldPoint(
      new Vector3(0, 0, dist)
    ).x;

    var rightBorder = Camera.main.ViewportToWorldPoint(
      new Vector3(1, 0, dist)
    ).x;

    var topBorder = Camera.main.ViewportToWorldPoint(
      new Vector3(0, 0, dist)
    ).y;

    var bottomBorder = Camera.main.ViewportToWorldPoint(
      new Vector3(0, 1, dist)
    ).y;

    transform.position = new Vector3(
      Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
      Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
      transform.position.z
    );
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Button")
        {
            Debug.LogError("Button Collision Detected");
            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Button")
        {
            Debug.LogError("Wow collision detected");
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0)) 
            {
                i++; //Make i one so you can't constantly press A 
                Debug.Log("Player clicked A");
                Invoke("CooldownA", .5f);
            }
        }

        if (col.tag == "Start")
        {
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                i++; //Make i one so you can't constantly press A 
                Debug.Log("Player clicked A " + col.tag);

                SceneManager.LoadScene(2);

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Left Button")
        {
            //Debug.Log("Left Button Detected");
            LeftButton = true;
            
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                i++; //Make i one so you can't constantly press A 
                Debug.Log("Player clicked A ");

                OnClickedLeft();

                Invoke("CooldownA", .5f);
            }
        }
        else
            LeftButton = false;

        if (col.name == "Right Button")
        {
            //Debug.Log("Right Button Detected");
            RightButton = true;
           
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                i++; //Make i one so you can't constantly press A 
                Debug.Log("Player clicked A ");

                OnClickedRight();

                Invoke("CooldownA", .5f);
            }
        }
        else
            RightButton = false;
    }

    void CooldownA()
    {
        i = 0;
    }
}
