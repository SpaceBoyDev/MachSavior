using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private int weightToActive;
    [SerializeField] private int currentWeight;

    [SerializeField] private GameEvent eventToActive;
    [SerializeField] private float YWhenActive;
    [SerializeField] private GameEvent eventToDesactive;
    [SerializeField] private float YWhenDesactive;

    private Tween activeTweenAnim;
    private Tween desactiveTweenAnim;
    public AudioSource plateActivate;
    private bool pressurePlateActive;
    void Start()
    {
        currentWeight = 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PickableObject>())
        {
            currentWeight += other.gameObject.GetComponent<PickableObject>().currentWeight;
            // Debug.Log(this.gameObject.name + " current weight: " + currentWeight);
            
            if (weightToActive <= CheckCurrentWeightIn() && pressurePlateActive == false)
            {
                PressurePlateActivate();
                eventToActive.Raise();
                pressurePlateActive = true;
            }
        }
        
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PickableObject>())
        {
            currentWeight -= other.gameObject.GetComponent<PickableObject>().currentWeight;
            
            // Debug.Log(this.gameObject.name + " current weight: " + currentWeight);
            if (weightToActive > CheckCurrentWeightIn() && pressurePlateActive == true)
            {
                PressurePlateDesactivate();
                eventToDesactive.Raise();
                pressurePlateActive = false;
            }
        }
    }

    int CheckCurrentWeightIn()
    {
        return currentWeight;
    }

    private void PressurePlateActivate()
    {
        activeTweenAnim = transform.DOLocalMoveY(YWhenActive, 1).SetEase(Ease.Linear);
        activeTweenAnim.Play();
        plateActivate.Play();
    }
    private void PressurePlateDesactivate()
    {
        desactiveTweenAnim = transform.DOLocalMoveY(YWhenDesactive, 1).SetEase(Ease.Linear);
        desactiveTweenAnim.Play();
    }
    
}
