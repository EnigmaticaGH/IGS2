using UnityEngine;
using System.Collections.Generic;

public class GrabBlock : MonoBehaviour
{
    public GameObject grenadePrefab;
    private ControllerNumber controller;
    private GameObject block;
    public float blockThrowForce;
    private bool foundBlock;
    private bool carryingBlock;
    public bool carryingExplosive;
    private Transform originalParent;
    private Transform sprite;
    private Queue<GameObject> originalBlocks;
    private Queue<GameObject> grenadeBlocks;
    private Vector3 forceAngle;
    private Vector3 blockPos;

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
        controller = GetComponent<ControllerNumber>();
        block = null;
        carryingBlock = false;
        carryingExplosive = false;
        originalBlocks = new Queue<GameObject>();
        grenadeBlocks = new Queue<GameObject>();
        blockPos = Vector3.right;
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
        bool blockGrabDown = Input.GetButtonDown("RB_" + controller.controllerNumber);
        float x = Input.GetAxis("R_XAxis_" + controller.controllerNumber);
        float y = -Input.GetAxis("R_YAxis_" + controller.controllerNumber);
        forceAngle = new Vector3(x, y);
        if (x > 0 && sprite.localScale.x < 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (x < 0 && sprite.localScale.x > 0)
        {
            sprite.localScale = new Vector3(-1, 1, 1);
        }
        if (y > 0)
        {
            blockPos = Vector3.up * 1.5f;
        }
        else
        {
            blockPos = Vector3.right * sprite.localScale.x;
        }
        if (blockGrabDown)
        {
            if (!foundBlock && !carryingBlock && !carryingExplosive)
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
                        carryingBlock = true;
                        CarryBlock();
                    }
                }
                else
                {
                    carryingBlock = false;
                    carryingExplosive = false;
                }
            }
        }

        if (carryingBlock && forceAngle.magnitude > 0.7f)
        {
            ThrowBlock();
        }
        else if (carryingExplosive && forceAngle.magnitude > 0.7f)
        {
            ThrowExplosiveBlock();
        }

        if (carryingBlock || carryingExplosive)
        {
            block.transform.position = transform.position + blockPos;
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
        BlockInteraction blockScript = block.GetComponent<BlockInteraction>();
        Vector3 force = new Vector3(blockThrowForce * forceAngle.x, blockThrowForce * (forceAngle.y + 0.25f), 0);
        blockScript.IsGrabbedBySomeoneElse = false;
        block.GetComponent<Collider>().enabled = true;
        block.transform.parent = originalParent;
        block.transform.localScale = Vector3.one;
        blockScript.Throw(force, 10, new Color(0.4f, 1, 0.6f));
        foundBlock = false;
        carryingBlock = false;
    }

    void ThrowExplosiveBlock()
    {
        GrenadeControl grenadeScript = block.GetComponent<GrenadeControl>();
        Vector3 force = new Vector3(blockThrowForce * forceAngle.x, blockThrowForce * (forceAngle.y + 0.25f), 0);
        block.GetComponent<Collider>().enabled = true;
        block.transform.parent = null;
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.GetComponent<Rigidbody>().useGravity = true;
        block.GetComponent<Rigidbody>().AddForce(force);
        grenadeScript.Exploded = false;
        Invoke("ResetGrenade", 5);
        foundBlock = false;
        carryingExplosive = false;
    }

    void ResetGrenade()
    {
        grenadeBlocks.Peek().GetComponent<GrenadeControl>().Explode();
        Destroy(grenadeBlocks.Dequeue());
        originalBlocks.Peek().SetActive(true);
        originalBlocks.Dequeue().GetComponent<BlockInteraction>().ResetImmediately();
    }

    public bool CarryingBlock
    {
        get { return carryingBlock || carryingExplosive; }
    }
}
