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
        carryingExplosive = false;
        originalBlocks = new Queue<GameObject>();
        grenadeBlocks = new Queue<GameObject>();
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
        //blockGrabPressed = Input.GetAxisRaw("TriggersR_" + controller.controllerNumber) == 1;
        blockGrabPressed = Input.GetAxisRaw("RB_" + controller.controllerNumber) == 1;
        if (blockGrabPressed)
        {
            if (!axisInUse)
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
                    }
                    else
                    {
                        carryingBlock = false;
                        carryingExplosive = false;
                    }
                }
                else if (carryingBlock && !carryingExplosive)
                {
                    ThrowBlock();
                    carryingBlock = false;
                }
                else if (carryingBlock && carryingExplosive)
                {
                    carryingExplosive = false;
                    carryingBlock = false;
                    ThrowExplosiveBlock();
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
        BlockInteraction blockScript = block.GetComponent<BlockInteraction>();
        blockScript.IsGrabbedBySomeoneElse = true;
        //blockScript.StopReset();
        block.GetComponent<Collider>().enabled = false;
        blockScript.SetColor(new Color(0.6f, 0.63f, 0.75f));
        var em = blockScript.GrabParticleSystem.emission;
        blockScript.GrabParticleSystem.startColor = new Color(0.6f, 0.63f, 0.75f);
        em.enabled = true;
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.transform.parent = transform;
    }

    void CarryExplosiveBlock()
    {
        //block.GetComponent<BlockInteraction>().StopReset();
        foundBlock = false;
        block.SetActive(false);
        originalBlocks.Enqueue(block);
        block = (GameObject)Instantiate(grenadePrefab, Vector3.up * 100, Quaternion.identity);
        Debug.Log(originalBlocks.Peek().name);
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
        blockScript.IsGrabbedBySomeoneElse = false;
        block.GetComponent<Collider>().enabled = true;
        block.transform.parent = originalParent;
        block.transform.localScale = Vector3.one;
        float verticalForce = Mathf.Clamp(GetComponentInParent<Rigidbody>().velocity.y * 200, -BASE_VERTICAL_FORCE, blockThrowForce);
        Vector3 force = Vector3.right * blockThrowForce * sprite.localScale.x + Vector3.up * (BASE_VERTICAL_FORCE + verticalForce);
        blockScript.Throw(force, 10, new Color(0.4f, 1, 0.6f));
        var em = blockScript.GrabParticleSystem.emission;
        blockScript.GrabParticleSystem.startColor = new Color(0.4f, 1, 0.6f);
        foundBlock = false;
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
    }

    void ResetGrenade()
    {
        grenadeBlocks.Peek().GetComponent<GrenadeControl>().Explode();
        Destroy(grenadeBlocks.Dequeue());
        originalBlocks.Peek().SetActive(true);
        originalBlocks.Dequeue().GetComponent<BlockInteraction>().ResetImmediately();
    }
}
