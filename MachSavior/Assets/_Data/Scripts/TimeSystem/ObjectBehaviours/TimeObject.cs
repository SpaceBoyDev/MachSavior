using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base time object class.
/// </summary>

public abstract class TimeObject : MonoBehaviour, ITimeInteractable
{
    [Header("Time Settings")]
    [SerializeField,Tooltip("Makes so the object is affected by time so its controllable by the player.")]
    protected bool timeAffected = true;
    [SerializeField,Tooltip("Checks the if the time state of the object is stopped or not.")] 
    protected bool isStopped;
    [SerializeField,Tooltip("Makes so the object has no movement at all when its stopped in time.")] 
    protected bool freezeInTime;
    
    [SerializeField,Tooltip("Selection state of the item.")] 
    protected bool isSelected = false;

    private Outline outline;
    //--------------------------------//
    private void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = isStopped ? Color.blue : Color.black;
        outline.OutlineWidth = 8f;
    }

    private void Start()
    {
    }

    public bool GetIsStopped() { return isStopped; }

    public bool GetIsSelected() { return isSelected; }
    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
            outline.OutlineColor = Color.white;
        //outline.OutlineWidth = 8f;
    }

    public void ChangeTimeState()
    {
        if (!timeAffected)
            return;
        // Flip the time state.
        isStopped = !isStopped;

        if (isStopped)
        {
            StopTime();
            outline.OutlineColor = Color.blue;
            //outline.OutlineWidth = 8f;
            //Debug.Log($"<color=green>Resume time</color> in object: <color=yellow>{target.name} </color>");
        }
        else
        {
            ResumeTime();
            outline.OutlineColor = Color.black;
            //outline.OutlineWidth = 8f;
            //Debug.Log($"<color=red>Stop time</color> in object: <color=yellow> {target.name} </color>");
        }
    }
    public abstract void ResumeTime();
    public abstract void StopTime();
}
