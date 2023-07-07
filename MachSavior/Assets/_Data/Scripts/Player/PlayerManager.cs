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
    private Vector3 additionalTurbineSpeed = Vector3.zero;
    private float gravityAdditionalTurbineSpeed = 10f;
    private bool isGravityAdditionalTurbineSpeedApplied = true;

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
        }
        
        ReduceAdditionalTurbineSpeed();
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

    public Vector3 GetAdditionalTurbineSpeed()
    {
        return additionalTurbineSpeed;
    }

    public void CalculateAdditionalTurbineSpeed(Vector3 direction, float force)
    {
        additionalTurbineSpeed = direction * force;
    }

    public void SetIsGravityAdditionalTurbineSpeedApplied(bool value)
    {
        isGravityAdditionalTurbineSpeedApplied = value;
    }
    
    private void ReduceAdditionalTurbineSpeed()
    {
        if (!isGravityAdditionalTurbineSpeedApplied)
        {
            additionalTurbineSpeed = Vector3.zero;
            return;
        }
        
        if (additionalTurbineSpeed.x > 0f)//X
        {
            additionalTurbineSpeed -= new Vector3(gravityAdditionalTurbineSpeed,0,0) * Time.deltaTime;

            if (additionalTurbineSpeed.x < 0f)
            {
                additionalTurbineSpeed = new Vector3(0f, additionalTurbineSpeed.y, additionalTurbineSpeed.z);
            }
        }
        else if (additionalTurbineSpeed.x < 0f)
        {
            additionalTurbineSpeed += new Vector3(gravityAdditionalTurbineSpeed, 0, 0) * Time.deltaTime;

            if (additionalTurbineSpeed.x > 0f)
            {
                additionalTurbineSpeed = new Vector3(0f, additionalTurbineSpeed.y, additionalTurbineSpeed.z);
            }
        }

        if (additionalTurbineSpeed.y > 0f)//Y
        {
            additionalTurbineSpeed -= new Vector3(0, gravityAdditionalTurbineSpeed, 0) * Time.deltaTime;

            if (additionalTurbineSpeed.y < 0f)
            {
                additionalTurbineSpeed = new Vector3(additionalTurbineSpeed.x, 0f, additionalTurbineSpeed.z);
            }
        }
        else if (additionalTurbineSpeed.y < 0f)
        {
            additionalTurbineSpeed += new Vector3(0, gravityAdditionalTurbineSpeed, 0) * Time.deltaTime;

            if (additionalTurbineSpeed.y > 0f)
            {
                additionalTurbineSpeed = new Vector3(additionalTurbineSpeed.x, 0f, additionalTurbineSpeed.z);
            }
        }
        
        if (additionalTurbineSpeed.z > 0f)//Z
        {
            additionalTurbineSpeed -= new Vector3(0, 0, gravityAdditionalTurbineSpeed) * Time.deltaTime;

            if (additionalTurbineSpeed.z < 0f)
            {
                additionalTurbineSpeed = new Vector3(additionalTurbineSpeed.x, additionalTurbineSpeed.y, 0f);
            }
        }
        else if (additionalTurbineSpeed.z < 0f)
        {
            additionalTurbineSpeed += new Vector3(0, 0, gravityAdditionalTurbineSpeed) * Time.deltaTime;

            if (additionalTurbineSpeed.z > 0f)
            {
                additionalTurbineSpeed = new Vector3(additionalTurbineSpeed.x, additionalTurbineSpeed.y, 0f);
            }
        }
    }

}
