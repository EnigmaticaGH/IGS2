using UnityEngine;
using System.Collections.Generic;

public class Ability
{
    string name;
    string button = "";
    string[] axis;
    float axisThreshold;
    GameObject[] objects;
    float cooldownTime;
    float activeTime;
    public enum Status
    {
        READY,
        ACTIVE,
        COOLDOWN,
        NULL
    };
    Status abilityStatus;

    public Ability(string abilityName, string buttonName, float abilityCooldownTime, float abilityActiveTime)
    {
        name = abilityName;
        button = buttonName;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
    }

    public Ability(string abilityName, string buttonName, float abilityCooldownTime, float abilityActiveTime, GameObject[] abilityObjects)
    {
        name = abilityName;
        button = buttonName;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
        objects = abilityObjects;
    }

    public Ability(string abilityName, float abilityCooldownTime, float abilityActiveTime, float abilityAxisThreshold, string[] axisNames)
    {
        name = abilityName;
        axis = axisNames;
        axisThreshold = abilityAxisThreshold;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
    }

    public Ability(string abilityName, float abilityCooldownTime, float abilityActiveTime, float abilityAxisThreshold, string[] axisNames, GameObject[] abilityObjects)
    {
        name = abilityName;
        axis = axisNames;
        axisThreshold = abilityAxisThreshold;
        cooldownTime = abilityCooldownTime;
        activeTime = abilityActiveTime;
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

    public float AxisThreshold
    {
        get { return axisThreshold; }
    }

    public string[] Axis
    {
        get { return axis; }
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
