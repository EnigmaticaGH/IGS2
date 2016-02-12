using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour
{
    private Movement movement;
    private bool grounded;
    private int hitsGround;

    void Start()
    {
        movement = GetComponentInParent<Movement>();
        hitsGround = 0;
    }
    void FixedUpdate()
    {
        hitsGround = 0;
        Ray ray = new Ray(transform.position + Vector3.left * 0.5f + Vector3.down, Vector3.right);

        Debug.DrawLine(transform.position + Vector3.left * 0.5f + Vector3.down
            , transform.position + Vector3.left * 0.5f + Vector3.down + Vector3.right
            , Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(ray, 1);
        if (hits != null)
        {
            foreach(RaycastHit hit in hits)
            {
                hitsGround += hit.collider.CompareTag("Block") ? 1 : 0;
            }
            grounded = hitsGround > 0;
        }
        else
        {
            grounded = false;
        }
        movement.SendGroundSensorReading(grounded);
    }
}
