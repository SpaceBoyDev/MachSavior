using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

/// <summary>
/// Manages the time cells UI behaviour.
/// </summary>
public class TimeCellsUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject timeCellPrefab;
    [SerializeField] private TimeControlSettings _timeControlSettings;

    private List<TimeCell> timeCells = new List<TimeCell>();

    [Header("Shake Animation")] 
    
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 15f;
    [SerializeField, Range(0, 100)] private int shakeVibration = 20;
    [SerializeField, Range(0, 90)] private float shakeRandomness = 0f;
    
    [Header("Scale Animation")] 
    
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float scaleUpSize = 1f;
    [SerializeField] private float scaleDownSize = 0.7f;
    [SerializeField] private Ease scaleEase;

    private void Start()
    {
        transform.localScale = new Vector3(scaleDownSize, scaleDownSize, scaleDownSize);
        DrawTimeCells();
    }

    public void DrawTimeCells()
    {
        ClearTimeCells();
        
        //Draw max number of cells;
        int cellsToDraw = _timeControlSettings.GetMaxTimeCells;
        for (int i = 0; i < cellsToDraw; i++)
        {
            CreateEmptyTimeCell();
        }

        for (int i = 0; i < timeCells.Count; i++)
        {
            int cellStatus = Mathf.Clamp(_timeControlSettings.CurrentTimeCells - i, 0, 1);
            timeCells[i].SetCellImage((TimeCellStatus)cellStatus);
        }
    }

    private void CreateEmptyTimeCell()
    {
        // Create a temporary reference to the prefab instance and add that instance to where it belongs in the hierarchy.
        GameObject newCell = Instantiate(timeCellPrefab);
        newCell.transform.SetParent(transform);
        // Create the Empty cell spot in the UI.
        TimeCell timeCellComponent = newCell.GetComponent<TimeCell>();
        timeCellComponent.SetCellImage(TimeCellStatus.Empty);
        timeCells.Add(timeCellComponent);   
    }
    
    private void ClearTimeCells()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        timeCells = new List<TimeCell>();
    }
    
    //*******/ ANIMATIONS /*******//

    public void ShakeCells()
    {
        transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0f, 0f), shakeVibration, shakeRandomness).SetEase(Ease.InOutQuad);
    }

    public void ScaleUpCells()
    {
        transform.DOScale(scaleUpSize, scaleDuration).SetEase(Ease.InOutCubic);
    }
    
    public void ScaleDownCells()
    {
        transform.DOScale(scaleDownSize, scaleDuration).SetEase(scaleEase);
    }
}
