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
    [SerializeField] private LayerMask mask;
    [Header("Events")]
    [SerializeField] private GameEvent onTimeCellUsed;
    [SerializeField] private GameEvent onEmptyTimeCells;
    //private List<ITimeInteractable> selectedTimeObjects = new List<ITimeInteractable>();
    private TimeObject timeObject; // Stores currently selected interactable object.

    private void Start()
    {
        _timeControlSettings.CurrentTimeCells = _timeControlSettings.GetMaxTimeCells;
    }

    private void Update()
    {
        //ToggleSelectMode();
        CheckTimeObject();

        /*var ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _timeControlSettings.GetMaxDistance, mask))
        {
            timeObject = hit.transform.gameObject.GetComponent<TimeObject>();
            
            if (Vector3.Distance(timeObject.transform.position, transform.position) > _timeControlSettings.GetMaxDistance)
            {
                Debug.Log($"<color=red>{timeObject.gameObject.name} </color>is NOT at range.");
                timeObject.isAtRange = false;
            }
            else
            {
                Debug.Log($"<color=blue>{timeObject.gameObject.name} </color>is at range.");
                timeObject.isAtRange = true;
            }
        }
        else
        {
            timeObject = null;
        }*/
        
        OnHoverInput();
    }

    /// <summary>
    /// Raycast that checks if the aimed object is a time object.
    /// </summary>
    private void CheckTimeObject()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, _timeControlSettings.GetMaxDistance))
        {
            timeObject = null;
            return;
        }
        
        var selection = hit.transform;
        timeObject = selection.gameObject.GetComponent<TimeObject>();
        
        if (timeObject == null)
            return;
        
        //Check if time object is at range so no effect applies.
        if (Vector3.Distance(timeObject.transform.position, transform.position) > _timeControlSettings.GetMaxDistance)
        {
            Debug.Log($"<color=red>{timeObject.gameObject.name} </color>is NOT at range.");
            timeObject.isAtRange = false;
        }
        else
        {
            Debug.Log($"<color=blue>{timeObject.gameObject.name} </color>is at range.");
            timeObject.isAtRange = true;
        }
    }
    
    /// <summary>
    /// Changes the current time state of an aimed object or from all the selected objects inside select mode.
    /// </summary>
    private void OnHoverInput()
    {
        if (timeObject == null)
            return;
        
        if (PlayerInputManager.Instance.ChangeTimeState())
        {
            

            if (timeObject.getIsStopped && _timeControlSettings.CurrentTimeCells > 0)
            {
                _timeControlSettings.CurrentTimeCells --;
                onTimeCellUsed.Raise();
                timeObject.UseTimeCell();
                //timeObject = null;
            }
            else if(!timeObject.getIsStopped && _timeControlSettings.CurrentTimeCells < _timeControlSettings.GetMaxTimeCells)
            {
                //Time cells
                _timeControlSettings.CurrentTimeCells ++;
                onTimeCellUsed.Raise();
                timeObject.TakeTimeCell();
            }
            else if (timeObject.getIsStopped && _timeControlSettings.CurrentTimeCells <= 0)
            {
                onEmptyTimeCells.Raise();
            }
            
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
