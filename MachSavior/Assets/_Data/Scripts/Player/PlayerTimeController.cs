using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeController : MonoBehaviour
{
    [SerializeField] private float selectDistance;
    
    void Update()
    {
        SelectTimeObject();
        // if (PlayerInputManager.Instance.IsResumeTime())
        // {
        //     if (TimeManager.Instance.isTimeStopped)
        //     {
        //         Debug.Log("$<color=green>Resume time </color> in object: <color=yellow>");
        //         //TimeManager.Instance.ContinueTime();
        //     }
        //     else
        //     {
        //         Debug.Log("<color=red>Stop time.</color>");
        //         //TimeManager.Instance.StopTime();
        //     }
        // }
    }
    
    void SelectTimeObject()
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo);
        if (hit)
        {
            GameObject hitObj = hitInfo.collider.gameObject;
            //If we dont hit a time controlled object return.
            if (hitObj.GetComponent<ITimeInteractable>() == null)
                return;
            if (PlayerInputManager.Instance.IsResumeTime())
            {
                if (hitObj.GetComponent<ITimeInteractable>().GetIsStopped()) 
                {
                    Debug.Log($"$<color=green>Resume time</color> in object: <color=yellow>{hitObj.name} </color>");
                    hitObj.GetComponent<ITimeInteractable>().ResumeTime();
                }
                else
                {
                    Debug.Log($"<color=red>Stop time</color> in object: <color=yellow> {hitObj.name} </color>");
                    hitObj.GetComponent<ITimeInteractable>().StopTime();
                }
            }
        
        }
        
    }
}
