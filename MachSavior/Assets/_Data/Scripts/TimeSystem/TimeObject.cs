using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeObject : MonoBehaviour, ITimeAffected
{
    private Vector3 recordedVelocity;
    private float recordedMagnitude;
    
    //This variable modifies the speed of the rb when the object is time affected.
    private float slowtime;

    public bool isTimeAffected = true;
    [SerializeField] private bool isFreezeInTime = false;
    [HideInInspector] public bool isStopped = true;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeManager.Instance.timeAffectedObjects.Add(this);
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

    public void StopTime()
    {
        isStopped = true;
    }

    public void ResumeTime()
    {
        slowtime = 1f;

        if(isFreezeInTime)
            rb.isKinematic = false;

        isStopped = false;

        rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the velocity that had before it got stopped.
    }
}
