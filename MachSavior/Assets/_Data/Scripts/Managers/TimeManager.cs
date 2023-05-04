using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public List<TimeObject> timeAffectedObjects = new List<TimeObject>();

    public bool isTimeStopped = true;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        isTimeStopped = true;
    }

    public void ContinueTime()
    {
        isTimeStopped = false;
        foreach(ITimeAffected obj in timeAffectedObjects)
        {
            obj.ResumeTime();
        }
    }

    public void StopTime()
    {
        isTimeStopped = true;
    }
}
