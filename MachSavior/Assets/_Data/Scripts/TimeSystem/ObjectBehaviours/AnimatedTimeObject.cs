using System;
using UnityEngine;
using DG.Tweening;

public class AnimatedTimeObject : TimeObject
{
    public Tween tweenAnim;
    private bool isActive;
    public bool IsActive() { return isActive; }

    public override void ResumeTime()
    {
        tweenAnim.Play();
        isActive = true;
    }

    public override void StopTime()
    {
        tweenAnim.Pause();
        isActive = false;
    }
}
