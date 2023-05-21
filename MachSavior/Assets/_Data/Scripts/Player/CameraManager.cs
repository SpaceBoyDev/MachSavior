using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    //private Camera playerCamera; //FirstPersonCamera

    public Camera GetPlayerCamera()
    {
        return PlayerManager.Instance.GetPlayerCamera();
    }

    [SerializeField]
    private Transform cameraPos;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        cameraPos = PlayerManager.Instance.GetCameraPos();
        PlayerManager.Instance.GetPlayerCamera().transform.parent = null;
    }

    private void Update()
    {
        PlayerManager.Instance.GetPlayerCamera().transform.position = cameraPos.position;
    }
}
