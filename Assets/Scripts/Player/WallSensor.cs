using UnityEngine;
using System.Collections;

public class WallSensor : MonoBehaviour
{
    private JumpControl jumpControl;

    void Start()
    {
        jumpControl = GetComponentInParent<JumpControl>();
        jumpControl.SendWallSensorReading(' ');
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Untagged") && jumpControl != null)
            jumpControl.SendWallSensorReading(name[0]);

        if (c.gameObject.name == "Companion")
        {
            ;
        }
       
    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Untagged") && jumpControl != null)
            jumpControl.SendWallSensorReading(' ');
    }
    bool TagsToIgnore(string tag)
    {
        return tag == "Sensor"
            || tag == "Player";
    }
}
