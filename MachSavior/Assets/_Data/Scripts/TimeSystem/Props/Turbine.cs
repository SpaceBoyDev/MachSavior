using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

[RequireComponent(typeof(AnimatedTimeObject))]
public class Turbine : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float airForce = 0.2f;
    [SerializeField] private GameObject turbine;
    [SerializeField] private GameObject airCollider;
    [SerializeField] private ParticleSystem airParticles;
    [SerializeField] private bool isActive;

    private AnimatedTimeObject _animatedTimeObject;

    private void Start()
    {
        _animatedTimeObject = GetComponent<AnimatedTimeObject>();
        RotateTurbine();
    }

    private void Update()
    {
        //if (_animatedTimeObject.getIsStopped)
        //{
        //        ChangeIsStopped(isActive);
        //    
        //}
        //else
        //{
        //        _animatedTimeObject.ResumeTime();
        //        airParticles.Play();
        //        speed = 3f;
        //}
        
        ChangeIsStopped(_animatedTimeObject.IsActive());
    }

    private void ChangeIsStopped(bool state)
    {
        if (state == isActive)
        {
            return;
        }
        else if (state == false && isActive == true)
        {
            OnExitActive();
        }
        else if (state == true && isActive == false)
        {
            OnEnterActive();
        }
    
        isActive = state;
    }

    private void OnEnterActive()
    {
        print("Entrada");
        _animatedTimeObject.ResumeTime();
        airParticles.Play();
        speed = 3f;
        airCollider.SetActive(true);
    }

    private void OnExitActive()
    {
        print("Salida");
        _animatedTimeObject.StopTime();
        airParticles.Stop();
        speed = 0.2f;
        airCollider.SetActive(false);
    }
    
    /// <summary>
    /// DOTween Anim for rotating the turbine.
    /// </summary>
    private void RotateTurbine()
    {
        _animatedTimeObject.tweenAnim = turbine.transform.DOLocalRotate(new Vector3(180f, 0f, 0f), speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear).SetRelative();

        _animatedTimeObject.tweenAnim.Play();
    }


    private void ApplyAirForce()
    {
        if (_animatedTimeObject.IsActive())
        {
            
        }
    }


}

