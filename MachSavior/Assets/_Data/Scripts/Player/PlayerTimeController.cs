using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [Header("Time Control")] 
    [SerializeField] private bool isSelectModeActive = false;
    [SerializeField] private float distance;
    [SerializeField] private List<ITimeInteractable> selectedTimeObjects;

    private void Update()
    {
        if (PlayerInputManager.Instance.IsSelectMode())
        {
            ToggleSelectMode();
        }
        CheckTimeObject();
    }
    
    private void CheckTimeObject()
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance);
        
        if (!hit)
            return;
        
        var interactable = hitInfo.collider.gameObject.GetComponent<ITimeInteractable>();

        //If we dont hit a time controlled object return.
        if (interactable == null)
            return;
        
        if (PlayerInputManager.Instance.IsChangeTimeState())
        {
            if (interactable.GetIsStopped()) 
            {
                //Debug.Log($"<color=green>Resume time</color> in object: <color=yellow>{hitObj.name} </color>");
                interactable.ChangeTimeState();
            }
            else
            {
                //Debug.Log($"<color=red>Stop time</color> in object: <color=yellow> {hitObj.name} </color>");
                interactable.ChangeTimeState();
            }
        }

        if (isSelectModeActive)
        {
            if (PlayerInputManager.Instance.IsSelectTimeObject())
            {
                if (!interactable.GetIsSelected())
                {
                    interactable.SetIsSelected(true);
                    Debug.Log(interactable);
                    selectedTimeObjects.Add(interactable);
                    Debug.Log($"<color=green>Added</color> object <color=yellow>{hitInfo.collider.gameObject.name}</color>");
                }
                else
                {
                    interactable.SetIsSelected(false);
                    //selectedTimeObjects.Remove(interactable);
                    Debug.Log($"<color=red>Removed</color> object <color=yellow>{hitInfo.collider.gameObject.name}</color>");
                }
            }
        }
    }

    private void ToggleSelectMode()
    {
        isSelectModeActive = !isSelectModeActive;
        if (isSelectModeActive)
        {
            //TODO:: Add effects for selection mode.
            Debug.Log($"<color=Blue>Selection mode activated.</color>");
        }
        else
        {
            Debug.Log($"<color=Yellow>Selection mode exited.</color>");
        }
    }
}
