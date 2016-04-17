using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private ControllerNumber controller;
    private Rigidbody player;
    public float jumpStrength;
    private bool jumpButtonPressed;
    private bool jumpAxisInUse;
    private bool canJump;
    private bool jump;
    private bool doubleJump;
    private string[] jumpAxis;
    Controls Controls;

    void Awake()
    {
        Controls = GetComponent<Controls>();
        DeathControl.OnRespawn += Reset;
        Movement.MovementStateEvent += MovementStateChange;
    }

    void OnDestroy()
    {
        DeathControl.OnRespawn -= Reset;
        Movement.MovementStateEvent -= MovementStateChange;
    }

    void Start()
    {
        controller = GetComponent<ControllerNumber>();
        player = GetComponent<Rigidbody>();
        jumpAxis = new string[]
            {
                "TriggersL",
                "L_YAxis",
                "LB",
                "Y"
            };
        jumpAxisInUse = false;
        doubleJump = false;
        jump = false;
    }

    void Reset(string sender)
    {
        if (sender == name)
        {
            jump = false;
            doubleJump = false;
        }
    }

    void Update()
    {
        //Debug.Log(name + ": " + controller.controllerNumber);
        jumpButtonPressed = 
            Input.GetAxisRaw(jumpAxis[2] + "_" + controller.controllerNumber) == 1 || 
            Input.GetAxisRaw(jumpAxis[3] + "_" + controller.controllerNumber) == 1 || 
            Input.GetButtonDown(Controls.JumpControls[0] + controller.controllerNumber) ||
            Input.GetKey(KeyCode.Space);
       
        if (jumpButtonPressed)
        {
            if (!jumpAxisInUse)
            {
                Jump();
                jumpAxisInUse = true;
            }
        }
        if (!jumpButtonPressed)
        {
            jumpAxisInUse = false;
        }
    }

    public void MovementStateChange(Movement.MovementState state, string n)
    {
        if (name != n) return;
        canJump = state == Movement.MovementState.GROUND;
        if (canJump)
        {
            jump = false;
            doubleJump = false;
        }
    }

    void Jump()
    {
        if (canJump && !jump)
        {
            player.velocity = Vector3.up * jumpStrength;
            jump = true;
        }
        else if (!doubleJump)
        {
            player.velocity = Vector3.up * jumpStrength;
            doubleJump = true;
        }
    }

    public bool isJumping()
    {
        return jump || doubleJump;
    }
}
