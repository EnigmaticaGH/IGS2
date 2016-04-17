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
            GameObject square = (GameObject)Instantiate(squarePrefab, block.transform.position + Vector3.back * 0.49f, Quaternion.identity);
            GameObject clouds = (GameObject)Instantiate(cloudsPrefab, block.transform.position, Quaternion.identity);
            square.GetComponent<SpriteRenderer>().color = Color.clear;
            square.transform.parent = block.transform;
            clouds.transform.parent = block.transform;
            block.transform.position = new Vector3(Mathf.Round(block.transform.position.x), Mathf.Round(block.transform.position.y), Mathf.Round(block.transform.position.z));
        }
    }
}
