using UnityEngine;
using System.Collections.Generic;

public class GrabBlock : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Color teamColor;
    private ControllerNumber controller;
    private GameObject block;
    public float blockThrowForce;
    public bool foundBlock;
    private bool carryingBlock;
    private bool carryingExplosive;
    private Transform originalParent;
    private Transform sprite;
    private Queue<GameObject> originalBlocks;
    private Queue<GameObject> grenadeBlocks;
    private Vector3 forceAngle;
    private Vector3 throwForce;
    private Vector3 blockPos;
    private bool isDead;
    public bool IsInvincible { get; private set; }
    public bool IsThrowing { get; private set; }
    int count;

    void Awake()
    {
        DeathControl.OnDeath += OnDeath;
        DeathControl.OnRespawn += OnRespawn;
    }

    void OnDestroy()
    {
        DeathControl.OnDeath += OnDeath;
        DeathControl.OnRespawn -= OnRespawn;
    }

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
        IsInvincible = false;
        IsThrowing = false;
    }

    void CheckForBlock()
    {
        foundBlock = false;
        Ray ray = new Ray(transform.position + Vector3.right * -sprite.localScale.x + Vector3.down * 2, Vector3.up);
        Ray ray2 = new Ray(transform.position + Vector3.left * -sprite.localScale.x + Vector3.down * 2, Vector3.up);
        Ray ray3 = new Ray(transform.position + Vector3.up + Vector3.left * 2, Vector3.right);

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
        if (isDead)
        {
            return;
        }

        bool blockGrabDown = 
            Input.GetButtonDown(Controls.GrabBlocksControls[0] + controller.controllerNumber) ||
            Input.GetButtonDown(Controls.GrabBlocksControls[1] + controller.controllerNumber) ||
            Input.GetMouseButtonDown(1);
        float x = Input.GetAxis("R_XAxis_" + controller.controllerNumber);
        float y = -Input.GetAxis("R_YAxis_" + controller.controllerNumber);
        forceAngle = new Vector3(x, y);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 m = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            Vector3 mouse = new Vector3(m.x, m.y, 0).normalized;
            if (forceAngle.magnitude == 0)
            {
                forceAngle = mouse;
            }
        }


        if (forceAngle.x > 0 && sprite.localScale.x < 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (forceAngle.x < 0 && sprite.localScale.x > 0)
        {
            sprite.localScale = new Vector3(-1, 1, 1);
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
            carryingBlock = false;
            IsThrowing = true;
            throwForce = forceAngle;
            Invoke("ThrowBlock", 0.05f);
        }
        else if (carryingExplosive && forceAngle.magnitude > 0.7f)
        {
            carryingExplosive = false;
            IsThrowing = true;
            throwForce = forceAngle;
            Invoke("ThrowExplosiveBlock", 0.05f);
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
        blockScript.SetParticleColor(teamColor);
        blockScript.IsGrabbedBySomeoneElse = true;
        block.GetComponent<Collider>().enabled = false;
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.GetComponent<Rigidbody>().useGravity = false;
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
        float xForce = blockThrowForce * throwForce.x;
        float yForce = blockThrowForce * (throwForce.y + 0.1f);
        carryingBlock = false;
        CancelInvoke("DisableInvincibility");
        IsInvincible = true;
        BlockInteraction blockScript = block.GetComponent<BlockInteraction>();
        Vector3 force = new Vector3(xForce, yForce, 0);
        blockScript.IsGrabbedBySomeoneElse = false;
        blockScript.IsBeingThrown = true;
        Invoke("EnableCollision", 0.08f);
        block.transform.parent = originalParent;
        block.GetComponent<Rigidbody>().useGravity = true;
        block.transform.localScale = Vector3.one;
        blockScript.Throw(force, 10, new Color(0.4f, 1, 0.6f));
        foundBlock = false;
        IsThrowing = false;
        Invoke("DisableInvincibility", 0.5f);
    }

    void ThrowExplosiveBlock()
    {
        GrenadeControl grenadeScript = block.GetComponent<GrenadeControl>();
        Vector3 force = new Vector3(blockThrowForce * throwForce.x, blockThrowForce * (throwForce.y + 0.25f), 0);
        CancelInvoke("DisableInvincibility");
        IsInvincible = true;
        carryingExplosive = false;
        Invoke("EnableCollision", 0.08f);
        block.transform.parent = null;
        block.GetComponent<Rigidbody>().isKinematic = false;
        block.GetComponent<Rigidbody>().useGravity = true;
        block.GetComponent<Rigidbody>().AddForce(force);
        grenadeScript.teamColor = teamColor;
        grenadeScript.Exploded = false;
        Invoke("ResetGrenade", 5);
        foundBlock = false;
        IsThrowing = false;
        Invoke("DisableInvincibility", 0.5f);
        GetComponent<PlayerAbilities>().RemovePowerup();
    }

    void EnableCollision()
    {
        block.GetComponent<Collider>().enabled = true;
    }

    void ResetGrenade()
    {
        grenadeBlocks.Peek().GetComponent<GrenadeControl>().Explode();
        Destroy(grenadeBlocks.Dequeue());
        originalBlocks.Peek().SetActive(true);
        originalBlocks.Dequeue().GetComponent<BlockInteraction>().ResetImmediately();
    }

    private void DisableInvincibility()
    {
        IsInvincible = false;
    }

    public bool CarryingBlock
    {
        get { return carryingBlock || carryingExplosive; }
    }

    private void OnDeath(float respawnTime, string sender)
    {
        if (sender == name)
        {
            isDead = true;
            throwForce = Vector2.zero;
            if (carryingBlock)
            {
                ThrowBlock();
            }
            else if (carryingExplosive)
            {
                ThrowExplosiveBlock();
            }
        }
    }

    private void OnRespawn(string sender)
    {
        if (sender == name)
            isDead = false;
    }

    public bool Invincible(GameObject invincibleFrom)
    {
        return IsInvincible && block == invincibleFrom;
    }


}
