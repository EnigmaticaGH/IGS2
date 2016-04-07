using UnityEngine;
using System.Collections;

public class LandingParticles : MonoBehaviour
{
    private GameObject prefab;
    private GameObject left;
    private GameObject right;
    public Transform canvas;
    //private IEnumerator land;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject g in Resources.LoadAll<GameObject>("Landing"))
        {
            prefab = g;
        }
        left = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
        right = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
        left.SetActive(false);
        right.SetActive(false);
        left.transform.localScale = new Vector3(-1, 1, 1);
        //land = Land(1);
    }

    public void StartLandingAnimation()
    {
        StartCoroutine(Land(0.25f));
    }

    IEnumerator Land(float animationTime)
    {
        Quaternion leftRot = Quaternion.Euler(new Vector3(0, 0, 45));
        Quaternion rightRot = Quaternion.Euler(new Vector3(0, 0, -45));
        Vector3 pos = transform.position + Vector3.up * -0.8f;
        float maxt = animationTime;
        left.SetActive(true);
        right.SetActive(true);
        while ((animationTime -= Time.deltaTime) > 0)
        {
            float normalizedTime = animationTime / maxt;
            left.transform.position = Vector3.Lerp(pos, pos + Vector3.left, 1 - normalizedTime);
            left.transform.rotation = Quaternion.Lerp(leftRot, Quaternion.identity, 1 - normalizedTime);
            left.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, (1 - normalizedTime)));

            right.transform.position = Vector3.Lerp(pos, pos + Vector3.right, 1 - normalizedTime);
            right.transform.rotation = Quaternion.Lerp(rightRot, Quaternion.identity, 1 - normalizedTime);
            right.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, (1 - normalizedTime)));
            yield return new WaitForFixedUpdate();
        }
        left.SetActive(false);
        right.SetActive(false);
    }
}
