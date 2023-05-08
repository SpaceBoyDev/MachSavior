using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [SerializeField] private float distance;
    
    void Update()
    {
        SelectTimeObject();
    }
    
    void SelectTimeObject()
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance);
        if (hit)
        {
            GameObject hitObj = hitInfo.collider.gameObject;
            //If we dont hit a time controlled object return.
            if (hitObj.GetComponent<ITimeInteractable>() == null)
                return;
            if (PlayerInputManager.Instance.IsChangeTimeState())
            {
                if (hitObj.GetComponent<ITimeInteractable>().GetIsStopped()) 
                {
                    Debug.Log($"$<color=green>Resume time</color> in object: <color=yellow>{hitObj.name} </color>");
                    hitObj.GetComponent<ITimeInteractable>().ChangeTimeState();
                }
                else
                {
                    Debug.Log($"<color=red>Stop time</color> in object: <color=yellow> {hitObj.name} </color>");
                    hitObj.GetComponent<ITimeInteractable>().ChangeTimeState();
                }
            }
        
        }
        
    }
}
