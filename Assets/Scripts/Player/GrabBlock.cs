using UnityEngine;
using System.Collections;

public class GrabBlock : MonoBehaviour
{
    private ControllerNumber controller;
    private bool blockGrabPressed;
    private bool axisInUse;
    private GameObject block;
    public float blockThrowForce;
    private bool foundBlock;
    private bool carryingBlock;
    private Transform originalParent;
    private Transform sprite;

    // Use this for initialization
    void Start()
    {
        foreach(Transform t in transform)
        {
            if (t.name == "Sprite")
            {
                sprite = t;
            }
        }
        controller = GetComponent<ControllerNumber>();
        block = null;
        axisInUse = false;
        carryingBlock = false;
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
            if (hit.collider.CompareTag("Block"))
            {
                Debug.Log("Found " + hit.collider.name);
                block = hit.collider.gameObject;
                originalParent = block.transform.parent;
                foundBlock = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        blockGrabPressed = Input.GetAxisRaw("TriggersR_" + controller.controllerNumber) == 1;
        if (blockGrabPressed)
        {
            if (!axisInUse)
            {
                if (!foundBlock && !carryingBlock)
                {
                    CheckForBlock();
                    if (foundBlock && !block.GetComponent<BlockInteraction>().IsGrabbedBySomeoneElse)
                    {
                        CarryBlock();
                        carryingBlock = true;
                    }
                    else
                    {
                        carryingBlock = false;
                    }
                }
                else if (carryingBlock)
                {
                    ThrowBlock();
                    carryingBlock = false;
                }
                axisInUse = true;
            }
        }
        if (!blockGrabPressed)
        {
            axisInUse = false;
        }
        if (carryingBlock)
        {
            block.transform.localScale = Vector3.one * 0.5f;
            block.transform.position = transform.position + Vector3.right * sprite.localScale.x * 0.75f;
        }
    }

    void CarryBlock()
    {
        foundBlock = false;
        block.GetComponent<BlockInteraction>().IsGrabbedBySomeoneElse = true;
        block.GetComponent<BlockInteraction>().StopReset();
        block.GetComponent<Collider>().enabled = false;
        block.GetComponent<BlockInteraction>().SetColor(new Color(0.6f, 0.63f, 0.75f));
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.transform.parent = transform;
    }

    void ThrowBlock()
    {
        const float BASE_VERTICAL_FORCE = 250;
        block.GetComponent<BlockInteraction>().IsGrabbedBySomeoneElse = false;
        block.GetComponent<Collider>().enabled = true;
        block.transform.parent = originalParent;
        block.transform.localScale = Vector3.one;
        float verticalForce = Mathf.Clamp(GetComponentInParent<Rigidbody>().velocity.y * 200, -BASE_VERTICAL_FORCE, blockThrowForce);
        Vector3 force = Vector3.right * blockThrowForce * sprite.localScale.x + Vector3.up * (BASE_VERTICAL_FORCE + verticalForce);
        block.GetComponent<BlockInteraction>().Throw(force, 10, new Color(0.4f, 1, 0.6f));
        foundBlock = false;
    }
}
