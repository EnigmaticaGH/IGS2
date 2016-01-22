using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour
{
    private Movement movement;

    void Start()
    {
        movement = GetComponentInParent<Movement>();
        movement.SendGroundSensorReading(true);
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Untagged") && movement != null)
            movement.SendGroundSensorReading(true);
    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Untagged") && movement != null)
            movement.SendGroundSensorReading(false);
    }
    bool TagsToIgnore(string tag)
    {
        return tag == "Sensor"
            || tag == "Player";
    }
}
