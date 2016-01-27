using UnityEngine;
using System.Collections;

public class DynamicCollider : MonoBehaviour
{
    public GameObject frictionCollider;
    public GameObject nonFrictionCollider;

    public void MovementStateChange(string state)
    {
        if(state == "GROUND")
        {
            frictionCollider.SetActive(true);
            nonFrictionCollider.SetActive(false);
        }
        else
        {
            frictionCollider.SetActive(false);
            nonFrictionCollider.SetActive(true);
        }
    }
}
