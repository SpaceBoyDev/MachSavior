using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField]
    private bool _canPick = true;
    
    [SerializeField]
    private bool _isPicked = false;

    public bool CanPick()
    {
        if (!_canPick)
        {
            return false;
        }
        return true;
    }

    public bool IsPicked()
    {
        if (!_isPicked)
        {
            return false;
        }
        return true;
    }
    
}
