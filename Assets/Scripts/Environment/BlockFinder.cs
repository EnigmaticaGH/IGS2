using UnityEngine;
using System.Collections;

public class BlockFinder : MonoBehaviour
{
    private static GameObject[] blocks;
    public GameObject squarePrefab;
    public GameObject cloudsPrefab;
    // Use this for initialization
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(GameObject block in blocks)
        {
            GameObject clouds = (GameObject)Instantiate(cloudsPrefab, block.transform.position + Vector3.back * 0.48f, Quaternion.identity);
            clouds.transform.parent = block.transform;
            block.transform.position = new Vector3(Mathf.Round(block.transform.position.x), Mathf.Round(block.transform.position.y), Mathf.Round(block.transform.position.z));
        }
    }
}
