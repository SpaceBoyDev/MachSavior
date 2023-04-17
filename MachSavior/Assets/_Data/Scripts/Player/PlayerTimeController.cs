using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeController : MonoBehaviour
{
    void Update()
    {
        if (PlayerInputManager.Instance.IsResumeTime())
        {
            if (TimeManager.Instance.isTimeStopped)
            {
                Debug.Log("Resume time.");
                TimeManager.Instance.ContinueTime();
            }
            else
            {
                Debug.Log("Stop time.");
                TimeManager.Instance.StopTime();
            }
        }
    }
}
