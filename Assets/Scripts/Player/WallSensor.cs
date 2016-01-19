using UnityEngine;
using System.Collections;

public class WallSensor : MonoBehaviour
{
    public delegate void SensorStatus(char wall);
    public static event SensorStatus SensorReading;

    void Start()
    {
        SensorReading(' ');
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Untagged") && SensorReading != null)
            SensorReading(name[0]);
    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Untagged") && SensorReading != null)
            SensorReading(' ');
    }
    bool TagsToIgnore(string tag)
    {
        return tag == "Sensor"
            || tag == "Player";
    }
}
