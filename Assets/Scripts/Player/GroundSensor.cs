using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour
{
    public delegate void SensorStatus(bool grounded);
    public static event SensorStatus SensorReading;

    void Start()
    {
        SensorReading(true);
    }
    void OnTriggerEnter(Collider c)
    {
        if (!TagsToIgnore(c.tag))
            SensorReading(true);
    }
    void OnTriggerExit(Collider c)
    {
        if (!TagsToIgnore(c.tag))
            SensorReading(false);
    }
    bool TagsToIgnore(string tag)
    {
        return tag == "Sensor"
            || tag == "Player";
    }
}
