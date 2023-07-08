using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SspikyDoorLogic : MonoBehaviour
{
    [SerializeField] private BoxCollider Colision;

    [SerializeField] private float speed;

    [SerializeField] private GameObject Door;
    
    void Start()
    {
       Door.GetComponent<AnimatedTimeObject>().ResumeTime();
       OpenCloseDoor();
    }

    void OpenCloseDoor()
    {
        Colision.isTrigger = false;
        Door.GetComponent<AnimatedTimeObject>().tweenAnim = Door.transform.DOLocalMoveY(1f, speed).
        SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (Door.GetComponent<AnimatedTimeObject>().getIsStopped)
        {
            Colision.isTrigger = true;
        }
        else
        {
            Colision.isTrigger = false;
        }
    }
}
