using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SspikyDoorLogic : MonoBehaviour
{
    [SerializeField] private BoxCollider Colision;
    [SerializeField] private GameObject DoorR;
    [SerializeField] private GameObject DoorL;

    Tween DoorRT;
    Tween DoorLT;
    void Start()
    {
        Colision.isTrigger = false;
        DoorRT = DoorR.transform.DOLocalMoveZ(-2, 0.1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        DoorLT = DoorL.transform.DOLocalMoveZ(-2, 0.1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        DoorRT.Play();
        DoorLT.Play();
    }


    public void DesactivateDoor()
    {
        Colision.isTrigger = false;
        
        DoorLT.timeScale = 0.05f;
        DoorRT.timeScale = 0.05f;
    }
}
