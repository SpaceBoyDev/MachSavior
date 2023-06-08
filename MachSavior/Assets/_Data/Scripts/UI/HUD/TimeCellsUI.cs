using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages the time cells UI behaviour.
/// </summary>
public class TimeCellsUI : MonoBehaviour
{
    [SerializeField] private GameObject timeCellPrefab;
    [SerializeField] private TimeControlSettings _timeControlSettings;

    private List<TimeCell> timeCells = new List<TimeCell>();

    private void Start()
    {
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
}
