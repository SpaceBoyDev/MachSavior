using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeObject : MonoBehaviour, ITimeInteractable
{
    private Vector3 recordedVelocity;
    private float recordedMagnitude;
    
    //This variable modifies the speed of the rb when the object is time affected.
    private float slowtime = 1f;

    [Tooltip("Makes so the object is affected by time so its controllable by the player.")]
    public bool isTimeAffected = true;
    [SerializeField, Tooltip("Changes the starting time state of the object.")] 
    private bool isStopped;
    [SerializeField, Tooltip("Makes so the object completely stops in time when stopped.")] 
    private bool isFreezeInTime = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isTimeAffected && isStopped)
        {
            //Record last velocity values before it slows
            recordedVelocity = rb.velocity.normalized;
            recordedMagnitude = rb.velocity.magnitude;

            // If  the object doesnt move at all when time is slowed, make it kinematic.
            if (isFreezeInTime)
                rb.isKinematic = true; 

            //Stop movement
            slowtime = 0.05f;
            isStopped = true;
        }
        rb.velocity *= slowtime;
    }
    
    public bool GetIsStopped()
    {
        return isStopped;
    }

    public void ChangeTimeState()
    {
        isStopped = !isStopped;

        if (isStopped)
            return;

        slowtime = 1f;
        
        if(isFreezeInTime)
            rb.isKinematic = false;
        
        rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the velocity that had before it got stopped.
    }
}
