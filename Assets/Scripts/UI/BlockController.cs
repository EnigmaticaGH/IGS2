using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.green);
            Debug.Log("Touching");
        }


    }

    void OnColliderEnter(Collider col)
    {
        if (col.tag == "Block")
        {
            Debug.Log("omg no way");
        }
    }
}
