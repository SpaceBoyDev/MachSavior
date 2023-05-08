using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("References")]
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Camera playerCamera;
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

    public GameObject GetRoot()
    {
        return root;
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

    public Transform GetCameraPos()
    {
        return cameraPos;
    }
}
