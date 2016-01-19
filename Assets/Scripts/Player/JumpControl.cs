using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private bool canJump;
    private bool jumpButtonPressed;
    public float jumpStrength;
    public float maxJumpTime;
    private Rigidbody player;
    public delegate void JumpState();
    public static event JumpState OnJump;

    public float wallJumpStrength;
    private bool canWallJump;
    private int direction;
    private float wallJumpForce;

    private Movement movement;

    void Awake()
    {
        Movement.StateChangeEvent += MovementState;
        WallSensor.SensorReading += ReadWallSensor;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
    }

    void OnDestroy()
    {
        Movement.StateChangeEvent -= MovementState;
        WallSensor.SensorReading -= ReadWallSensor;
    }

    void Update()
    {
        if (canJump && Input.GetButton("A_1") && !jumpButtonPressed)
        {
            StartCoroutine(JumpKeyDown());
        }
        if(canWallJump && Input.GetButton("A_1"))
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
    }

    void MovementState(string state)
    {
        canJump = state == "GROUND";
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
        float time = maxJumpTime;
        if (OnJump != null)
            OnJump();
        while (time > 0 && Input.GetButton("A_1"))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
    }

    IEnumerator JumpKeyDown()
    {
        jumpButtonPressed = true;
        while (canJump && Input.GetButton("A_1"))
        {
            yield return null;
        }
        jumpButtonPressed = false;
    }

    void ReadWallSensor(char status)
    {
        direction = status == 'L' ? 1 : -1;
        canWallJump = status != ' ';
        wallJumpForce = canWallJump ? direction * wallJumpStrength : 0;
    }
}
