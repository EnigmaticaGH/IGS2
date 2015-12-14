using UnityEngine;
using System.Collections;

public class JumpControl : MonoBehaviour
{
    private bool canJump;
    private bool jumpButtonPressed;
    public float jumpStrength;
    public float maxJumpTime;
    private Rigidbody player;

    void Awake()
    {
        Movement.AbleToJump += AbleToJump;
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    void OnDestroy()
    {
        Movement.AbleToJump -= AbleToJump;
    }

    void Update()
    {
        if (canJump && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(JumpKeyDown());
        }
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed)
        {
            StartCoroutine(JumpTimer());
        }
    }

    void AbleToJump(bool status)
    {
        canJump = status;
    }

    IEnumerator JumpTimer()
    {
        float time = maxJumpTime;
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
