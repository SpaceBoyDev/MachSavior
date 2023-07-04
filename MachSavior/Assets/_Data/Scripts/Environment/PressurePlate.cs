using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private int weightToActive;
    [SerializeField] private int currentWeight;

    [SerializeField] private GameEvent eventToActive;
    
    void Start()
    {
        currentWeight = 0;
    }

    void Update()
    {
        CheckCurrentWeightIn();
        if (weightToActive <= CheckCurrentWeightIn())
        {
            // TO DO Animation of active plate here
            eventToActive.Raise();
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PickableObject>())
        {
            currentWeight += other.gameObject.GetComponent<PickableObject>().currentWeight;
            Debug.Log(this.gameObject.name + " current weight: " + currentWeight);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PickableObject>())
        {
            currentWeight -= other.gameObject.GetComponent<PickableObject>().currentWeight;
            Debug.Log(this.gameObject.name + " current weight: " + currentWeight);
        }
    }

    int CheckCurrentWeightIn()
    {
        return currentWeight;
    }
    
}
