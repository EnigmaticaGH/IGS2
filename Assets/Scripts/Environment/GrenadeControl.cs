using UnityEngine;
using System.Collections;

public class GrenadeControl : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    //private Rigidbody body;
    private static GameObject[] blocks;
    private ParticleSystem explosion;
    [HideInInspector]
    public Color teamColor;

    // Use this for initialization
    void Start()
    {
        //body = GetComponent<Rigidbody>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        explosion = transform.FindChild("Explosion").GetComponent<ParticleSystem>();
        Exploded = false;
    }

    static void UpdateBlockList()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
    }

    void ExplosionProcess()
    {
        explosion.Play();
        UpdateBlockList();
        Exploded = true;
        foreach (GameObject block in blocks)
        {
            if (Vector3.Distance(block.transform.position, transform.position) < explosionRadius &&
                block.GetComponent<BlockInteraction>() != null)
            {
                Vector3 distance = block.transform.position - transform.position;
                Vector3 force = distance * (explosionForce / distance.sqrMagnitude);
                block.GetComponent<BlockInteraction>().SetParticleColor(teamColor);
                block.GetComponent<BlockInteraction>().Explode(force);
            }
        }

        foreach (GameObject player in PlayerTracker.players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < explosionRadius)
            {
                Vector3 distance = player.transform.position - transform.position;
                Vector3 force = distance * (explosionForce / distance.sqrMagnitude);
                player.GetComponent<Rigidbody>().AddForce(force);
            }
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!Exploded && other.relativeVelocity.sqrMagnitude > 10)
        {
            ExplosionProcess();
        }
    }

    public void Explode()
    {
        if (!Exploded)
        {
            ExplosionProcess();
        }
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(false);
    }

    public bool Exploded
    {
        get;
        set;
    }
}
