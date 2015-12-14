using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    enum MovementState
    {
        GROUND,
        AIR,
        JUMP,
        HOVER,
        DISABLED
    }
    private delegate void StateFunction();
    private StateFunction[] SetState;
    public delegate void StateChange(string state);
    public static event StateChange StateChangeEvent;
    private MovementState state;

    void MapStateFunctions()
    {
        SetState = new StateFunction[] {
            Ground,
            Air,
            Jump,
            Hover,
            Disabled
        };
    }

    private const float AIR_STOP_TIME = 0.06f;

    public float moveForce;
    public float maxSpeed;
    private Rigidbody player;
    private bool isGrounded;

    void Awake()
    {
        GroundSensor.SensorReading += ReadGroundSensor;
        JumpControl.OnJump += OnJump;
        HoverControl.OnHoverStart += OnHoverStart;
        HoverControl.OnHoverDone += OnHoverDone;
        DeathControl.OnDeath += OnDeath;
    }

    void OnDestroy()
    {
        GroundSensor.SensorReading -= ReadGroundSensor;
        JumpControl.OnJump -= OnJump;
        HoverControl.OnHoverStart += OnHoverStart;
        HoverControl.OnHoverDone -= OnHoverDone;
        DeathControl.OnDeath -= OnDeath;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
        MapStateFunctions();
        ChangeState(MovementState.GROUND);
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
        if(StateChangeEvent != null)
            StateChangeEvent(state.ToString());
    }

    void UpdateMovement()
    {
        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("Horizontal") * moveForce;
        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);

        if (player.velocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0
         || player.velocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            player.velocity = new Vector3(0, player.velocity.y, player.velocity.z);
            if (!isGrounded)
                StartCoroutine(DisableMovement(AIR_STOP_TIME));
        }
    }

    void Ground()
    {
        if (!isGrounded)
        {
            ChangeState(MovementState.AIR);
        }

        UpdateMovement();
    }

    void Air()
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
    }

    void Jump()
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
    }

    void Hover()
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
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

    void OnJump()
    {
        ChangeState(MovementState.JUMP);
    }

    void OnHoverStart()
    {
        ChangeState(MovementState.HOVER);
    }

    void OnHoverDone()
    {
        ChangeState(MovementState.AIR);
    }

    void OnDeath(float respawnTime)
    {
        StartCoroutine(DisableMovement(respawnTime));
    }
}