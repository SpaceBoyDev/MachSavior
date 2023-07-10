using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt, pickPrompt;
    [SerializeField] private GameObject timeCells;

    private void Start()
    {
        interactPrompt.SetActive(false);
        pickPrompt.SetActive(false);
    }

    public void HoverEnter()
    {
        interactPrompt.SetActive(true);

        //timeCells.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutCubic);
    }

    public void HoverExit()
    {
        interactPrompt.SetActive(false);

        //timeCells.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).SetEase(Ease.InOutCubic);
    }

    public void CanPickObject()
    {
        pickPrompt.SetActive(true);
    }

    public void CantPickObject()
    {
        pickPrompt.SetActive(false);
    } 
}
