using UnityEngine;
using System.Collections;

public class HoverControl : MonoBehaviour
{
    public delegate void HoverState();
    public static event HoverState OnHoverStartOrResume;
    public static event HoverState OnHoverPause;
    public static event HoverState OnHoverDone;

    private Rigidbody player;
    private bool canHover;
    public float maxHoverTime;
    private bool hoverButtonPressed;
    private bool inAir;

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
        inAir = state == "AIR" || state == "JUMP" || state == "HOVER" || state == "LANDING";
    }

    void Update()
    {
        if (canHover && Input.GetKeyDown(KeyCode.Space))
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
        if (OnHoverStartOrResume != null)
            OnHoverStartOrResume();
        while (time > 0 && inAir)
        {
            time -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (OnHoverStartOrResume != null)
                    OnHoverStartOrResume();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
                player.useGravity = false;
            }
            else
            {
                if (OnHoverPause!= null)
                    OnHoverPause();
                player.useGravity = true;
            }
            yield return null;
        }
        player.useGravity = true;
        if (OnHoverDone != null)
            OnHoverDone();
    }

    IEnumerator HoverKeyDown()
    {
        hoverButtonPressed = true;
        while (canHover && Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        hoverButtonPressed = false;
    }
}
