using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    #region Variables

    private ControllerNumber controller;
    private int controllerNumber;
    public bool useKeyboard;
    public enum MovementState
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
    private bool useForce;
    private float forceTime;
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
        useForce = false;
        forceTime = 0;
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
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

    void UpdateMovement()
    {
        float lateralVelocity = Input.GetAxis("L_XAxis_" + controllerNumber) * maxSpeed;
        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("L_XAxis_" + controllerNumber) * moveForce;

        if (!useForce)
            player.velocity = new Vector3(lateralVelocity, player.velocity.y, player.velocity.z);
        else if (Mathf.Abs(player.velocity.x) < maxSpeed)
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
        //Delay ground checking until the player is off the ground initially from the jump
        if(!startedJumpCoroutine)
            StartCoroutine(JumpToGroundState());
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

    void Landing() //The player is landing from a hover (but still in the air)
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

    #endregion

    IEnumerator DisableMovement(float disableTime)
    {
        MovementState oldState = state;
        ChangeState(MovementState.DISABLED);
        yield return new WaitForSeconds(disableTime);
        oldState = oldState.ToString() != "DISABLED" ? oldState : MovementState.AIR;
        ChangeState(oldState);
    }

    IEnumerator ChangeToForce()
    {
        useForce = true;
        while(forceTime >= 0)
        {
            forceTime -= Time.deltaTime;
            yield return null;
        }
        useForce = false;
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

    public void UseForceInstead(float time)
    {
        if (!useForce)
        {
            forceTime = time;
            StartCoroutine(ChangeToForce());
        }
        else
            forceTime += time;
    }

    public void SendGroundSensorReading(bool status)
    {
        isGrounded = status;
    }

    public void OnJump()
    {
        ChangeState(MovementState.JUMP);
    }

    public MovementState State
    {
        get { return state; }
    }
}