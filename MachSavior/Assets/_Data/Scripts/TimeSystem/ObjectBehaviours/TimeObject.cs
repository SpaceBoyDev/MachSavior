using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base time object class.
/// </summary>
public abstract class TimeObject : MonoBehaviour
{
    [Header("General")] 
    
    [SerializeField] 
    private TimeControlSettings _timeSettings;
    
    [SerializeField,Tooltip("Makes so the object has no movement at all when its stopped in time.")] 
    protected bool freezeInTime;

    [Tooltip("Checks the if the time state of the object is stopped or not.")]
    public bool isStopped = true;
    public bool getIsStopped => isStopped;

    [Tooltip("Checks if the time object is at manipulation range.")]
    public bool isAtRange = false;
    
    private Outline outline;
    private Renderer renderer;
    
    // *********/ ABSTRACT METHODS /********* //
    
    public abstract void ResumeTime();
    public abstract void StopTime();

    // *********/ UNITY METHODS /********* //
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        renderer = GetComponent<Renderer>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 0f;
        
        renderer.material.SetColor("_FresnelColor", _timeSettings.GetDefaultFresnelColor);
        renderer.material.SetColor("_InteriorColor", _timeSettings.GetDefaultInteriorColor);
    }

    public void OnHoverEnter()
    {
        _timeSettings.GetOnHoverEnter.Raise();

        StartCoroutine(OutlineEffectLerp(6f, _timeSettings.GetChangeTimeDelay));

        if (isStopped)
        {
            outline.OutlineColor = _timeSettings.GetDefaultOutlineColor;
            StartCoroutine(HighlightEffectLerp(1f,_timeSettings.GetChangeTimeDelay));
        }
        else
        {
            outline.OutlineColor = _timeSettings.GetPoweredOutlineColor;
        }
    }

    public void OnHoverExit()
    {
        _timeSettings.GetOnHoverExit.Raise();
        
        StartCoroutine(OutlineEffectLerp(0f, _timeSettings.GetChangeTimeDelay));

        if(isStopped)
            StartCoroutine(HighlightEffectLerp(0f,_timeSettings.GetChangeTimeDelay));
    }
    
    // *********/ PUBLIC METHODS /********* //
    
    public void UseTimeCell()
    {
        // Flip the time state.
        isStopped = !isStopped;

        outline.OutlineColor = _timeSettings.GetPoweredOutlineColor;
        //Apply Shader
        StartCoroutine(HighlightEffectLerp(1f,_timeSettings.GetChangeTimeDelay, _timeSettings.GetPoweredFresnelColor, _timeSettings.GetPoweredInteriorColor));
        // Delay before resuming time.
        StartCoroutine(DelayAction(ResumeTime));
    }
    public void TakeTimeCell()
    {
        isStopped = !isStopped;

        outline.OutlineColor = _timeSettings.GetDefaultOutlineColor;
        //Apply shader
        StartCoroutine(HighlightEffectLerp(1f,_timeSettings.GetChangeTimeDelay, _timeSettings.GetDefaultFresnelColor, _timeSettings.GetDefaultInteriorColor));
        //Delay before stopping time.
        StartCoroutine(DelayAction(StopTime));
    }

    // *********/ PRIVATE METHODS /********* //

    
    // *********/ COROUTINES /********* //

    private IEnumerator DelayAction(Action action)
    {
        yield return new WaitForSeconds(_timeSettings.GetChangeTimeDelay);
        action();
    }
    /// <summary>
    /// Interpolate the value of the highlight effect.
    /// </summary>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator HighlightEffectLerp(float endValue, float duration)
    {
        float time = 0f;
        float startValue = renderer.material.GetFloat("_EffectBlend");
        
        while (time < duration)
        {
            renderer.material.SetFloat("_EffectBlend", Mathf.Lerp(startValue, endValue, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        renderer.material.SetFloat("_EffectBlend", endValue);
    }
    private IEnumerator HighlightEffectLerp(float endValue, float duration, Color endFresnelColor, Color endInteriorColor)
    {
        float time = 0f;
        float startValue = renderer.material.GetFloat("_EffectBlend");

        Color startFresnelColor = renderer.material.GetColor("_FresnelColor");
        Color startInteriorColor = renderer.material.GetColor("_InteriorColor");
        
        while (time < duration)
        {
            renderer.material.SetFloat("_EffectBlend", Mathf.Lerp(startValue, endValue, time / duration));
            renderer.material.SetColor("_FresnelColor", Color.Lerp(startFresnelColor, endFresnelColor, time / duration));
            renderer.material.SetColor("_InteriorColor", Color.Lerp(startInteriorColor, endInteriorColor, time / duration));

            time += Time.deltaTime;
            yield return null;
        }
        renderer.material.SetFloat("_EffectBlend", endValue);
        renderer.material.SetColor("_FresnelColor", endFresnelColor);
        renderer.material.SetColor("_InteriorColor", endInteriorColor);
    }
    private IEnumerator OutlineEffectLerp(float endValue, float duration)
    {
        float time = 0f;
        float startValue = outline.OutlineWidth;
        
        while (time < duration)
        {
            outline.OutlineWidth = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        outline.OutlineWidth = endValue;
    }
}
