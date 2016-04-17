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
            new Ability("BlockSmash", 0.13f, 0.13f, 0.2f,
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
        currentPowerup = Powerup.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.State == Movement.MovementState.GROUND)
            usedAirDash = false;
        if (movement.State == Movement.MovementState.DISABLED)
            return;

        float x = Input.GetAxis(abilities[0].Axis[0] + "_" + controller.controllerNumber);
        float y = -Input.GetAxis(abilities[0].Axis[1] + "_" + controller.controllerNumber);
        Vector3 rThumbstick = new Vector3(x, y);
        if (Input.GetMouseButtonDown(0))
        {
            if (!(GetComponent<GrabBlock>().IsInvincible || GetComponent<GrabBlock>().CarryingBlock) && abilities[0].AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + abilities[0].Name + "_Activate", abilities[0]);
            }
        }
        if (rThumbstick.magnitude != 0)
        {
            if (thumbstickInUse == false)
            {
                if (!GetComponent<GrabBlock>().CarryingBlock && abilities[0].AbilityStatus == Ability.Status.READY)
                {
                    StartCoroutine("Ability_" + abilities[0].Name + "_Activate", abilities[0]);
                }
                thumbstickInUse = true;
            }
        }
        else if (rThumbstick.magnitude == 0)
        {
            thumbstickInUse = false;
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
        float x = Input.GetAxis(ability.Axis[0] + "_" + controller.controllerNumber);
        float y = -Input.GetAxis(ability.Axis[1] + "_" + controller.controllerNumber);
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
        if (Mathf.Abs(x) > ability.AxisThreshold / 2)
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
