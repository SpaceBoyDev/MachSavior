using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [Header("Time Control")] 
    //[SerializeField] private bool isSelectModeActive = false;
    [SerializeField] private float distance;
    
    //private List<ITimeInteractable> selectedTimeObjects = new List<ITimeInteractable>();
    private ITimeInteractable interactable;

    private void Update()
    {
        //ToggleSelectMode();
        ChangeTimeState();
    }
    
    /// <summary>
    /// Raycast that checks if the aimed object is Time Interactable.
    /// </summary>
    private void CheckTimeObject()
    {
        var hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hitInfo, distance);
        
        if (hit)
            interactable = hitInfo.collider.gameObject.GetComponent<ITimeInteractable>();
    }
    
    /// <summary>
    /// Changes the current time state of an aimed object or from all the selected objects inside select mode.
    /// </summary>
    private void ChangeTimeState()
    {
        if (!PlayerInputManager.Instance.IsChangeTimeState())
            return;
        //Raycast for checking objects.
        CheckTimeObject();
        
        if (interactable != null)
        {
            //Single object time change.
            interactable.ChangeTimeState();
            interactable = null;
        }
        // Change te time state in the selected objects inside selection mode.-> [SELECT MODE CURRENTLY UNUSED]
        /*if (isSelectModeActive && selectedTimeObjects != null) 
            {
                foreach (var selection in selectedTimeObjects)
                {
                    if (selection.GetIsSelected())
                    {
                        selection.ChangeTimeState();
                        //selection.SetIsSelected(false);
                    }
                }
            }*/
    }
    
    //-------------------------[SELECT MODE CURRENTLY UNUSED]--------------------------//
    /*private void ToggleSelectMode()
    {
        if (PlayerInputManager.Instance.EnterSelectMode())
        {
            //TODO:: Add effects for selection mode.
            Debug.Log($"<color=Blue>Selection mode activated.</color>");
            isSelectModeActive = true;
            SelectMode();
        }
        else if(PlayerInputManager.Instance.ExitSelectMode())
        {
            Debug.Log($"<color=Yellow>Selection mode exited.</color>");
            isSelectModeActive = false;
            
            foreach (var selection in selectedTimeObjects)
            {
                selection.SetIsSelected(false);
            }
            //Empty the list of objects when exiting select mode.
            selectedTimeObjects.Clear();
            interactable = null;
        }
    }
    /// <summary>
    /// Contains all behaviour related to the time object selection mode.
    /// </summary>
    private void SelectMode()
    {

        if (PlayerInputManager.Instance.IsSelectTimeObject())
        {
            CheckTimeObject();
            if (interactable == null)
                return;
            if (!interactable.GetIsSelected())
            {
                interactable.SetIsSelected(true);
                selectedTimeObjects.Add(interactable);
                interactable = null;
                Debug.Log($"<color=green>Object added to select list.</color>");
            }
            else
            {
                interactable.SetIsSelected(false);
                selectedTimeObjects.Remove(interactable);
                interactable = null;
                Debug.Log($"<color=cyan>Object removed from select list.</color>");
            }
        }   
    }*/
}
