using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineTrigger : MonoBehaviour
{
    private void OnDisable()
    {
        PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PlayerManager.Instance.CalculateAdditionalTurbineSpeed(transform.up, 10f);
            //PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(true);
            other.GetComponent<PlayerMovement>().ResetVerticalSpeed();
            other.GetComponent<Rigidbody>().AddForce(transform.up * 20f, ForceMode.VelocityChange);
        }
        else
        {
            PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
        }
    }

   //private void OnTriggerExit(Collider other)
   //{
   //    if (other.CompareTag("Player"))
   //    {
   //        PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
   //        StartCoroutine(ApplyFinalAirForce(other));
   //    }
   //}
   //
   //private IEnumerator ApplyFinalAirForce(Collider other)
   //{
   //    yield return new WaitForSeconds(0.01f);
   //    other.GetComponent<Rigidbody>().velocity = other.GetComponent<Rigidbody>().velocity
   //}
}
