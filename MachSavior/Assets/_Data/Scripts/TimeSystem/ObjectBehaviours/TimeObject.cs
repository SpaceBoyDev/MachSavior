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
    
    /*[SerializeField,Tooltip("Selection state of the item.")] 
    protected bool isSelected = false;*/

    [HideInInspector] public bool hasTimeCell = false;

    [Header("Effects")]
    private Material highlightMat;

    private Outline outline;

    [SerializeField] private Renderer highlightRenderer;

    [Header("Events")] 
    
    [SerializeField] private GameEvent onHoverEnter, onHoverExit;
    //--------------------------------//
    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.black;
        outline.OutlineWidth = 4f;
    }

    private void Start()
    {
        hasTimeCell = !isStopped;
        //renderer = GetComponentInChildren<Renderer>();
    }

    public bool GetIsStopped() { return isStopped; }
    public void OnHoverEnter()
    {
        onHoverEnter.Raise();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 8f;
        
        highlightRenderer.material.SetFloat("_EffectBlend", 1f);
    }

    public void OnHoverExit()
    {
        onHoverExit.Raise();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.black;
        outline.OutlineWidth = 4f;
        
        highlightRenderer.material.SetFloat("_EffectBlend", 0f);
    }

    public bool GetHasTimeCell()
    {
        return hasTimeCell;
    }

    public void UseTimeCell()
    {
        hasTimeCell = true; //Makes sure it uses only one time cell.
        // Flip the time state.
        isStopped = !isStopped;
        ResumeTime();
    }

    public void TakeTimeCell()
    {
        hasTimeCell = false;
        isStopped = !isStopped;
        StopTime();
    }

    public abstract void ResumeTime();
    public abstract void StopTime();
    
    //-------------------------[SELECT MODE CURRENTLY UNUSED]--------------------------//
    /*public bool GetIsSelected() { return isSelected; }
    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
            outline.OutlineColor = Color.white;
        else
            outline.OutlineColor = isStopped ? Color.blue : Color.black;
        //outline.OutlineWidth = 8f;
    }*/
}
