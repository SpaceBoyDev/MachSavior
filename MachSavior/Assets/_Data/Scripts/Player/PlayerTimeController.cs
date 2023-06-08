using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [Header("General")] 
    
    [SerializeField] private Camera cam;
    //[SerializeField] private bool isSelectModeActive = false;
    [SerializeField] private TimeControlSettings _timeControlSettings;

    [SerializeField] private GameEvent onTimeCellUsed;
    
    //private List<ITimeInteractable> selectedTimeObjects = new List<ITimeInteractable>();
    private ITimeInteractable timeObject; // Stores currently selected interactable object.

    private void Start()
    {
        _timeControlSettings.CurrentTimeCells = _timeControlSettings.GetMaxTimeCells;
    }

    private void Update()
    {
        //ToggleSelectMode();
        CheckTimeObject();
    }
    
    /// <summary>
    /// Raycast that checks if the aimed object is a time object.
    /// </summary>
    private void CheckTimeObject()
    {
        if (timeObject != null)
        {
            timeObject.OnHoverExit();
            timeObject = null;
        }
        
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, _timeControlSettings.GetMaxDistance)) 
            return;
        
        var selection = hit.transform;
        var timeSelection = selection.GetComponent<ITimeInteractable>();
        
        //Check if there is an item selected and the performs the selection behaviour
        timeSelection?.OnHoverEnter();

        timeObject = timeSelection;
        
        OnHoverInput();

    }
    
    /// <summary>
    /// Changes the current time state of an aimed object or from all the selected objects inside select mode.
    /// </summary>
    private void OnHoverInput()
    {
        if (PlayerInputManager.Instance.IsUseTimeCell())
        {
            if (_timeControlSettings.CurrentTimeCells <= 0 || timeObject.GetHasTimeCell()) 
                return;
            
            //Time cells
            _timeControlSettings.CurrentTimeCells --;
            //timeObject.hasTimeCell = true;
            onTimeCellUsed.Raise();
            
            //Single object time change.
            timeObject.UseTimeCell();
            timeObject = null;
        }
        
        if (PlayerInputManager.Instance.IsTakeTimeCell())
        {
            if (_timeControlSettings.CurrentTimeCells >= _timeControlSettings.GetMaxTimeCells || !timeObject.GetHasTimeCell()) 
                return;
            
            //Time cells
            _timeControlSettings.CurrentTimeCells ++;
            //timeObject.hasTimeCell = true;
            onTimeCellUsed.Raise();
            
            //Single object time change.
            timeObject.TakeTimeCell();
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
