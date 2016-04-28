using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{
    public float grenadeThrowForce;
    public GameObject grenadePrefab;

    private Dictionary<string, GameObject> particleSystems;
    //private GameObject sprite;
    private ControllerNumber controller;
    private Rigidbody player;
    private Movement movement;
    private ParticleSystem dashTrail;
    private const float ABILITY_B_FORCE = 600;
    //Assign new abilities here
    private Ability[] abilities;
    private bool usedAirDash;
    private bool thumbstickInUse;
    
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
        dashTrail = transform.FindChild("Dash").GetComponent<ParticleSystem>();
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        controller = GetComponent<ControllerNumber>();
        //sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
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
            new Ability("BlockSmash", 0.13f, 0.13f),
            new Ability(Powerup.BlockDrop.ToString(), 0, 2.5f, true),
            new Ability(Powerup.GrenadeBlock.ToString(), 0, 7.5f)
        };
        foreach (Ability ability in abilities)
        {
            //Initially set all abilities to READY
            ability.AbilityStatus = Ability.Status.READY;
            //Add abilities to a registry of abilities
            AbilityRegistry.RegisterAbility(name, ability);
        }
        usedAirDash = false;
        currentPowerup = Powerup.None;
    }

    // Update is called once per frame
    void Update()
    {
        bool DashPressed = 
            Input.GetButtonUp(Controls.DashControls[0] + controller.controllerNumber) || 
            Input.GetButtonUp(Controls.DashControls[1] + controller.controllerNumber);
        if (movement.State == Movement.MovementState.GROUND)
            usedAirDash = false;
        if (movement.State == Movement.MovementState.DISABLED)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!(GetComponent<GrabBlock>().IsThrowing || GetComponent<GrabBlock>().CarryingBlock) && abilities[0].AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + abilities[0].Name + "_Activate", abilities[0]);
            }
        }

        if (DashPressed && abilities[0].AbilityStatus == Ability.Status.READY)
        {
            StartCoroutine("Ability_" + abilities[0].Name + "_Activate", abilities[0]);
        }

        if (currentPowerup != Powerup.None)
        {
            if (abilities[(int)currentPowerup].AbilityStatus == Ability.Status.READY)
            {
                if (abilities[(int)currentPowerup].UseButton && Input.GetButtonUp("B_" + controller.controllerNumber))
                {
                    StartCoroutine("Ability_" + currentPowerup.ToString() + "_Activate", abilities[(int)currentPowerup]);
                }
                else if (!abilities[(int)currentPowerup].UseButton)
                {
                    StartCoroutine("Ability_" + currentPowerup.ToString() + "_Activate", abilities[(int)currentPowerup]);
                }
            }
        }
    }

    void ZeroVelocity()
    {
        player.velocity = Vector3.zero;
    }

    public void RemovePowerup()
    {
        currentPowerup = Powerup.None;
        ParticleSystem.EmissionModule em = particleSystems["BlockDrop"].GetComponent<ParticleSystem>().emission;
        em.enabled = false;
        em = particleSystems["GrenadeBlock"].GetComponent<ParticleSystem>().emission;
        em.enabled = false;
    }

    #region Ability 1

    IEnumerator Ability_BlockSmash_Activate(Ability ability)
    {
        float x = Input.GetAxis("L_XAxis_" + controller.controllerNumber);
        float y = -Input.GetAxis("L_YAxis_" + controller.controllerNumber);
        Vector3 forceAngle = new Vector3(x, y);
        Vector3 m = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        Vector3 mouse = new Vector3(m.x, m.y, 0).normalized;
        if (forceAngle.magnitude == 0)
        {
            forceAngle = mouse;
        }

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
        dashTrail.Play();
        Vector3 force = new Vector3(ABILITY_B_FORCE * forceAngle.normalized.x, ABILITY_B_FORCE * forceAngle.normalized.y);
        player.useGravity = false;
        if (y < 0.2f)
        {
            player.MovePosition(transform.position + Vector3.up * 0.2f);
        }
        player.AddForce(force);
        movement.Disable(ability.ActiveTime, false);
        Invoke("ZeroVelocity", ability.ActiveTime);
        
        yield return new WaitForSeconds(ability.ActiveTime);

        player.useGravity = true;

        ability.AbilityStatus = Ability.Status.COOLDOWN;

        yield return new WaitForSeconds(ability.CooldownTime);

        ability.AbilityStatus = Ability.Status.READY;
    }

    #endregion

    #region Powerups

    IEnumerator Ability_BlockDrop_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;

        

        yield return new WaitForSeconds(ability.ActiveTime);
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);

        ParticleSystem.EmissionModule em = particleSystems[ability.Name].GetComponent<ParticleSystem>().emission;
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
                ParticleSystem.EmissionModule em = particleSystems["BlockDrop"].GetComponent<ParticleSystem>().emission;
                em.enabled = true;
                other.gameObject.SetActive(false);
            }
        }
    }
}
