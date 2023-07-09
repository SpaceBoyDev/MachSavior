using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private Transform cameraStartTransform;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Transform camPC;
    [SerializeField] private AnimationCurve curve;
    private float lerpSpeed;
    private bool isCameraAnimDone = true;

    private void Awake()
    {
        cameraStartTransform = camera.transform;
    }

    public void OnButtonPlay()
    {
        buttons.SetActive(false);
        StartCoroutine(CameraCloseUp());
    }
    
    public void OnButtonOptions()
    {
        
    }
    
    public void OnButtonExit()
    {
        Application.Quit();
        print("Quit app");
    }

    private IEnumerator CameraCloseUp()
    {
        lerpSpeed = 0;
        isCameraAnimDone = false;
        while (lerpSpeed < 1)
        {
            camera.transform.position =
                Vector3.Lerp(cameraStartTransform.position, camPC.transform.position, lerpSpeed);
            camera.transform.rotation =
                Quaternion.Lerp(cameraStartTransform.rotation, camPC.transform.rotation, lerpSpeed);

            lerpSpeed += Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.1f);
        
        SceneLoader.Instance.Load(1);
    }
}
