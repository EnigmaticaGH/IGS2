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
        Ray ray = new Ray(transform.position + (Vector3.right * 0.9f) + Vector3.down, Vector3.left);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1.25f, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(ray, 1.3f);
        foreach(RaycastHit hit in hits)
        {
            hitsGround += hit.collider.CompareTag("Block") ? 1 : 0;
        }
        grounded = hitsGround > 0;
        movement.SendGroundSensorReading(grounded);
    }
}
