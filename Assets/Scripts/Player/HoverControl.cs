using UnityEngine;
using System.Collections;

public class HoverControl : MonoBehaviour
{
    public delegate void HoverState();
    public static event HoverState OnHoverStart;
    public static event HoverState OnHoverDone;

    private Rigidbody player;
    private bool canHover;
    public float maxHoverTime;
    private bool hoverButtonPressed;

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

    void MovementState(string state)
    {
        canHover = state == "AIR" || state == "JUMP";
    }

    void Update()
    {
        if (canHover && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(HoverKeyDown());
        }
    }

    void FixedUpdate()
    {
        if (hoverButtonPressed)
        {
            StartCoroutine(HoverTimer());
        }
    }

    IEnumerator HoverTimer()
    {
        float time = maxHoverTime;
        if (OnHoverStart != null)
            OnHoverStart();
        while (time > 0)
        {
            time -= Time.deltaTime;
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.useGravity = false;
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                time = 0;
            }
            yield return null;
        }
        player.useGravity = true;
        if (OnHoverDone != null)
            OnHoverDone();
    }

    IEnumerator HoverKeyDown()
    {
        float time = 0.1f;
        hoverButtonPressed = true;
        while (time > 0 && canHover)
        {
            time -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                time = 0;
            }
            yield return null;
        }
        hoverButtonPressed = false;
    }
}
