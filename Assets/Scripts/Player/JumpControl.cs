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

    PlayerSounds sounds;

    void Awake()
    {
        sounds = GetComponent<PlayerSounds>();
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
            Input.GetButtonDown(Controls.JumpControls[0] + controller.controllerNumber) ||
            Input.GetButtonDown(Controls.JumpControls[1] + controller.controllerNumber) ||
            Input.GetKey(KeyCode.Space);
       
        if (jumpButtonPressed)
        {
            //sounds.Sound("Jump");

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
            sounds.Sound("Jump");
            player.velocity = Vector3.up * jumpStrength;
            jump = true;
        }
        else if (!doubleJump)
        {
            sounds.Sound("Jump");
            player.velocity = Vector3.up * jumpStrength;
            doubleJump = true;
        }
    }

    public bool isJumping()
    {
        return jump || doubleJump;
    }
}
