using UnityEngine;
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

    public float velocityMin;
    public float moveForce;
    public float maxSpeed;
    private Rigidbody player;
    private bool isGrounded;
    private bool useForce;
    private float forceTime;

    #endregion

    void Start()
    {
        player = GetComponent<Rigidbody>();
        controller = GetComponent<ControllerNumber>();
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

        if (!useForce)
            player.velocity = new Vector3(lateralVelocity, player.velocity.y, player.velocity.z);
        else if (Mathf.Abs(player.velocity.x) < maxSpeed)
            player.AddForce(lateralForce);

        //player.velocity = new Vector3(player.velocity.x, Mathf.Clamp(-15, Mathf.infinity, player.velocity.z));
    }

    void IncreaseGravity()
    {
        if (player.velocity.y < 0 && player.velocity.y > velocityMin)
            player.AddForce(Physics.gravity);
    }

    #endregion

    #region State Machine

    void Ground()
    {
        if (!isGrounded)
        {
            ChangeState(MovementState.AIR);
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

    public void Disable(float time)
    {
        StartCoroutine(DisableMovement(time));
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
        isGrounded = status;
    }

    public MovementState State
    {
        get { return state; }
    }
}