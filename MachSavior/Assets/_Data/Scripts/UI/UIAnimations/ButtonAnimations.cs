using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    IPointerUpHandler
{
   // private RectTransform rectTransform;
    public int startRotationZ;
    public float duration;
    private Vector3 startPosition;
    private Vector3 startScale;
    private Quaternion startRotation;
    public bool isCanvasWorld = true;
    public Vector3 newPosition = new Vector3(0.05f, 0f, 0f);
    public Vector3 newScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void Start()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        startRotation = transform.rotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isCanvasWorld)
        {
            transform.DOMove(startPosition + newPosition, 0.3f, false).SetUpdate(true);
        }
        else
        {
            transform.DOLocalMove(startPosition + newPosition, 0.3f, false).SetUpdate(true).SetRelative(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("exit button");
        transform.DOMove(startPosition, 0.3f, false).SetUpdate(true);
        //rectTransform.DOLocalMove(startPosition, 0.3f, false).SetUpdate(true).SetRelative(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable == true)
        {
            transform.DOScale(startScale + newScale, 0.2f).SetUpdate(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(startScale, 0.2f).SetUpdate(true).SetEase(Ease.InQuad);
    }
}
