using UnityEngine;
using System.Collections;

public class BlockFinder : MonoBehaviour
{
    private static GameObject[] blocks;
    public GameObject squarePrefab;
    // Use this for initialization
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(GameObject block in blocks)
        {
            GameObject square = (GameObject)Instantiate(squarePrefab, block.transform.position + Vector3.back * 0.49f, Quaternion.identity);
            square.GetComponent<SpriteRenderer>().color = Color.clear;
            square.transform.parent = block.transform;
        }
    }
}
