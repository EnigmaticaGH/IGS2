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
        if (CheckTags(c))
        {
            jumpControl.SendWallSensorReading(name[0]);
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
        return c.CompareTag("Untagged") || c.CompareTag("Ground");
    }
}
