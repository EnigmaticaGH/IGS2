using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour
{
    private ControllerNumber controller;
    private Rigidbody player;
    private Movement movement;
    private const float ABILITY_B_FORCE = 600;
    //Assign new abilities here
    private Ability[] abilities;
    private bool usedAirDash;

    // Use this for initialization
    void Awake()
    {
        player = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        controller = GetComponent<ControllerNumber>();
        abilities = new Ability[]
        {
            new Ability("BlockSmash", 0.5f, 0.125f, 0.5f, 
                new string[]
                {
                    "R_XAxis",
                    "R_YAxis"
                }),
            new Ability("BlockDrop", "B", 5f, 0.5f)
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
            }
            else if (ability.Axis != null)
            {
                foreach(string axisName in ability.Axis)
                {
                    if(Mathf.Abs(Input.GetAxis(axisName + "_" + controller.controllerNumber)) > ability.AxisThreshold && ability.AbilityStatus == Ability.Status.READY)
                    {
                        StartCoroutine("Ability_" + ability.Name + "_Activate", ability);
                    }
                }
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

    #region Ability 2

    IEnumerator Ability_BlockDrop_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;
        yield return new WaitForSeconds(ability.ActiveTime);
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);
        ability.AbilityStatus = Ability.Status.READY;
    }

    #endregion
}
