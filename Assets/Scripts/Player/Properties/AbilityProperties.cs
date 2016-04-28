using UnityEngine;
using System.Collections.Generic;

public class Ability
{
    string name;
    float cooldownTime;
    float activeTime;
    public enum Status
    {
        READY,
        ACTIVE,
        COOLDOWN,
        NULL
    };
    public bool UseButton { get; private set; }

    public Ability(string abilityName, float abilityCooldownTime, float abilityActiveTime)
    {
        name = abilityName;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
    }

    public Ability(string abilityName, float abilityCooldownTime, float abilityActiveTime, bool useButton)
    {
        name = abilityName;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
        UseButton = useButton;
    }

    public Status AbilityStatus
    {
        get;
        set;
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }

    public float ActiveTime
    {
        get { return activeTime; }
    }

    public string Name
    {
        get { return name; }
    }
}

public static class AbilityRegistry
{
    private static Dictionary<string, Dictionary<string, Ability>> abilityRegistry
        = new Dictionary<string, Dictionary<string, Ability>>();

    public static void RegisterAbility(string playerName, Ability ability)
    {
        if (!abilityRegistry.ContainsKey(playerName))
        {
            abilityRegistry.Add(playerName, new Dictionary<string, Ability>());
            abilityRegistry[playerName].Add(ability.Name, ability);
        }
        else if (!abilityRegistry[playerName].ContainsKey(ability.Name))
        {
            abilityRegistry[playerName].Add(ability.Name, ability);
        }
        else
        {
            Debug.LogError("Error: Abilities on the same player must have unique names!");
        }
    }

    public static Ability.Status AbilityStatus(string playerName, string abilityName)
    {
        if (abilityRegistry.ContainsKey(playerName))
        {
            if (abilityRegistry[playerName].ContainsKey(abilityName))
            {
                return abilityRegistry[playerName][abilityName].AbilityStatus;
            }
            else
            {
                Debug.LogError("Error: Ability " + abilityName + " does not exist on " + playerName);
                return Ability.Status.NULL;
            }
        }
        else
        {
            Debug.LogError("Error: " + playerName + " does not have any abilities.");
            return Ability.Status.NULL;
        }
    }

    public static void Reset()
    {
        abilityRegistry.Clear();
    }
}
