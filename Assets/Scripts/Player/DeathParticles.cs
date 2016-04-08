using UnityEngine;
using System.Collections;

public class DeathParticles : MonoBehaviour
{
    private GameObject prefab;
    private GameObject death;
    private ParticleSystem deathParticles;
    private ParticleSystem.EmissionModule em;
    private Rigidbody body;
    private float offset;
    // Use this for initialization
    void Awake()
    {
        DeathControl.OnDeath += Die;
        DeathControl.OnRespawn += Respawn;
        body = GetComponent<Rigidbody>();
        foreach (GameObject g in Resources.LoadAll<GameObject>("Death"))
        {
            prefab = g;
        }
        death = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        death.transform.parent = transform;
        deathParticles = death.GetComponent<ParticleSystem>();
        em = deathParticles.emission;
        deathParticles.Stop();
        em.enabled = false;
        offset = deathParticles.shape.arc / 2;
    }

    void OnDestroy()
    {
        DeathControl.OnDeath -= Die;
        DeathControl.OnRespawn -= Respawn;
    }

    void Update()
    {
        float angle;
        if (transform.position.y < 20)
        {
            angle = 90 - offset;
            if (transform.position.x < -20)
            {
                angle = 45 - offset;
            }
            else if (transform.position.x > 20)
            {
                angle = 135 - offset;
            }
        }
        else
        {
            angle = 270 - offset;
            if (transform.position.x < -20)
            {
                angle = 315 - offset;
            }
            else if (transform.position.x > 20)
            {
                angle = 225 - offset;
            }
        }
        death.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Die(float respawnTime)
    {
        em.enabled = true;
        deathParticles.Play();
    }

    void Respawn()
    {
        deathParticles.Stop();
        em.enabled = false;
    }
}
