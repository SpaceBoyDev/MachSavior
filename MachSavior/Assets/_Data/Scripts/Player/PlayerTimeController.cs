using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimeController : MonoBehaviour
{
    [Header("General")] 
    
    [SerializeField] private Camera cam;
    [SerializeField] private TimeControlSettings _timeSettings;
    
    private TimeObject timeObject; // Stores currently selected interactable object.

    private void Start()
    {
        _timeSettings.CurrentTimeCells = _timeSettings.GetMaxTimeCells;
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

        if (!Physics.Raycast(ray, out var hit, _timeSettings.GetMaxDistance))
        {
            timeObject = null;
            return;
        }
        
        var selection = hit.transform;
        timeObject = selection.gameObject.GetComponent<TimeObject>();
        
        if (timeObject == null)
            return;
        
        //Check if time object is at range so no effect applies.
        if (Vector3.Distance(timeObject.transform.position, transform.position) > _timeSettings.GetMaxDistance)
        {
            Debug.Log($"<color=red>{timeObject.gameObject.name} </color>is NOT at range.");
            timeObject.isAtRange = false;
        }
        else
        {
            Debug.Log($"<color=blue>{timeObject.gameObject.name} </color>is at range.");
            timeObject.isAtRange = true;
        }
        //TODO::Last frame time obj 
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

            if (timeObject.getIsStopped && _timeSettings.CurrentTimeCells > 0)
            {
                _timeSettings.CurrentTimeCells --;
                _timeSettings.GetOnTimeCellUsed.Raise();
                timeObject.UseTimeCell();
                //timeObject = null;
            }
            else if(!timeObject.getIsStopped && _timeSettings.CurrentTimeCells < _timeSettings.GetMaxTimeCells)
            {
                //Time cells
                _timeSettings.CurrentTimeCells ++;
                _timeSettings.GetOnTimeCellUsed.Raise();
                timeObject.TakeTimeCell();
            }
            else if (timeObject.getIsStopped && _timeSettings.CurrentTimeCells <= 0)
            {
                _timeSettings.GetOnTimeCellsEmpty.Raise();
            }
            
        }
    }
}
