using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int startRotationZ;
    public float duration;
    private Vector2 startPosition;

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, startRotationZ);
        startPosition = transform.position;
        //transform.DORotate(new Vector3(0, 0, -startRotationZ), duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetUpdate(true);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        transform.DOMoveY(startPosition.y + 10f, 0.3f, false).SetUpdate(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOMoveY(startPosition.y, 0.3f, false).SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable == true)
        {
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetUpdate(true);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetUpdate(true).SetEase(Ease.InQuad);
    }
}
