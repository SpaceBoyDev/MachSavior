using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [Header("Image Keys")]
    [SerializeField] private Image wKey;
    [SerializeField] private Image aKey;
    [SerializeField] private Image sKey;
    [SerializeField] private Image dKey;
    
    [SerializeField] private Image spacebar;
    
    [Header("Sprite Pressed Keys")]
    [SerializeField] private Sprite wKeyPressed;
    [SerializeField] private Sprite aKeyPressed;
    [SerializeField] private Sprite sKeyPressed;
    [SerializeField] private Sprite dKeyPressed;
    
    [SerializeField] private Sprite spacebarPressed;
    
    private enum TutorialType { checkMovementInputs, checkJumpInputs, textBox, textBoxTimeStop }
    [SerializeField] private TutorialType tutorialType;
    [SerializeField] private bool hasBeenTriggered = false;
    [SerializeField] private float onlyTextBoxSeconds;
    [SerializeField] private float stopTimeSeconds;

    private void Awake()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered && !TutorialManager.Instance.isTriggerOn)
        {
            hasBeenTriggered = true;
            TutorialManager.Instance.isTriggerOn = true;
            canvas.SetActive(true);
            if (tutorialType == TutorialType.checkMovementInputs)
            {
                StartCoroutine(CheckMovementInputs());
            }
            else if (tutorialType== TutorialType.checkJumpInputs)
            {
                StartCoroutine(CheckJumpInput());
            }
            else if (tutorialType == TutorialType.textBox)
            {
                StartCoroutine(TextBox());
            }
            else if (tutorialType == TutorialType.textBoxTimeStop)
            {
                StartCoroutine(TextBoxTimeStop());
            }
        }
    }
    
    private IEnumerator TextBoxTimeStop()
    {
        PlayerInputManager.Instance.IsInputAllowed = false;
        PlayerInputManager.Instance.IsPauseAllowed= false;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(stopTimeSeconds);
        
        PlayerInputManager.Instance.IsInputAllowed = true;
        PlayerInputManager.Instance.IsPauseAllowed= true;
        Time.timeScale = 1;
        
        yield return new WaitForSeconds(onlyTextBoxSeconds);
        TutorialManager.Instance.isTriggerOn = false;
        canvas.SetActive(false);
    }
    
    private IEnumerator TextBox()
    {
        yield return new WaitForSeconds(onlyTextBoxSeconds);
        TutorialManager.Instance.isTriggerOn = false;
        canvas.SetActive(false);
    }

    private IEnumerator CheckJumpInput()
    {
        bool jumpInputChecked = false;
        while (!jumpInputChecked)
        {
            if (PlayerInputManager.Instance.IsJumpDown())
            {
                spacebar.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 0).SetEase(Ease.InQuad)
                    .SetLoops(0);
                spacebar.sprite = spacebarPressed;
                jumpInputChecked = true;
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        TutorialManager.Instance.isTriggerOn = false;
        canvas.SetActive(false);
    }

    private IEnumerator CheckMovementInputs()
    {
        int inputsChecked = 0;
        int[] keysPressed = new int[4];
        while (inputsChecked < 4)
        {
            inputsChecked = 0;
            if (PlayerInputManager.Instance.GetVerticalMovement() > 0 && keysPressed[0] == 0)
            {
                keysPressed[0] = 1;
                wKey.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 0).SetEase(Ease.InQuad)
                    .SetLoops(0);
                wKey.sprite = wKeyPressed;
            }
            
            if (PlayerInputManager.Instance.GetVerticalMovement() < 0 && keysPressed[1] == 0)
            {
                keysPressed[1] = 1;
                sKey.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 0).SetEase(Ease.InQuad)
                    .SetLoops(0);
                sKey.sprite = sKeyPressed;
            }
            
            if (PlayerInputManager.Instance.GetHorizontalMovement() > 0 && keysPressed[2] == 0)
            {
                keysPressed[2] = 1;
                dKey.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 0).SetEase(Ease.InQuad)
                    .SetLoops(0);
                dKey.sprite = dKeyPressed;
            }
            
            if (PlayerInputManager.Instance.GetHorizontalMovement() < 0 && keysPressed[3] == 0)
            {
                keysPressed[3] = 1;
                aKey.gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 0).SetEase(Ease.InQuad)
                    .SetLoops(0);
                aKey.sprite = aKeyPressed;
            }

            for (int i = 0; i < keysPressed.Length; i++)
            {
                inputsChecked += keysPressed[i];
            }

            yield return null;

        }

        yield return new WaitForSeconds(1f);
        TutorialManager.Instance.isTriggerOn = false;
        canvas.SetActive(false);
    }
}
