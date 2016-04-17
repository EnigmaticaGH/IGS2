using UnityEngine;
using System.Collections;

public class DeathParticles : MonoBehaviour
{
    private Transform upperBounds;
    private Transform lowerBounds;
    private GameObject prefab;
    private GameObject death;
    private ParticleSystem deathParticles;
    private Color teamColor;
    private float offset;
    // Use this for initialization
    void Awake()
    {
        DeathControl.OnDeath += Die;
        foreach (GameObject g in Resources.LoadAll<GameObject>("Death"))
        {
            prefab = g;
        }
        death = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        death.transform.parent = transform;
        deathParticles = death.GetComponent<ParticleSystem>();
        offset = deathParticles.shape.arc / 2;
        teamColor = GetComponent<GrabBlock>().teamColor;
        deathParticles.subEmitters.birth0.startColor = teamColor;
    }

    void Start()
    {
        upperBounds = GameObject.Find("UpperBounds").transform;
        lowerBounds = GameObject.Find("LowerBounds").transform;
    }

    void OnDestroy()
    {
        DeathControl.OnDeath -= Die;
    }

    void Update()
    {
        float angle;
        if (transform.position.y < upperBounds.position.y)
        {
            if (transform.position.x < lowerBounds.position.x)
            {
                angle = 45 - offset;
            }
            else if (transform.position.x > upperBounds.position.x)
            {
                angle = 135 - offset;
            }
            else
            {
                angle = 90 - offset;
            }
        }
        else
        {
            if (transform.position.x < lowerBounds.position.x)
            {
                angle = 315 - offset;
            }
            else if (transform.position.x > upperBounds.position.x)
            {
                angle = 225 - offset;
            }
            else
            {
                angle = 270 - offset;
            }
        }
        death.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Die(float respawnTime, string sender)
    {
        if (sender == name)
            deathParticles.Play();
    }
}
