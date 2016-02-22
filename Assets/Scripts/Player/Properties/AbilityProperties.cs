using UnityEngine;
using System.Collections.Generic;

public class Ability
{
    string name;
    string button;
    GameObject[] objects;
    float cooldownTime;
    public enum Status
    {
        READY,
        ACTIVE,
        COOLDOWN,
        NULL
    };
    Status abilityStatus;

    public Ability(string abilityName, string buttonName, float abilityCooldownTime)
    {
        name = abilityName;
        button = buttonName;
        cooldownTime = abilityCooldownTime;
    }

    public Ability(string abilityName, string buttonName, float abilityCooldownTime, GameObject[] abilityObjects)
    {
        name = abilityName;
        button = buttonName;
        cooldownTime = abilityCooldownTime;
        objects = abilityObjects;
    }

    public GameObject[] Objects
    {
        get { return objects; }
    }

    public Status AbilityStatus
    {
        get;
        set;
    }

    public string Button
    {
        get { return button; }
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }

    public string Name
    {
        get { return name; }
    }
}

public static class AbilityRegistry
{
    private static Dictionary<string, Dictionary<string, Ability>> abilityRegistry = new Dictionary<string, Dictionary<string, Ability>>();

    public static void RegisterAbility(string playerName, Ability ability)
    {
        if (!abilityRegistry.ContainsKey(playerName))
        {
            Debug.Log("Creating ability registry entry for " + playerName);
            abilityRegistry.Add(playerName, new Dictionary<string, Ability>());
            Debug.Log("Adding ability " + ability.Name + " for " + playerName);
            abilityRegistry[playerName].Add(ability.Name, ability);
        }
        else if (!abilityRegistry[playerName].ContainsKey(ability.Name))
        {
            Debug.Log("Adding ability " + ability.Name + " for " + playerName);
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
}