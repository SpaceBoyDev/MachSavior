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
        return PlayerManager.Instance.playerCamera;
    }

    [SerializeField]
    private Transform cameraPos;

    private void Awake()
    {
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
        cameraPos = PlayerManager.Instance.cameraPos;
        PlayerManager.Instance.playerCamera.transform.parent = null;
    }

    private void Update()
    {
        PlayerManager.Instance.playerCamera.transform.position = cameraPos.position;
    }
}
