using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum TimeCellStatus
{
    Empty = 0,
    Full = 1
}
public class TimeCell : MonoBehaviour
{
    [SerializeField] private Sprite emptyTimeCell, fullTimeCell;
    private Image timeCellImage;
    
    private void Awake()
    {
        timeCellImage = GetComponent<Image>();
    }

    public void SetCellImage(TimeCellStatus cellStatus)
    {
        switch (cellStatus)
        {
            case TimeCellStatus.Empty:
                timeCellImage.sprite = emptyTimeCell;
                break;
            case TimeCellStatus.Full:
                timeCellImage.sprite = fullTimeCell;
                break;
        }
    }
}
