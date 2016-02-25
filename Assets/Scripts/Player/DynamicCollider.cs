using UnityEngine;
using System.Collections;

public class DynamicCollider : MonoBehaviour
{
    public GameObject frictionCollider;
    public GameObject nonFrictionCollider;
    private bool disabled = false;
    private bool forcingMode = false;

    void Awake()
    {
        Movement.MovementStateEvent += MovementStateChange;
    }

    void OnDestroy()
    {
        Movement.MovementStateEvent -= MovementStateChange;
    }

    public void MovementStateChange(Movement.MovementState state, string n)
    {
        if (name != n) return;
        if (state == Movement.MovementState.GROUND && !disabled && !forcingMode)
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
