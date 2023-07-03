using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Time Control Settings", menuName = ("MachSavior/TimeSystem"), order = 0)]
public class TimeControlSettings : ScriptableObject
{
    #region General 

    [Header("General")] 
    
    [SerializeField, Tooltip("The max interact distance from a time object.")] 
    private float maxDistance = 25f;
    public float GetMaxDistance => maxDistance;

    [SerializeField, Tooltip("The delay the object has when changing time states.")]
    private float changeTimeDelay = 0.2f;
    public float GetChangeTimeDelay => changeTimeDelay;

    #endregion

    #region Time Cells
    
    [Header("Time Cells")]
    
    [SerializeField] private int maxTimeCells = 3;
    public int GetMaxTimeCells => maxTimeCells;

    [HideInInspector]
    public int CurrentTimeCells;
    
    #endregion
    
    #region Events

    [Header("Events")] 
    
    [SerializeField] private GameEvent onHoverEnter;
    public GameEvent GetOnHoverEnter => onHoverEnter;

    [SerializeField] private GameEvent onHoverExit;
    public GameEvent GetOnHoverExit => onHoverExit;

    [SerializeField] private GameEvent onTimeCellUsed;
    public GameEvent GetOnTimeCellUsed => onTimeCellUsed;

    [SerializeField] private GameEvent onTimeCellsEmpty;
    public GameEvent GetOnTimeCellsEmpty => onTimeCellsEmpty;

    #endregion

    #region Effects
    
    [Header("Effects")]
    
    [SerializeField, ColorUsage(true, true)]
    private Color defaultFresnelColor;
    public Color GetDefaultFresnelColor
    {
        get { return defaultFresnelColor; }
    }
    //------------------------------------------------//
    
    [SerializeField,ColorUsage(true, true)] 
    private Color defaultInteriorColor;
    public Color GetDefaultInteriorColor
    {
        get { return defaultInteriorColor; }
    }
    //------------------------------------------------//
    
    [SerializeField, ColorUsage(true, true)]
    private Color poweredFresnelColor;
    public Color GetPoweredFresnelColor
    {
        get { return poweredFresnelColor; }
    }
    //------------------------------------------------//
    
    [SerializeField,ColorUsage(true, true)] 
    private Color poweredInteriorColor;
    public Color GetPoweredInteriorColor
    {
        get { return poweredInteriorColor; }
    }
    //------------------------------------------------//
    
    #endregion
}
