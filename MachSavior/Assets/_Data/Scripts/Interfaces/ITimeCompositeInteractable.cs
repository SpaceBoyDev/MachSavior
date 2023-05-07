using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITimeCompositeInteractable : MonoBehaviour, ITimeInteractable
{
    [SerializeField] private List<GameObject> timeInteractObjects;

    public bool GetIsStopped()
    {
        throw new System.NotImplementedException();
    }

    public void StopTime()
    {
        throw new System.NotImplementedException();
    }

    public void ResumeTime()
    {
        throw new System.NotImplementedException();
    }
}
