using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public int controllerNumber;
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
    private companionScript portal;
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
    private char wallSensorStatus;

    public delegate void PlayerPosition(float position, string sender);
    public static event PlayerPosition PositionUpdateEvent;

    void Awake()
    {
        HoverControl.OnHoverStartOrResume += OnHoverStartOrResume;
        HoverControl.OnHoverPause += OnHoverPause;
        HoverControl.OnHoverDone += OnHoverDone;

         //portal = portal.GetComponent<companionScript>();
    }

    void OnDestroy()
    {
        HoverControl.OnHoverStartOrResume += OnHoverStartOrResume;
        HoverControl.OnHoverPause -= OnHoverPause;
        HoverControl.OnHoverDone -= OnHoverDone;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
        jumpControl = GetComponent<JumpControl>();
        //hoverControl = GetComponent<HoverControl>();
        dynamicCollider = GetComponent<DynamicCollider>();
        portal = GameObject.Find("Companion").GetComponent<companionScript>();
        MapStateFunctions();
        ChangeState(MovementState.GROUND);
        startedJumpCoroutine = false;
    }

    void FixedUpdate()
    {
        SetState[(int)state]();
        PositionUpdateEvent(transform.position.x, name);
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
        jumpControl.MovementState(state.ToString());
        dynamicCollider.MovementStateChange(state.ToString());
        /*if (hoverControl != null)
            hoverControl.MovementState(state.ToString());*/
        portal.MovementStateChange(state.ToString());
    }

    void UpdateMovementGround()
    {
        float lateralVelocity = Input.GetAxis("L_XAxis_" + controllerNumber) * maxSpeed;
        int a = 0, d = 0;
        
        if(Input.GetKey(KeyCode.D))
        {
            d = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            a = 1;
        }

        if (useKeyboard)
            lateralVelocity = (d - a) * maxSpeed;

        if (lateralVelocity != 0)
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