using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public bool isTriggerOn = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
}
