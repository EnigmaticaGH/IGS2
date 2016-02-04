using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody body;
    private Movement move;
    private string state;

    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponentInParent<Rigidbody>();
        move = GetComponentInParent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("VerticalSpeed", body.velocity.y);
        anim.SetBool("Moving", Mathf.Abs(body.velocity.x) > 0.05f);
        anim.SetBool("Jumping", state == "JUMP");
        anim.SetBool("Falling", state == "AIR");
        anim.SetBool("Grounded", state == "GROUND");
        if (anim.GetBool("Moving") && anim.GetBool("Grounded"))
            anim.speed = Mathf.Abs(body.velocity.x / move.maxSpeed);
    }

    public void MovementStateChange(string moveState)
    {
        state = moveState;
    }
}
