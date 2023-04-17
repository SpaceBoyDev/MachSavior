using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    private Vector3 recordedVelocity;
    private float recordeMagnitude;

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
            recordeMagnitude = rb.velocity.magnitude;

            //Stop movement
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            isStopped = true;
        }
    }

    public void ContinueObjectTime()
    {
        rb.isKinematic = false;
        isStopped = false;
        rb.velocity = recordedVelocity * recordeMagnitude; //Adds back the velocity that had before it got stopped.
    }
}
