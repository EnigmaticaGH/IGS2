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

    private Vector3 posX;
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
        posX = new Vector3(transform.position.x, 1, 0);
        pos = transform.position;
    }

    //Spawn block in front of player
    IEnumerator Trap1Activate()
    {
        trap1activated = true;

        trap1block.SetActive(true);
        trap1block.transform.position = pos + Vector3.right * 2.5f + Vector3.up * 0.25f;

        yield return new WaitForSeconds(trap1cooldown);
        trap1block.SetActive(false);
        trap1activated = false;
    }
    //confine player
    IEnumerator Trap2Activate()
    {
        float netCooldownTime;

        trap2activated = true;

        trap2walls[0].transform.position = posX + Vector3.right * 2f + Vector3.up * -5f;
        trap2walls[1].transform.position = posX + Vector3.left * 2f + Vector3.up * -3.5f;
        trap2walls[0].SetActive(true);
        trap2walls[1].SetActive(true);

        //Shoot up from the bottom of the screen quickly
        for(int i = 0; i < 20; i++)
        {
            trap2walls[0].transform.position += Vector3.up * 0.25f;
            trap2walls[1].transform.position += Vector3.up * 0.25f;
            yield return new WaitForFixedUpdate();
        }
        //stay in position for a bit
        for(int i = 0; i < 20; i++) { yield return new WaitForFixedUpdate(); }
        //slowly move in on the player
        for(int i = 0; i < 144; i++)
        {
            trap2walls[0].transform.position += Vector3.left * 0.01f;
            trap2walls[1].transform.position += Vector3.right * 0.01f;
            yield return new WaitForFixedUpdate();
        }
        //if the player is caught inside when the walls have finished collapsing,
        //kill the player
        if (posX.x < trap2walls[0].transform.position.x 
            && posX.x > trap2walls[1].transform.position.x
            && pos.y < trap2walls[1].transform.position.y + 2.5f)
        {
            netCooldownTime = trap2cooldown - playerLife.respawnTime;
            playerLife.Kill();
            yield return new WaitForSeconds(playerLife.respawnTime);
            trap2walls[0].SetActive(false);
            trap2walls[1].SetActive(false);
        }
        else
        {
            netCooldownTime = trap2cooldown;
        }
        //start trap cooldown
        yield return new WaitForSeconds(netCooldownTime);
        trap2walls[0].SetActive(false);
        trap2walls[1].SetActive(false);
        trap2activated = false;
    }
}
