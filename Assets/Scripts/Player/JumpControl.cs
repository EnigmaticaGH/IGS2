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

    public float wallJumpStrength;
    private bool canWallJump;
    private int direction;
    private float wallJumpForce;

    private Movement movement;

    private bool spacePressed;
    private bool jumpStarted;

    void Start()
    {
        controllerNumber = GetComponent<Movement>().controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        spacePressed = false;
        jumpStarted = false;
    }

    void Update()
    {
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        jumpButtonPressed = canNormalJump && (Input.GetButton("A_" + controllerNumber) || spacePressed);
        if(canWallJump && (Input.GetButton("A_" + controllerNumber) || spacePressed) && !canNormalJump)
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
    }

    public void MovementState(string state)
    {
        //Debug.Log(state);
        canNormalJump = state == "GROUND";
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed && !jumpStarted)
        {
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        jumpStarted = true;
        float time = maxJumpTime;
        movement.OnJump();
        spacePressed = movement.useKeyboard && Input.GetKey(KeyCode.Space);
        while (time > 0 && (Input.GetButton("A_" + controllerNumber) || spacePressed))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
        jumpStarted = false;
    }

    public void SendWallSensorReading(char status)
    {
        direction = status == 'L' ? 1 : -1;
        canWallJump = status != ' ';
        wallJumpForce = canWallJump ? direction * wallJumpStrength : 0;
    }
}
