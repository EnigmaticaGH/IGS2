using UnityEngine;
using System.Collections;

public class StunParticles : MonoBehaviour
{
    private GameObject prefab;
    private ParticleSystem.EmissionModule em;

    void Awake()
    {
        foreach (GameObject g in Resources.LoadAll<GameObject>("Stun"))
        {
            prefab = (GameObject)Instantiate(g, transform.position + Vector3.up, Quaternion.identity);
        }
        em = prefab.GetComponent<ParticleSystem>().emission;
        em.enabled = false;
    }

    void Update()
    {
        prefab.transform.position = transform.position + Vector3.up * 0.5f + Vector3.right * Mathf.Sin(Time.time * Mathf.PI * 2) * 0.4f + Vector3.back;
    }

    public void Stun(float time)
    {
        em.enabled = true;
        Invoke("StopStun", time);
    }

    private void StopStun()
    {
        em.enabled = false;
    }
}
