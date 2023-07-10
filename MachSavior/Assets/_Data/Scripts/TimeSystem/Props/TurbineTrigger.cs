using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineTrigger : MonoBehaviour
{
    [SerializeField] private float valueToResetVerticalSpeed;
    private void OnDisable()
    {
        PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ResetVerticalSpeed(valueToResetVerticalSpeed);
            other.GetComponent<Rigidbody>().AddForce(transform.up * 20f, ForceMode.VelocityChange);
        }
        else
        {
            PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
        }
    }
}
