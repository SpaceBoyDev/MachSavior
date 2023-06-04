using UnityEngine;

[CreateAssetMenu(fileName = "Time Control Settings", menuName = ("MachSavior/TimeSystem"), order = 0)]
public class TimeControlSettings : ScriptableObject
{
    #region General 

    [Header("General")] 
    
    [SerializeField, Tooltip("The max interact distance from a time object.")] 
    private float maxDistance = 25f;
    public float GetMaxDistance
    {
        get { return maxDistance; }
    }
    
    #endregion

    #region Time Cells
    
    [Header("Time Cells")]
    
    [SerializeField] private int maxTimeCells = 3;
    public int GetMaxTimeCells
    {
        get { return maxTimeCells; }
    }
    
    [HideInInspector]
    public int currentTimeCells;
    
    #endregion
}
