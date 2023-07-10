using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SspikyDoorLogic : MonoBehaviour
{
    [SerializeField] private BoxCollider Colision;

    [SerializeField] private float speed;

    [SerializeField] private GameObject Door;
    private bool isActive;
    private void Start()
    {
        OpenCloseDoor();
        CheckTimeState(Door.GetComponent<AnimatedTimeObject>().IsActive());
    }

    void Update()
    {
        CheckTimeState(!Door.GetComponent<AnimatedTimeObject>().getIsStopped);
        
        if (!Door.GetComponent<AnimatedTimeObject>().getIsStopped)
        {
            Colision.isTrigger = false;
        }
        else
        {
            Colision.isTrigger = true;
        }
    }

    IEnumerator StopDoorCourutine()
    {
        yield return new WaitForSeconds(0.1f);
        isActive = false;
    }
    IEnumerator ActiveDoorCourutine()
    {
        yield return new WaitForSeconds(0.1f);
        isActive = true;
        OpenCloseDoor();
    }
    void OpenCloseDoor()
    {
        Door.GetComponent<AnimatedTimeObject>().tweenAnim = Door.transform.DOLocalMoveY(1f, speed).
        SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        
        Door.GetComponent<AnimatedTimeObject>().tweenAnim.Play();
        isActive = true;
    }
    
    private void CheckTimeState(bool state)
    {
        if (state == isActive)
        {
            return;
        }
        else if (state == false && isActive == true)
        {
            Door.GetComponent<AnimatedTimeObject>().ResumeTime();
            ActiveDoorCourutine();
            Colision.isTrigger = true;
        }
        else if (state == false && isActive == false)
        {
            Door.GetComponent<AnimatedTimeObject>().StopTime();
            StopDoorCourutine();
            Colision.isTrigger = false;
        }

        isActive = state;

    }

}
