using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private int controllerNumber;
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
    private string playerstate;
    
    void Start()
    {
        controllerNumber = GetComponent<Movement>().controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        jumpButton = "A_" + controllerNumber;
        spacePressed = false;
        jumpStarted = false;
        jumpKeyUp = true;
    }

    void Update()
    {
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        jumpButtonPressed = (Input.GetButton(jumpButton) || spacePressed);
        if (canWallJump && jumpButtonPressed && jumpKeyUp && !canNormalJump)
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
    }

    public void MovementState(string state)
    {
        canNormalJump = state == "GROUND";
        playerstate = state;
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
        float time = maxJumpTime;
        movement.OnJump();
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        while (time > 0 && (Input.GetButton(jumpButton) || spacePressed))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
        jumpStarted = false;
        while((Input.GetButton(jumpButton) || spacePressed))
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
