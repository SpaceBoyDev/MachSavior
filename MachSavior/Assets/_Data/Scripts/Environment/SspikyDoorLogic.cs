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
        Door.GetComponent<AnimatedTimeObject>().tweenAnim = Door.transform.DOLocalMoveY(1f, speed).
        SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    void StopDoor()
    {
        Door.GetComponent<AnimatedTimeObject>().tweenAnim =
            Door.transform.DOLocalMoveY(3.5f, speed).SetEase(Ease.Linear);

    }

    private void Update()
    {
        if (Door.GetComponent<AnimatedTimeObject>().getIsStopped)
        {
            Colision.isTrigger = false;
        }
        else
        {
            StopDoor();
            Colision.isTrigger = true;
        }
    }
}
