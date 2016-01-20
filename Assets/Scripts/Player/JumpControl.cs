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
    public delegate void JumpState();
    public static event JumpState OnJump;

    public float wallJumpStrength;
    private bool canWallJump;
    private bool jumpButtonUp;
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
        controllerNumber = GetComponent<Movement>().controllerNumber;
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        jumpButtonUp = false;
    }

    void OnDestroy()
    {
        Movement.StateChangeEvent -= MovementState;
        WallSensor.SensorReading -= ReadWallSensor;
    }

    void Update()
    {
        if (canNormalJump && Input.GetButton("A_" + controllerNumber) && !jumpButtonPressed)
        {
            StartCoroutine(JumpKeyDown());
        }
        if(canWallJump && Input.GetButton("A_" + controllerNumber) && jumpButtonUp && !canNormalJump)
        {
            player.velocity = new Vector3(wallJumpForce, Mathf.Abs(wallJumpForce), player.velocity.z);
            canWallJump = false;
            movement.Disable(0.5f);
        }
    }

    void MovementState(string state)
    {
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
        if (OnJump != null)
            OnJump();
        while (time > 0 && Input.GetButton("A_" + controllerNumber))
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
        while (canNormalJump && Input.GetButton("A_" + controllerNumber))
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
