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
    private bool jumpButtonUp;
    private int direction;
    private float wallJumpForce;

    private Movement movement;

    void Start()
    {
        controllerNumber = GetComponent<Movement>().controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        jumpButtonUp = false;
    }

    void Update()
    {
        if (canNormalJump && (Input.GetButton("A_" + controllerNumber) || Input.GetKey(KeyCode.Space)) && !jumpButtonPressed)
        {
            StartCoroutine(JumpKeyDown());
        }
        if(canWallJump && (Input.GetButton("A_" + controllerNumber) || Input.GetKey(KeyCode.Space)) && jumpButtonUp && !canNormalJump)
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
    }

    public void MovementState(string state)
    {
        Debug.Log(state);
        canNormalJump = state == "GROUND";
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed)
        {
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        jumpButtonUp = false;
        float time = maxJumpTime;
        while (time > 0 && (Input.GetButton("A_" + controllerNumber) || Input.GetKey(KeyCode.Space)))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
        jumpButtonUp = true;
    }

    IEnumerator JumpKeyDown()
    {
        jumpButtonPressed = true;
        while (canNormalJump && (Input.GetButton("A_" + controllerNumber) || Input.GetKey(KeyCode.Space)))
        {
            yield return null;
        }
        jumpButtonPressed = false;
    }

    public void SendWallSensorReading(char status)
    {
        direction = status == 'L' ? 1 : -1;
        canWallJump = status != ' ';
        wallJumpForce = canWallJump ? direction * wallJumpStrength : 0;
    }
}
