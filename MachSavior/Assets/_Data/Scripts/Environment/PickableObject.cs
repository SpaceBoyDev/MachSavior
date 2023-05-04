using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField]
    private bool _canPick = true;

    public bool CanPick()
    {
        if (!_canPick)
        {
            return false;
        }
        return true;
    }
}
