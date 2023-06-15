using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PickableObject : MonoBehaviour
{
    [SerializeField]
    private bool _canPick = true;
    
    [SerializeField]
    private bool _useGravity = false;
    
    [SerializeField]
    public bool _isPicked = false;
    
    private enum ObjectWeightCategory 
    { 
        lightWeight = 0, 
        midWeight = 1, 
        heavyWeight = 2 
    };

    [SerializeField] private ObjectWeightCategory objectWeight;
    [SerializeField] private Vector3 _rotationWhenPick = Vector3.zero;
    [SerializeField] private Vector3 _positionWhenPick = Vector3.zero;

    #region Bolean
    public bool CanPick()
    {
        if (!_canPick)
        {
            return false;
        }
        return true;
    }
    public bool UseGravity()
    {
        if (!_useGravity)
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

    #endregion
    
    
    #region ObjectWeight
    public int ObjectWeight()
    {
        if(objectWeight == ObjectWeightCategory.heavyWeight)
        {
            return 2;
        }

        if (objectWeight == ObjectWeightCategory.midWeight)
        {
            return 1;
        }

        if (objectWeight == ObjectWeightCategory.lightWeight)
        {
            return 0;
        }

        return 0;

    }
    #endregion

    #region  RotationWhenPick

    public Vector3 RotationWhenPick()
    {
        return _rotationWhenPick;
    }

    #endregion
    
    #region  PositionWhenPick

    public Vector3 PositionWhenPick()
    {
        return _positionWhenPick;
    }

    #endregion
}
