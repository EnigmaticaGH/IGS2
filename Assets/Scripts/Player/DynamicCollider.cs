using UnityEngine;
using System.Collections;

public class DynamicCollider : MonoBehaviour
{
    public GameObject frictionCollider;
    public GameObject nonFrictionCollider;
    private bool disabled = false;
    private bool forcingMode = false;

    public void MovementStateChange(string state)
    {
        if(state == "GROUND" && !disabled && !forcingMode)
        {
            frictionCollider.SetActive(true);
            nonFrictionCollider.SetActive(false);
        }
        else if (!disabled && !forcingMode)
        {
            frictionCollider.SetActive(false);
            nonFrictionCollider.SetActive(true);
        }
    }

    public void ForceUseFrictionless(float time)
    {
        forcingMode = true;
        frictionCollider.SetActive(false);
        nonFrictionCollider.SetActive(true);
        Invoke("TurnOffForceMode", time);
    }

    void TurnOffForceMode()
    {
        forcingMode = false;
    }

    public void Disable()
    {
        disabled = true;
        frictionCollider.SetActive(false);
        nonFrictionCollider.SetActive(false);
    }
}
