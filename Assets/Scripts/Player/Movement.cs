﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    #region Variables

    private ControllerNumber controller;
    public enum MovementState
    {
        GROUND,
        AIR,
        DISABLED
    }
    private MovementState state;

    void MapStateFunctions()
    {
        SetState = new StateFunction[] {
            Ground,
            Air,
            Disabled
        };
    }
    private delegate void StateFunction();
    private StateFunction[] SetState;
    public delegate void MovementStateChange(MovementState state, string sender);
    public static event MovementStateChange MovementStateEvent;

    public float moveForce;
    public float maxSpeed;
    private Rigidbody player;
    private bool isGrounded;
    private bool useForce;
    private float forceTime;

    PlayerSounds Sounds;

    #endregion

    void Start()
    {
        player = GetComponent<Rigidbody>();
        controller = GetComponent<ControllerNumber>();
        Sounds = GetComponent<PlayerSounds>();
        MapStateFunctions();
        ChangeState(MovementState.AIR);  
        useForce = false;
        forceTime = 0;
    }

    void FixedUpdate()
    {
        SetState[(int)state]();

        if (state != MovementState.DISABLED)
            UpdateMovement();
    }

    void ChangeState(MovementState newState)
    {
        state = newState;
        MovementStateEvent(state, name);
    }

    #region General Movement

    void UpdateMovement()
    {
        float lateralVelocity = Input.GetAxis("L_XAxis_" + controller.controllerNumber) * maxSpeed;
        if (Mathf.Abs(lateralVelocity) / maxSpeed < 0.3f) lateralVelocity = 0;
        Vector3 lateralForce = Vector3.right * Input.GetAxisRaw("L_XAxis_" + controller.controllerNumber) * moveForce;
        if (Input.GetKey(KeyCode.A))
        {
            lateralVelocity = -maxSpeed;
            lateralForce = Vector3.right * -moveForce;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lateralVelocity = maxSpeed;
            lateralForce = Vector3.right * moveForce;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            lateralVelocity = 0;
            lateralForce = Vector3.zero;
        }



        if (!useForce)
            player.velocity = new Vector3(lateralVelocity, player.velocity.y, player.velocity.z);
        else if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);
    }

    void IncreaseGravity()
    {
        if (player.velocity.y < 0 && (player.velocity.y < maxSpeed))
            player.AddForce(Physics.gravity / 2);
    }

    #endregion

    #region State Machine

    void Ground()
    {
        //Debug.Log("Grounded");
        if (!isGrounded)
        {
            ChangeState(MovementState.AIR);
            //Debug.Log("In Air");
        }
    }

    void Air()
    {
        if (isGrounded)
        {
            ChangeState(MovementState.GROUND);
        }

        IncreaseGravity();
    }

    void Disabled()
    {
        //The player is unable to move
    }

    #endregion

    IEnumerator DisableMovement(float disableTime, bool isStun)
    {
        MovementState oldState = state;
        ChangeState(MovementState.DISABLED);
        if (isStun)
            GetComponent<StunParticles>().Stun(disableTime);
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

    public void Disable(float time, bool isStun)
    {
        StartCoroutine(DisableMovement(time, isStun));
    }

    public void UseForceInstead(float time)
    {
        if (!useForce)
        {
            player.velocity = Vector3.zero;
            forceTime = time;
            StartCoroutine(ChangeToForce());
        }
        else
            forceTime += time;
    }

    public void SendGroundSensorReading(bool status)
    {
        bool oldStatus = isGrounded;
        isGrounded = status;
        if (oldStatus != status && status && player.velocity.y < 0)
        {
            GetComponent<LandingParticles>().StartLandingAnimation();
        }
    }

    public MovementState State
    {
        get { return state; }
    }
}