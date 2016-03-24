using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{
    public float grenadeThrowForce;
    public GameObject grenadePrefab;

    private Dictionary<string, GameObject> particleSystems;
    private GameObject sprite;
    private ControllerNumber controller;
    private Rigidbody player;
    private Movement movement;
    private TrailRenderer dashTrail;
    private const float ABILITY_B_FORCE = 600;
    //Assign new abilities here
    private Ability[] abilities;
    private bool usedAirDash;

    enum Powerup : int
    {
        None = -1,
        BlockDrop = 1,
        GrenadeBlock = 2
    };
    Powerup currentPowerup;

    // Use this for initialization
    void Awake()
    {
        dashTrail = GetComponent<TrailRenderer>();
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        controller = GetComponent<ControllerNumber>();
        sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        particleSystems = new Dictionary<string, GameObject>();
        foreach(GameObject ps in Resources.LoadAll<GameObject>("Particles"))
        {
            //Debug.Log(ps.name);
            particleSystems.Add(ps.name, (GameObject)Instantiate(ps, transform.position, Quaternion.identity));
        }
        foreach(KeyValuePair<string, GameObject> ps in particleSystems)
        {
            ParticleSystem.EmissionModule em = ps.Value.GetComponent<ParticleSystem>().emission;
            ps.Value.transform.position = transform.position + ps.Value.GetComponent<ParticlePosition>().relativePosition;
            ps.Value.transform.parent = transform;
            em.enabled = false;
        }
        abilities = new Ability[]
        {
            new Ability("BlockSmash", 0.5f, 0.125f, 0.5f,
                new string[]
                {
                    "R_XAxis",
                    "R_YAxis"
                }),
            new Ability(Powerup.BlockDrop.ToString(), 0, 2.5f),
            new Ability(Powerup.GrenadeBlock.ToString(), 0, 7.5f)
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
        usedAirDash = false;
        dashTrail.time = 0;
        currentPowerup = Powerup.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.State == Movement.MovementState.GROUND)
            usedAirDash = false;
        foreach (Ability ability in abilities)
        {
            //Check for input on abilities
            if (ability.Button != "" && Input.GetButtonDown(ability.Button + "_" + controller.controllerNumber) && ability.AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + ability.Name + "_Activate", ability);
                Debug.Log(ability.Button + "_" + controller.controllerNumber);
            }
            else if (ability.Axis != null)
            {
                foreach(string axisName in ability.Axis)
                {
                    if(Mathf.Abs(Input.GetAxis(axisName + "_" + controller.controllerNumber)) > ability.AxisThreshold && ability.AbilityStatus == Ability.Status.READY)
                    {
                        StartCoroutine("Ability_" + ability.Name + "_Activate", ability);
                        Debug.Log(name + axisName + "_" + controller.controllerNumber);
                    }
                }
            }
        }

        if (currentPowerup != Powerup.None)
        {
            if (abilities[(int)currentPowerup].AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + currentPowerup.ToString() + "_Activate", abilities[(int)currentPowerup]);
            }
        }
    }

    void ZeroVelocity()
    {
        player.velocity = Vector3.zero;
    }

    #region Ability 1

    IEnumerator Ability_BlockSmash_Activate(Ability ability)
    {
        string axisX = ability.Axis[0];
        string axisY = ability.Axis[1];
        float threshold = ability.AxisThreshold;
        dashTrail.time = ability.ActiveTime * 2;
        ability.AbilityStatus = Ability.Status.ACTIVE;
        if (movement.State != Movement.MovementState.GROUND && !usedAirDash)
        {
            usedAirDash = true;
        }
        else if (movement.State != Movement.MovementState.GROUND && usedAirDash)
        {
            ability.AbilityStatus = Ability.Status.READY;
            yield break;
        }
        // Ability code here
        if (Input.GetAxis(axisY + "_" + controller.controllerNumber) < -threshold)
        {
            //shoot up
            player.AddForce(Vector3.up * ABILITY_B_FORCE);
            movement.Disable(ability.ActiveTime);
            Invoke("ZeroVelocity", ability.ActiveTime);
        }
        else if (Input.GetAxis(axisY + "_" + controller.controllerNumber) > threshold)
        {
            //shoot down
            player.AddForce(Vector3.down * ABILITY_B_FORCE);
            movement.Disable(ability.ActiveTime);
            Invoke("ZeroVelocity", ability.ActiveTime);
        }
        else if (Input.GetAxis(axisX + "_" + controller.controllerNumber) > threshold)
        {
            //shoot right
            player.useGravity = false;
            player.MovePosition(transform.position + Vector3.up * 0.1f);
            player.AddForce(Vector3.right * ABILITY_B_FORCE);
            movement.Disable(ability.ActiveTime);
            Invoke("ZeroVelocity", ability.ActiveTime);
        }
        else if (Input.GetAxis(axisX + "_" + controller.controllerNumber) < -threshold)
        {
            //shoot left
            player.useGravity = false;
            player.MovePosition(transform.position + Vector3.up * 0.1f);
            player.AddForce(Vector3.left * ABILITY_B_FORCE);
            movement.Disable(ability.ActiveTime);
            Invoke("ZeroVelocity", ability.ActiveTime);
        }
        // -----------------
        yield return new WaitForSeconds(ability.ActiveTime);
        player.useGravity = true;
        dashTrail.time = 0;
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);

        while (Mathf.Abs(Input.GetAxis(axisX + "_" + controller.controllerNumber)) > threshold
            || Mathf.Abs(Input.GetAxis(axisY + "_" + controller.controllerNumber)) > threshold)
        {
            yield return new WaitForFixedUpdate();
        }

        ability.AbilityStatus = Ability.Status.READY;
    }

    #endregion

    #region Powerups

    IEnumerator Ability_BlockDrop_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;

        ParticleSystem.EmissionModule em = particleSystems[ability.Name].GetComponent<ParticleSystem>().emission;
        em.enabled = true;

        yield return new WaitForSeconds(ability.ActiveTime);
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);

        em.enabled = false;
        currentPowerup = Powerup.None;

        ability.AbilityStatus = Ability.Status.READY;
    }

    IEnumerator Ability_GrenadeBlock_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;

        ParticleSystem.EmissionModule em = particleSystems[ability.Name].GetComponent<ParticleSystem>().emission;
        em.enabled = true;

        yield return new WaitForSeconds(ability.ActiveTime);
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);

        em.enabled = false;
        currentPowerup = Powerup.None;

        ability.AbilityStatus = Ability.Status.READY;
    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (currentPowerup == Powerup.None)
        {
            if (other.CompareTag("Powerup") && other.name == "GrenadePowerup(Clone)" &&
            abilities[(int)Powerup.GrenadeBlock].AbilityStatus == Ability.Status.READY)
            {
                currentPowerup = Powerup.GrenadeBlock;
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("Powerup") && other.name == "DropPowerup(Clone)" &&
                abilities[(int)Powerup.BlockDrop].AbilityStatus == Ability.Status.READY)
            {
                currentPowerup = Powerup.BlockDrop;
                other.gameObject.SetActive(false);
            }
        }
    }
}
