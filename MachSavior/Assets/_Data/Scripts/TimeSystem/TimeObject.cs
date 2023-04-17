using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float slowtime;

    public bool isTimeAffected;
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
            recordedVelocity = rb.velocity.normalized;
            recordedMagnitude = rb.velocity.magnitude;

            //Stop movement
            slowtime = 0.1f;
            //rb.isKinematic = true;
            isStopped = true;
        }
        rb.velocity *= slowtime;
    }

    public void ContinueObjectTime()
    {
        slowtime = 1f;
        rb.isKinematic = false;
        isStopped = false;
        rb.velocity = recordedVelocity * recordedMagnitude; //Adds back the velocity that had before it got stopped.
    }
}
