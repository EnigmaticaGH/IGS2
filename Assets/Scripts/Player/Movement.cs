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
        LANDING,
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
            Landing,
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
        HoverControl.OnHoverStartOrResume += OnHoverStartOrResume;
        HoverControl.OnHoverPause += OnHoverPause;
        HoverControl.OnHoverDone += OnHoverDone;
    }

    void OnDestroy()
    {
        GroundSensor.SensorReading -= ReadGroundSensor;
        JumpControl.OnJump -= OnJump;
        HoverControl.OnHoverStartOrResume += OnHoverStartOrResume;
        HoverControl.OnHoverPause -= OnHoverPause;
        HoverControl.OnHoverDone -= OnHoverDone;
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

    void Jump() //The player initiated a jump
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
    }

    void Hover() //The player is currently hovering
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
    }

    void Landing() //The player is landing from a hover
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovement();
    }

    void Disabled()
    {
        //The player is unable to move
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

    void OnHoverStartOrResume()
    {
        ChangeState(MovementState.HOVER);
    }

    void OnHoverPause()
    {
        ChangeState(MovementState.AIR);
    }

    void OnHoverDone()
    {
        ChangeState(MovementState.LANDING);
    }
}