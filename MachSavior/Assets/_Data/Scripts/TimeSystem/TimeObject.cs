using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float slowtime;

    public bool isTimeAffected = true;
    [SerializeField] private bool isFreeze = false;
    private bool isStopped;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeManager.Instance.timeAffectedObjects.Add(this);
    }

    private void Update()
    {
        if(isTimeAffected && TimeManager.Instance.isTimeStopped && !isStopped)
        {
            //Record last velocity values before it slows
            recordedVelocity = rb.velocity.normalized;
            recordedMagnitude = rb.velocity.magnitude;

            // If  the object doesnt move at all when time is slowed, make it kinematic.
            if (isFreeze)
                rb.isKinematic = true; 

            //Stop movement
            slowtime = 0.05f;
            isStopped = true;
        }
        rb.velocity *= slowtime;
    }

    public void ContinueObjectTime()
    {
        slowtime = 1f;

        if(isFreeze)
            rb.isKinematic = false;

        isStopped = false;

        rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the velocity that had before it got stopped.
    }
}
