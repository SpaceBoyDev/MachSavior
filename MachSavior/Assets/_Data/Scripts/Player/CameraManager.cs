using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private bool isClampingCameraHorizontal = false;

    private float playerRotationYOnClamp;
    private Vector3 cameraWallrunLookForward;
    public Vector3 CameraWallrunLookForward{
        get{
            return cameraWallrunLookForward;
        }
    }
    private bool isCameraUpdateAllowed = true;

    [SerializeField] FirstPersonCamera playerCam;

    public bool GetIsClampingCameraHorizontal()
    {
        return isClampingCameraHorizontal;
    }

    public void SetIsClampingCameraHorizontal(bool value)
    {
        playerRotationYOnClamp = playerCam.transform.rotation.eulerAngles.y;
        isClampingCameraHorizontal = value;
    }

    public void SetCameraWallrunLookForward(Vector3 newCameraWallrunLookForward){
        cameraWallrunLookForward = newCameraWallrunLookForward;
        Debug.DrawLine(cameraWallrunLookForward, cameraWallrunLookForward + (Vector3.up * 0.5f), Color.green, 2f);
    }

    public bool IsCameraUpdateAllowed(){
        return isCameraUpdateAllowed;
    }
    public void SetIsCameraUpdateAllowed(bool value){
        isCameraUpdateAllowed = value;    
        if(!isCameraUpdateAllowed)
        {
            playerCam.LerpToWallrunForward();
        }    
    }

    public Camera GetPlayerCamera()
    {
        return PlayerManager.Instance.GetPlayerCamera();
    }

    public float GetPlayerRotationYOnClamp()
    {
        return playerRotationYOnClamp;
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
    private void Start()
    {
        PlayerManager.Instance.GetCameraExtraRot().transform.parent = null;
    }

    private void LateUpdate()
    {
        PlayerManager.Instance.GetCameraExtraRot().transform.position = PlayerManager.Instance.GetCameraExtraPos().position;
    }

    private void ClampCameraHorizontal(float minHorizontal, float maxHorizontal)
    {
        
    }
}
