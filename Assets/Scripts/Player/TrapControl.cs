using UnityEngine;
using System.Collections;

public class TrapControl : MonoBehaviour
{
    private string trap1;
    private string trap2;

    public float trap1cooldown;
    public float trap2cooldown;

    private bool trap1activated;
    private bool trap2activated;

    private Vector3 pos;
    public GameObject block;
    public GameObject wall;
    private GameObject trap1block;
    private GameObject[] trap2walls = new GameObject[2];

    private DeathControl playerLife;

    // Use this for initialization
    void Start()
    {
        trap1 = "B_1";
        trap2 = "X_1";
        trap1activated = false;
        trap2activated = false;

        trap1block = (GameObject)Instantiate(block, Vector3.zero, Quaternion.identity);
        trap1block.SetActive(false);

        trap2walls[0] = (GameObject)Instantiate(wall, Vector3.zero, Quaternion.identity);
        trap2walls[1] = (GameObject)Instantiate(wall, Vector3.zero, Quaternion.identity);
        trap2walls[0].SetActive(false);
        trap2walls[1].SetActive(false);

        playerLife = GameObject.Find("Player").GetComponent<DeathControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(trap1) && !trap1activated)
        {
            StartCoroutine(Trap1Activate());
        }
        if (Input.GetButton(trap2) && !trap2activated)
        {
            StartCoroutine(Trap2Activate());
        }
        pos = transform.position;
    }

    //Shove block up in front of player
    IEnumerator Trap1Activate()
    {
        trap1activated = true;

        trap1block.SetActive(true);
        trap1block.transform.position = pos + Vector3.right * 2 + Vector3.up * 0.5f;

        yield return new WaitForSeconds(trap1cooldown);
        trap1block.SetActive(false);
        trap1activated = false;
    }
    //confine player
    IEnumerator Trap2Activate()
    {
        trap2activated = true;

        trap2walls[0].transform.position = pos + Vector3.right * 2f;
        trap2walls[1].transform.position = pos + Vector3.left * 2f + Vector3.up * 1.5f;
        trap2walls[0].SetActive(true);
        trap2walls[1].SetActive(true);

        for(int i = 0; i < 144; i++)
        {
            trap2walls[0].transform.position += Vector3.left * 0.01f;
            trap2walls[1].transform.position += Vector3.right * 0.01f;
            yield return new WaitForFixedUpdate();
        }

        if (pos.x < trap2walls[0].transform.position.x && pos.x > trap2walls[1].transform.position.x)
        {
            playerLife.Kill();
        }

        yield return new WaitForSeconds(trap2cooldown);
        trap2walls[0].SetActive(false);
        trap2walls[1].SetActive(false);
        trap2activated = false;
    }
}
