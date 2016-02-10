using UnityEngine;
using System.Collections;

public class BlockDisableGravityOnLand : MonoBehaviour {

	void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Block") || c.collider.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        if (c.collider.CompareTag("Player") && GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            c.gameObject.GetComponent<DeathControl>().Hurt(1);
        }
    }
}
