using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    [Header("References")]
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Transform cameraExtraPos;
    [SerializeField]
    private Transform cameraExtraRot;
    [SerializeField]
    private TextMeshProUGUI textFPS;
    [SerializeField] private GameEvent OnPause;

    private float mouseX = 0, mouseY = 0;

    public float GetMouseX()
    {
        return mouseX;
    }

    public float GetMouseY()
    {
        return mouseY;
    }

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

    private void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }

        textFPS.text = m_lastFramerate.ToString();

        if (PlayerInputManager.Instance.GetPause())
        {
            OnPause.Raise();
            print("Pausa");
        }
    }

    public GameObject GetRoot()
    {
        return root;
    }

    public float GetPlayerRotationY()
    {
        return root.transform.rotation.y;
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

    public Transform GetCameraExtraPos()
    {
        return cameraExtraPos;
    }

    public Transform GetCameraExtraRot()
    {
        return cameraExtraRot;
    }

}
