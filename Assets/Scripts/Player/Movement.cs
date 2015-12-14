using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    enum MovementState
    {
        GROUND,
        AIR,
        DISABLED
    }
    private delegate void StateFunction();
    private StateFunction[] SetState;
    private MovementState state;

    void MapStateFunctions()
    {
        SetState = new StateFunction[] {
            Ground,
            Air,
            Disabled
        };
    }

    private const float AIR_STOP_TIME = 0.06f;

    public float moveForce;
    public float maxSpeed;
    public float jumpStrength;
    public float maxJumpTime;
    private Rigidbody player;
    private bool isGrounded;
    private bool canJump;
    private bool jumpButtonPressed;

    void Awake()
    {
        GroundSensor.SensorReading += ReadGroundSensor;
    }

    void OnDestroy()
    {
        GroundSensor.SensorReading -= ReadGroundSensor;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
        canJump = true;
        jumpButtonPressed = false;
        MapStateFunctions();
        ChangeState(MovementState.GROUND);
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space: " + Time.time);
        }
        if(!isGrounded)
        {
            Debug.Log("Air: " + Time.time);
        }
        jumpButtonPressed = isGrounded && Input.GetKeyDown(KeyCode.Space);
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
    }

    void Ground()
    {
        if (!isGrounded)
            ChangeState(MovementState.AIR);

        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("Horizontal") * moveForce;
        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);

        if (player.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0
         || player.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            player.velocity = new Vector3(0, player.velocity.y, player.velocity.z);
        }
        if (jumpButtonPressed && canJump)
        {
            StartCoroutine(JumpTimer());
        }
    }

    void Air()
    {
        if (isGrounded)
            ChangeState(MovementState.GROUND);

        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("Horizontal") * moveForce;
        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);

        if (player.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0
         || player.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            player.velocity = new Vector3(0, player.velocity.y, player.velocity.z);
            StartCoroutine(DisableMovement(AIR_STOP_TIME));
        }
    }

    void Disabled()
    {

    }

    IEnumerator JumpTimer()
    {
        float time = maxJumpTime;
        canJump = false;
        while(time > 0)
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            if (Input.GetKeyUp(KeyCode.Space))
            {
                time = 0;
                canJump = true;
            }
            yield return null;
        }
        canJump = true;
    }

    public IEnumerator DisableMovement(float disableTime)
    {
        MovementState oldState = state;
        ChangeState(MovementState.DISABLED);
        yield return new WaitForSeconds(disableTime);
        ChangeState(oldState);
    }

    void ReadGroundSensor(bool status)
    {
        isGrounded = status;
    }
}