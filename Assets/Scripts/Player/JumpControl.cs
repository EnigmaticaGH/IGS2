using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private bool canJump;
    private bool jumpButtonPressed;
    public float jumpStrength;
    public float maxJumpTime;
    private Rigidbody player;
    public delegate void JumpState();
    public static event JumpState OnJump;

    void Awake()
    {
        Movement.StateChangeEvent += MovementState;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    void OnDestroy()
    {
        Movement.StateChangeEvent -= MovementState;
    }

    void Update()
    {
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(JumpKeyDown());
        }
    }

    void MovementState(string state)
    {
        canJump = state == "GROUND";
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed)
        {
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        float time = maxJumpTime;
        if (OnJump != null)
            OnJump();
        while (time > 0)
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            if (Input.GetKeyUp(KeyCode.Space))
            {
                time = 0;
            }
            yield return null;
        }
    }

    IEnumerator JumpKeyDown()
    {
        float time = 0.4f;
        jumpButtonPressed = true;
        while (time > 0 && canJump)
        {
            time -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                time = 0;
            }
            yield return null;
        }
        jumpButtonPressed = false;
    }
}
