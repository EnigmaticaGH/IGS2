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

    private bool spacePressed;
    private string jumpButton;
    private bool canDoubleJump;
    
    void Start()
    {
        controller = GetComponent<ControllerNumber>();
        controllerNumber = controller.controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        jumpButton = "A";
        spacePressed = false;
        jumpStarted = false;
        jumpKeyUp = true;
        canDoubleJump = false;
    }

    void Update()
    {
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        jumpButtonPressed = (Input.GetButton(jumpButton + "_" + controllerNumber) || spacePressed);
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
        controllerNumber = controller.controllerNumber;
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
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        while (time > 0 && (Input.GetButton(jumpButton + "_" + controllerNumber) || spacePressed))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
        jumpStarted = false;
        while((Input.GetButton(jumpButton + "_" + controllerNumber) || spacePressed))
        {
            yield return null;
        }
        jumpKeyUp = true;
    }

    public void SendWallSensorReading(char status)
    {
        direction = status == 'L' ? 1 : -1;
        canWallJump = status != ' ';
        wallJumpForce = canWallJump ? direction * wallJumpStrength : 0;
    }
}
