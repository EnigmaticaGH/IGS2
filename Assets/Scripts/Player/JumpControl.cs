using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private int controllerNumber;
    private ControllerNumber controller;
    private bool canNormalJump;
    private bool jumpButtonPressed;
    public float jumpStrength;
    public float maxJumpTime;
    private Rigidbody player;
    private bool jumpStarted;

    public float wallJumpStrength;
    private bool canWallJump;
    private int direction;
    private float wallJumpForce;
    private bool jumpKeyUp;

    private Movement movement;

    private string jumpButton;
    private bool canDoubleJump;
    
    void Start()
    {
        controller = GetComponent<ControllerNumber>();
        controllerNumber = controller.controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        jumpButton = "A";
        jumpStarted = false;
        jumpKeyUp = true;
        canDoubleJump = false;
    }

    void Update()
    {
        jumpButtonPressed = Input.GetButton(jumpButton + "_" + controllerNumber);
        if (canWallJump && jumpButtonPressed && jumpKeyUp && !canNormalJump)
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
        if (canDoubleJump && jumpButtonPressed && jumpKeyUp && !canNormalJump && !jumpStarted)
        {
            StartCoroutine(JumpTimer());
            canDoubleJump = false;
        }
    }

    public void MovementState(string state)
    {
        canNormalJump = state == "GROUND";
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed && canNormalJump && !jumpStarted)
        {
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        jumpStarted = true;
        jumpKeyUp = false;
        canDoubleJump = true;
        float time = maxJumpTime;
        movement.OnJump();
        while (time > 0 && Input.GetButton(jumpButton + "_" + controllerNumber))
        {
            time -= Time.deltaTime;
            //player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            player.velocity = Vector3.up * jumpStrength;
            yield return null;
        }
        jumpStarted = false;
        while(Input.GetButton(jumpButton + "_" + controllerNumber))
        {
            yield return null;
        }
        jumpKeyUp = true;
    }

    public void SendWallSensorReading(char status)
    {
        direction = status == 'L' ? 1 : -1;
        //canWallJump = status != ' ';
        canWallJump = false;
        wallJumpForce = canWallJump ? direction * wallJumpStrength : 0;
    }
}
