using System;
using UnityEngine;
using DG.Tweening;

public class AnimatedTimeObject : TimeObject
{
    public Tween tweenAnim;

    public override void ResumeTime()
    {
        tweenAnim.Play();
    }

    public override void StopTime()
    {
        tweenAnim.Pause();
    }
}
