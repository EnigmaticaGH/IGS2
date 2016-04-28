using UnityEngine;
using System.Collections;

public class BlockFinder : MonoBehaviour
{
    private static GameObject[] blocks;
    // Use this for initialization
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(GameObject block in blocks)
        {
            block.transform.position = new Vector3(Mathf.Round(block.transform.position.x), Mathf.Round(block.transform.position.y), Mathf.Round(block.transform.position.z));
        }
    }
}
