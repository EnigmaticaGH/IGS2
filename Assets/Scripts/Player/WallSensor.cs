using UnityEngine;
using System.Collections;

public class WallSensor : MonoBehaviour
{
    private JumpControl jumpControl;
    private Movement movement;

    void Start()
    {
        jumpControl = GetComponentInParent<JumpControl>();
        movement = GetComponentInParent<Movement>();
        jumpControl.SendWallSensorReading(' ');
    }
    void OnTriggerEnter(Collider c)
    {
        if (CheckTags(c))
        {
            jumpControl.SendWallSensorReading(name[0]);
        }

        if (c.gameObject.name == "Companion")
        {
            ;
        }
       
    }
    void OnTriggerExit(Collider c)
    {
        if (CheckTags(c))
        {
            jumpControl.SendWallSensorReading(' ');
        }
    }
    bool CheckTags(Collider c)
    {
        return c.CompareTag("Untagged") || c.CompareTag("Ground") || c.CompareTag("Trap");
    }
}
