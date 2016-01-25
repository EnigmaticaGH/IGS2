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
        HOVER,
        LANDING,
        DISABLED
    }
    private delegate void StateFunction();
    private StateFunction[] SetState;
    private JumpControl jumpControl;
    private HoverControl hoverControl;
    private companionScript portal;
    private MovementState state;

    void MapStateFunctions()
    {
        SetState = new StateFunction[] {
            Ground,
            Air,
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
        hoverControl = GetComponent<HoverControl>();
        portal = GameObject.Find("Companion").GetComponent<companionScript>();
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
        jumpControl.MovementState(state.ToString());
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

    public void Disable(float time)
    {
        StartCoroutine(DisableMovement(time));
    }

    public void SendGroundSensorReading(bool status)
    {
        isGrounded = status;
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