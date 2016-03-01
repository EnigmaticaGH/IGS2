using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody body;
    private Movement move;
    private JumpControl jump;
    private Movement.MovementState state;

    void Awake()
    {
        Movement.MovementStateEvent += MovementStateChange;
    }

    void OnDestroy()
    {
        Movement.MovementStateEvent -= MovementStateChange;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponentInParent<Rigidbody>();
        move = GetComponentInParent<Movement>();
        jump = GetComponentInParent<JumpControl>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("VerticalSpeed", body.velocity.y);
        anim.SetBool("Moving", Mathf.Abs(body.velocity.x) > 0.05f);
        anim.SetBool("Jumping", jump.isJumping());
        anim.SetBool("Grounded", state == Movement.MovementState.GROUND);
        if (anim.GetBool("Moving") && anim.GetBool("Grounded"))
            anim.speed = Mathf.Abs(body.velocity.x / move.maxSpeed);
        else
            anim.speed = 1;
    }

    public void MovementStateChange(Movement.MovementState moveState, string n)
    {
        if (transform.parent.name != n) return;
        state = moveState;
    }
}
