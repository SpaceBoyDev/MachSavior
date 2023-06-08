using UnityEngine;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrompt, timeCells;

    private void Start()
    {
        buttonPrompt.SetActive(false);
    }

    public void HoverEnter()
    {
        buttonPrompt.SetActive(true);
        //timeCells.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutCubic);
    }

    public void HoverExit()
    {
        buttonPrompt.SetActive(false);
        //timeCells.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).SetEase(Ease.InOutCubic);
    }
}
