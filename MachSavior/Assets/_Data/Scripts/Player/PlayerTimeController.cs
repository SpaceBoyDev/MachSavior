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
    private TimeObject lastTimeObject;

    private void Start()
    {
        _timeSettings.CurrentTimeCells = _timeSettings.GetMaxTimeCells;
    }

    private void Update()
    {
        //ToggleSelectMode();
        CheckTimeObject();
        OnHoverInput();
    }

    /// <summary>
    /// Raycast that checks if the aimed object is a time object.
    /// </summary>
    private void CheckTimeObject()
    {
        var ray = new Ray(cam.transform.position, cam.transform.forward);

        if (!Physics.Raycast(ray, out var hit, _timeSettings.GetMaxDistance))
        {
            if (lastTimeObject != null)
            {
                lastTimeObject.OnHoverExit();
                lastTimeObject = null;
            }
            if (timeObject != null)
            {
                timeObject.OnHoverExit();
                timeObject = null;
            }
            return;
        }
        
        var selection = hit.transform;
        timeObject = selection.GetComponent<TimeObject>();

        if (timeObject == lastTimeObject)
        {
            
            return;
        }
        
        timeObject.OnHoverEnter();

        lastTimeObject = timeObject;
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
