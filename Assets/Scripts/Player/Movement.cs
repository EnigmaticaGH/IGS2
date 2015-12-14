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
    private Rigidbody player;
    private bool isGrounded;

    public delegate void JumpDelegate(bool canJump);
    public static event JumpDelegate AbleToJump;

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
        MapStateFunctions();
        AbleToJump(true);
        ChangeState(MovementState.GROUND);
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
    }

    void Ground()
    {
        if (!isGrounded)
        {
            AbleToJump(false);
            ChangeState(MovementState.AIR);
        }

        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("Horizontal") * moveForce;
        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);

        if (player.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0
         || player.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            player.velocity = new Vector3(0, player.velocity.y, player.velocity.z);
        }
        
    }

    void Air()
    {
        if (isGrounded)
        {
            AbleToJump(true);
            ChangeState(MovementState.GROUND);
        }

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