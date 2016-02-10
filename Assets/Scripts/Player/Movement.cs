using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    #region Variables

    private ControllerNumber controller;
    private int controllerNumber;
    public bool useKeyboard;
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
    private JumpControl jumpControl;
    //private HoverControl hoverControl;
    private DynamicCollider dynamicCollider;
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
    private bool startedJumpCoroutine;

    public PlayerAnim playerAnimator;

    #endregion

    void Start()
    {
        player = GetComponent<Rigidbody>();
        jumpControl = GetComponent<JumpControl>();
        //hoverControl = GetComponent<HoverControl>();
        dynamicCollider = GetComponent<DynamicCollider>();
        MapStateFunctions();
        ChangeState(MovementState.GROUND);
        startedJumpCoroutine = false;
        controller = GetComponent<ControllerNumber>();
        controllerNumber = controller.controllerNumber;
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
        controllerNumber = controller.controllerNumber;
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
        jumpControl.MovementState(state.ToString());
        dynamicCollider.MovementStateChange(state.ToString());
        if (playerAnimator != null)
            playerAnimator.MovementStateChange(state.ToString());
        
    }

    #region General Movement

    void UpdateMovementGround()
    {
        float lateralVelocity = Input.GetAxis("L_XAxis_" + controllerNumber) * maxSpeed;
        int a = 0, d = 0;
        if (Input.GetKey(KeyCode.D))
        {
            d = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            a = 1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            a = 0;
            d = 0;
        }

        if (useKeyboard)
            lateralVelocity = (d - a) * maxSpeed;
        player.velocity = new Vector3(lateralVelocity, player.velocity.y, player.velocity.z);
    }

    void UpdateMovementAir()
    {
        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("L_XAxis_" + controllerNumber) * moveForce;

        int a = 0, d = 0;
        if (Input.GetKey(KeyCode.D))
        {
            d = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            a = 1;
        }

        if (useKeyboard)
            lateralForce = Vector3.right * (d - a) * moveForce;

        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);
    }

    #endregion

    #region State Machine

    void Ground()
    {
        if (!isGrounded)
        {
            ChangeState(MovementState.AIR);
        }

        UpdateMovementGround();
    }

    void Air()
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovementAir();
    }

    void Jump() //The player initiated a jump
    {
        //Delay ground checking until the player is off the ground initially from the jump
        if(!startedJumpCoroutine)
            StartCoroutine(JumpToGroundState());

        UpdateMovementAir();
    }

    void Hover() //The player is currently hovering
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovementAir();
    }

    void Landing() //The player is landing from a hover (but still in the air)
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        UpdateMovementAir();
    }

    void Disabled()
    {
        //The player is unable to move
    }

    #endregion

    IEnumerator DisableMovement(float disableTime)
    {
        MovementState oldState = state;
        ChangeState(MovementState.DISABLED);
        yield return new WaitForSeconds(disableTime);
        oldState = oldState.ToString() != "DISABLED" ? oldState : MovementState.AIR;
        ChangeState(oldState);
    }

    IEnumerator JumpToGroundState()
    {
        startedJumpCoroutine = true;
        yield return new WaitForSeconds(0.1f);
        while (!isGrounded)
        {
            yield return new WaitForFixedUpdate();            
        }
        startedJumpCoroutine = false;
        ChangeState(MovementState.GROUND);
    }

    public void Disable(float time)
    {
        StartCoroutine(DisableMovement(time));
    }

    public void SendGroundSensorReading(bool status)
    {
        isGrounded = status;
    }

    public void OnJump()
    {
        ChangeState(MovementState.JUMP);
    }
}