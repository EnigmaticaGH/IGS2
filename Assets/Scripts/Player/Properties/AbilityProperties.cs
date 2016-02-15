using UnityEngine;
using System.Collections;

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
        COOLDOWN
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
