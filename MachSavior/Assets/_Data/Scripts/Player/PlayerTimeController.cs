using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [Header("General")] 
    //[SerializeField] private bool isSelectModeActive = false;
    [SerializeField] private TimeControlSettings _timeControlSettings;

    [SerializeField] private GameEvent onTimeCellUsed;
    
    //private List<ITimeInteractable> selectedTimeObjects = new List<ITimeInteractable>();
    private TimeObject timeObject; // Stores currently selected interactable object.

    private void Start()
    {
        _timeControlSettings.currentTimeCells = _timeControlSettings.GetMaxTimeCells;
    }

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
        var hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hitInfo, _timeControlSettings.GetMaxDistance);
        
        if (hit)
            timeObject = hitInfo.collider.gameObject.GetComponent<TimeObject>();
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

        if (_timeControlSettings.currentTimeCells > 0 && !timeObject.hasTimeCell)
        {
            UseTimeCell();
            //Single object time change.
            timeObject.ChangeTimeState();
            timeObject = null;
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

    private void UseTimeCell()
    {
        _timeControlSettings.currentTimeCells --;
        timeObject.hasTimeCell = true;
        onTimeCellUsed.Raise();
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
