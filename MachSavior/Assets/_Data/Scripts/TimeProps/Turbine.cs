using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Turbine : MonoBehaviour, ITimeInteractable
{
    private bool isStopped;
    private float duration = 0.2f;

    private Tween rotTween;

    private void Start()
    {
        RotateTurbine();
    }
    
    private void RotateTurbine()
    {
        rotTween = transform.DORotate(new Vector3(0f, 0f, 180f), 0.2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear).SetRelative();
    }

    public bool GetIsStopped()
    {
        return isStopped;
    }

    public void StopTime()
    {
        isStopped = true;
        rotTween.Pause();
    }

    public void ResumeTime()
    {
        isStopped = false;
        rotTween.Restart();
    }
}
