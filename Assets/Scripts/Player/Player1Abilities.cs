using UnityEngine;
using System.Collections;

public class Player1Abilities : MonoBehaviour
{
    public GameObject windTunnelPrefab;
    private int controllerNumber;
    private Rigidbody player;
    private Movement movement;
    private const float ABILITY_B_FORCE = 600;
    //Assign new abilities here
    private Ability[] abilities;

    // Use this for initialization
    void Awake()
    {
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        controllerNumber = GetComponent<ControllerNumber>().controllerNumber;
        GetComponent<ConstantForce>().enabled = false;
        abilities = new Ability[]
        {
            new Ability("Block Smash", "B", 0.5f),
            new Ability("Wind Tunnel", "X", 5,
                new GameObject[1]
                {
                    (GameObject)Instantiate(windTunnelPrefab, Vector3.up * 20, Quaternion.identity)
                })
        };
        foreach (Ability ability in abilities)
        {
            //Initially set all abilities to READY
            ability.AbilityStatus = Ability.Status.READY;
            //Deactivate all objects associated with ability
            if (ability.Objects != null)
                foreach(GameObject g in ability.Objects)
                {
                    g.SetActive(false);
                }
            //Add abilities to a registry of abilities
            AbilityRegistry.RegisterAbility(name, ability);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Ability ability in abilities)
        {
            //Check for input on abilities
            if (Input.GetButtonDown(ability.Button + "_" + controllerNumber) && ability.AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + ability.Button + "_Activate", ability);
            }
        }
    }

    void SetVelocityToZero()
    {
        player.velocity = Vector3.zero;
    }

    IEnumerator Ability_B_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;
        // Ability code here
        if (Input.GetAxis("L_YAxis_" + controllerNumber) < -0.5f)
        {
            //shoot up
            player.AddForce(Vector3.up * ABILITY_B_FORCE);
            movement.Disable(0.5f);
            Invoke("SetVelocityToZero", 0.5f);
        }
        else if (Input.GetAxis("L_YAxis_" + controllerNumber) > 0.5f)
        {
            //shoot down
            player.AddForce(Vector3.down * ABILITY_B_FORCE);
            movement.Disable(0.5f);
        }
        else if (Input.GetAxis("L_XAxis_" + controllerNumber) > 0.5f)
        {
            //shoot right
            player.useGravity = false;
            player.MovePosition(transform.position + Vector3.up * 0.1f);
            player.AddForce(Vector3.right * ABILITY_B_FORCE);
            movement.Disable(0.5f);
        }
        else if (Input.GetAxis("L_XAxis_" + controllerNumber) < -0.5f)
        {
            //shoot left
            player.useGravity = false;
            player.MovePosition(transform.position + Vector3.up * 0.1f);
            player.AddForce(Vector3.left * ABILITY_B_FORCE);
            movement.Disable(0.5f);
        }
        // -----------------
        yield return new WaitForSeconds(0.5f);
        player.useGravity = true;
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);
        ability.AbilityStatus = Ability.Status.READY;
    }

    IEnumerator Ability_X_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;
        float axis = Input.GetAxis("L_XAxis_" + controllerNumber);
        int direction = axis >= 0 ? 1 : -1;
        float activeTimer = 4;
        ability.Objects[0].SetActive(true);
        ability.Objects[0].transform.position = Vector3.up * (transform.position.y + 2) + Vector3.right * direction * 20;
        while((activeTimer -= Time.deltaTime) > 0)
        {
            foreach(GameObject player in PlayerTracker.players)
            {
                player.GetComponent<ConstantForce>().force = Vector3.left * direction * 2;
                if (player.transform.position.y > ability.Objects[0].transform.position.y - 3
                 && player.transform.position.y < ability.Objects[0].transform.position.y + 3)
                {
                    player.GetComponent<ConstantForce>().enabled = true;
                    player.GetComponent<Movement>().UseForceInstead(Time.deltaTime);
                }
                else
                {
                    player.GetComponent<ConstantForce>().enabled = false;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        foreach (GameObject player in PlayerTracker.players)
        {
            player.GetComponent<ConstantForce>().enabled = false;
        }
        ability.Objects[0].SetActive(false);
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);
        ability.AbilityStatus = Ability.Status.READY;
    }
}
