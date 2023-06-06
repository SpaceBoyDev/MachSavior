using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PhysicsTimeObject : TimeObject
{
    private Vector3 recordedVelocity;
    private float recordedMagnitude;
    
    //This variable modifies the speed of the rb when the object is time affected.
    [HideInInspector]
    public float slowtime = 1f;
    
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if(isStopped)
            StopTime();
    }

    private void Update()
    {
        rb.velocity *= slowtime;
    }

    public override void ResumeTime()
    {
        if (freezeInTime)
            rb.isKinematic = false;

        slowtime = 1f;

        //Adds back the velocity that had before it got stopped.
        rb.velocity = recordedVelocity * recordedMagnitude;
    }

    public override void StopTime()
    {
        //Record last velocity values before it slows
        recordedVelocity = rb.velocity.normalized;
        recordedMagnitude = rb.velocity.magnitude;

        // If  the object doesnt move at all when time is slowed, make it kinematic.
        if (freezeInTime)
            rb.isKinematic = true;

        //Stop movement
        slowtime = 0.05f;
        isStopped = true;
    }
}