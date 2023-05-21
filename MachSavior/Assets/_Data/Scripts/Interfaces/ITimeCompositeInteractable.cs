using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITimeCompositeInteractable : MonoBehaviour, ITimeInteractable
{
    [SerializeField] private List<GameObject> timeInteractableObjects;

    public void Add(GameObject interactable)
    {
        timeInteractableObjects.Add(interactable);
    }

    public bool GetIsStopped()
    {
        foreach (var timeInteractableObject in timeInteractableObjects)
        {
            var interactable = timeInteractableObject.GetComponent<ITimeInteractable>();
            if (interactable == null)
                continue;
            return interactable.GetIsStopped();
        }

        return false;
    }

    public void ChangeTimeState()
    {
        foreach (var timeInteractableObject in timeInteractableObjects)
        {
            var interactable = timeInteractableObject.GetComponent<ITimeInteractable>();
            if (interactable == null)
                continue;
            interactable.ChangeTimeState();
        }
    }
}
