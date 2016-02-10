using UnityEngine;
using System.Collections;

public class DynamicCollider : MonoBehaviour
{
    public GameObject frictionCollider;
    public GameObject nonFrictionCollider;
    private bool disabled = false;

    public void MovementStateChange(string state)
    {
        if(state == "GROUND" && !disabled)
        {
            frictionCollider.SetActive(true);
            nonFrictionCollider.SetActive(false);
        }
        else if (!disabled)
        {
            frictionCollider.SetActive(false);
            nonFrictionCollider.SetActive(true);
        }
    }

    public void Disable()
    {
        disabled = true;
        frictionCollider.SetActive(false);
        nonFrictionCollider.SetActive(false);
    }
}
