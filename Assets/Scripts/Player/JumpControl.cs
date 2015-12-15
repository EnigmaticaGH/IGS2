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
        while (time > 0 && Input.GetKey(KeyCode.Space))
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, jumpStrength, player.velocity.z);
            yield return null;
        }
    }

    IEnumerator JumpKeyDown()
    {
        jumpButtonPressed = true;
        while (canJump && Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        jumpButtonPressed = false;
    }
}
