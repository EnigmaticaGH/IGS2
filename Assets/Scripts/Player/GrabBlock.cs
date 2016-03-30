using UnityEngine;
using System.Collections.Generic;

public class GrabBlock : MonoBehaviour
{
    public GameObject grenadePrefab;
    private ControllerNumber controller;
    private bool blockGrabPressed;
    private bool axisInUse;
    private GameObject block;
    public float blockThrowForce;
    private bool foundBlock;
    private bool carryingBlock;
    private bool carryingExplosive;
    private Transform originalParent;
    private Transform sprite;
    private Queue<GameObject> originalBlocks;
    private Queue<GameObject> grenadeBlocks;
    private const float BASE_VERTICAL_FORCE = 250;
    private bool canThrow;
    private bool throwing;
    private float angle;

    // Use this for initialization
    void Start()
    {
        foreach(Transform t in transform)
        {
            if (t.name.Contains("Sprite"))
            {
                sprite = t;
            }
        }
        //Debug.Log(name);
        controller = GetComponent<ControllerNumber>();
        block = null;
        axisInUse = false;
        carryingBlock = false;
        carryingExplosive = false;
        originalBlocks = new Queue<GameObject>();
        grenadeBlocks = new Queue<GameObject>();
        canThrow = false;
        throwing = false;
    }

    void CheckForBlock()
    {
        foundBlock = false;
        Ray ray = new Ray(transform.position + Vector3.right * sprite.localScale.x + Vector3.down * 1.5f, Vector3.up);
        Ray ray2 = new Ray(transform.position + Vector3.left * sprite.localScale.x * 1.2f + Vector3.down * 1.5f, Vector3.up);
        Ray ray3 = new Ray(transform.position + Vector3.up + Vector3.left * 1.5f, Vector3.right);

        RayCast(ray3);
        RayCast(ray2);
        RayCast(ray);
        if (!foundBlock)
        {
            block = null;
        }
    }

    void RayCast(Ray ray)
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2, Color.black);
        RaycastHit[] hits = Physics.RaycastAll(ray, 2);
        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;
            if (collider.CompareTag("Block") && collider.gameObject.GetComponent<GrenadeControl>() == null) //cannot grab explosive blocks. Not that you'd want to, anyways.
            {
                block = hit.collider.gameObject;
                originalParent = block.transform.parent;
                foundBlock = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //blockGrabPressed = Input.GetAxisRaw("TriggersR_" + controller.controllerNumber) == 1;
        blockGrabPressed = Input.GetAxisRaw("TriggersR_" + controller.controllerNumber) == 1 || Input.GetAxisRaw("RB_" + controller.controllerNumber) == 1;
        bool blockGrabDown = Input.GetButtonDown("RB_" + controller.controllerNumber);
        bool blockGrab = Input.GetButton("RB_" + controller.controllerNumber);
        bool blockGrabUp = Input.GetButtonUp("RB_" + controller.controllerNumber);
        if (blockGrabDown)
        {
            if (!foundBlock && !carryingBlock)
            {
                CheckForBlock();
                if (foundBlock && !block.GetComponent<BlockInteraction>().IsGrabbedBySomeoneElse)
                {
                    if (AbilityRegistry.AbilityStatus(name, "GrenadeBlock") == Ability.Status.ACTIVE)
                    {
                        carryingExplosive = true;
                        CarryExplosiveBlock();
                    }
                    else
                    {
                        CarryBlock();
                    }
                    carryingBlock = true;
                    canThrow = true;
                }
                else
                {
                    carryingBlock = false;
                    carryingExplosive = false;
                }
            }
        }
        if (blockGrab && canThrow)
        {
            if (carryingBlock && !carryingExplosive)
            {
                ThrowBlock();
            }
            else if (carryingBlock && carryingExplosive)
            {
                ThrowExplosiveBlock();
            }
        }
        if (carryingBlock)
        {
            block.transform.position = transform.position + Vector3.right * sprite.localScale.x * 0.75f;
        }
    }

    void CarryBlock()
    {
        foundBlock = false;
        BlockInteraction blockScript = block.GetComponent<BlockInteraction>();
        blockScript.IsGrabbedBySomeoneElse = true;
        block.GetComponent<Collider>().enabled = false;
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.transform.position = transform.position + Vector3.right * sprite.localScale.x * 0.75f;
        block.transform.localScale = Vector3.one * 0.5f;
        block.transform.parent = transform;
    }

    void CarryExplosiveBlock()
    {
        //block.GetComponent<BlockInteraction>().StopReset();
        foundBlock = false;
        block.SetActive(false);
        originalBlocks.Enqueue(block);
        block = (GameObject)Instantiate(grenadePrefab, Vector3.up * 100, Quaternion.identity);
        grenadeBlocks.Enqueue(block);
        block.SetActive(true);
        block.transform.rotation = Quaternion.identity;
        block.GetComponent<Rigidbody>().isKinematic = true;
        block.GetComponent<Rigidbody>().useGravity = false;
        block.GetComponent<Collider>().enabled = false;
        block.transform.parent = transform;
    }

    void ThrowBlock()
    {
        float x = Input.GetAxis("R_XAxis_" + controller.controllerNumber);
        float y = -Input.GetAxis("R_YAxis_" + controller.controllerNumber);
        Vector3 forceAngle = new Vector3(x, y);
        BlockInteraction blockScript = block.GetComponent<BlockInteraction>();
        
        if (forceAngle.magnitude > 0.5f)
        {
            throwing = true;
            forceAngle = forceAngle.normalized;
            blockScript.IsGrabbedBySomeoneElse = false;
            block.GetComponent<Collider>().enabled = true;
            block.transform.parent = originalParent;
            block.transform.localScale = Vector3.one;
            Vector3 force = new Vector3(blockThrowForce * forceAngle.x, blockThrowForce * forceAngle.y, 0);
            blockScript.Throw(force, 10, new Color(0.4f, 1, 0.6f));
            var em = blockScript.GrabParticleSystem.emission;
            blockScript.GrabParticleSystem.startColor = new Color(0.4f, 1, 0.6f);
            foundBlock = false;
            canThrow = false;
            carryingBlock = false;
        }
    }

    void ThrowExplosiveBlock()
    {
        GrenadeControl grenadeScript = block.GetComponent<GrenadeControl>();
        block.GetComponent<Collider>().enabled = true;
        block.transform.parent = null;
        float verticalForce = Mathf.Clamp(GetComponentInParent<Rigidbody>().velocity.y * 200, -BASE_VERTICAL_FORCE, blockThrowForce);
        Vector3 force = Vector3.right * blockThrowForce * sprite.localScale.x + Vector3.up * (BASE_VERTICAL_FORCE + verticalForce);
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.GetComponent<Rigidbody>().useGravity = true;
        block.GetComponent<Rigidbody>().AddForce(force);
        grenadeScript.Exploded = false;
        Invoke("ResetGrenade", 5);
        foundBlock = false;
        carryingExplosive = false;
        carryingBlock = false;
    }

    void ResetGrenade()
    {
        grenadeBlocks.Peek().GetComponent<GrenadeControl>().Explode();
        Destroy(grenadeBlocks.Dequeue());
        originalBlocks.Peek().SetActive(true);
        originalBlocks.Dequeue().GetComponent<BlockInteraction>().ResetImmediately();
    }

    public bool Throwing
    {
        get { return throwing; }
    }
}
