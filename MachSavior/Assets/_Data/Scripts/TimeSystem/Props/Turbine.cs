using System;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(AnimatedTimeObject))]
public class Turbine : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;

    private AnimatedTimeObject _animatedTimeObject;

    private void Start()
    {
        _animatedTimeObject = GetComponent<AnimatedTimeObject>();
        RotateTurbine();
        if(_animatedTimeObject.GetIsStopped())
            _animatedTimeObject.StopTime();
        else 
            _animatedTimeObject.ResumeTime();
    }
    
    /// <summary>
    /// DOTween Anim for rotating the turbine.
    /// </summary>
    private void RotateTurbine()
    {
        _animatedTimeObject.tweenAnim = transform.DORotate(new Vector3(0f, 0f, 180f), speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear).SetRelative();
    }
}
