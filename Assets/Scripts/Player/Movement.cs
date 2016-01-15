﻿using UnityEngine;
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

    void UpdateMovementGround()
    {
        float lateralVelocity = Input.GetAxis("kb_Horizontal") * maxSpeed;
        player.velocity = new Vector3(lateralVelocity, player.velocity.y, player.velocity.z);
    }

    void UpdateMovementAir()
    {
        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("kb_Horizontal") * moveForce;
        if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);
        if (Mathf.Approximately(Input.GetAxis("kb_Horizontal"), 0))
        {
            StartCoroutine(DisableMovement(AIR_STOP_TIME));
        }
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