using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineTrigger : MonoBehaviour
{
    //[SerializeField] private AnimatedTimeObject _animatedTimeObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PlayerManager.Instance.CalculateAdditionalTurbineSpeed(transform.up, 10f);
            //PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(true);
            other.GetComponent<PlayerMovement>().ResetVerticalSpeed();
            other.GetComponent<Rigidbody>().AddForce(transform.up * 20f, ForceMode.VelocityChange);
            print("Apply air force");
        }
        else
        {
            PlayerManager.Instance.SetIsGravityAdditionalTurbineSpeedApplied(false);
        }

    }
}
