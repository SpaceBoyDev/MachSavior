using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SspikyDoorLogic : MonoBehaviour
{
    [FormerlySerializedAs("Colision")] [SerializeField] private BoxCollider doorCollider;
    [SerializeField] private AnimatedTimeObject _animatedTimeObject;
    [SerializeField] private float speed;
    private Vector3 initPosition;

    [FormerlySerializedAs("Door")] [SerializeField] private GameObject door;
    [SerializeField] private bool isActive;

    private void Start()
    {
        initPosition = door.transform.position;
        _animatedTimeObject = door.GetComponent<AnimatedTimeObject>();
        ChangeIsStopped(_animatedTimeObject.IsActive());
    }

    private void Update()
    {
        ChangeIsStopped(!_animatedTimeObject.getIsStopped);
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
        print("enter");
        doorCollider.isTrigger = false;
        speed = 0.1f;
        MoveDoor();
    }

    private void OnExitActive()
    {
        print("exit");
        doorCollider.isTrigger = true;
        speed = 0f;
        StopDoor();
    }

    private void MoveDoor()
    {
        door.transform.position = initPosition;
        _animatedTimeObject.tweenAnim = door.transform.DOMoveY(initPosition.y + 3.5f, speed).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        _animatedTimeObject.ResumeTime();
    }

    private void StopDoor()
    {
        door.transform.position =
            new Vector3(door.transform.position.x, initPosition.y + 3.5f, door.transform.position.z);
        _animatedTimeObject.StopTime();
        _animatedTimeObject.tweenAnim.Kill();
    }
}
