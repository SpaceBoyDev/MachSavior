using System;
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

    private Renderer objRenderer;
    //--------------------------------//
    private void Start()
    {
        objRenderer = this.GetComponent<Renderer>();
    }

    public bool GetIsStopped() { return isStopped; }

    public bool GetIsSelected() { return isSelected; }
    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
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
        }
        else
        {
            ResumeTime();
        }
    }
    public abstract void ResumeTime();
    public abstract void StopTime();
}
