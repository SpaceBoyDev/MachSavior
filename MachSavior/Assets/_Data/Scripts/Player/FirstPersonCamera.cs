using Rewired;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig playerConfig;

    [SerializeField]
    private PlayerMovement playerMovement;

    private Camera playerCamera;

    private float mouseX, mouseY;

    private int maxY = 290;
    private int minY = 70;

    float rotationY;
    bool change = false;

    private float maxX, minX;

    private IEnumerator  wallrunCameraCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = CameraManager.Instance.GetPlayerCamera();
    }

    // Update is called once per frame
    void Update()
    {
        CameraInputs();
        ClampCamera();
    }

    private void CameraInputs()
    {
        if (PlayerInputManager.Instance.IsHorizontalMouseAllowed)
        {
            mouseX = PlayerInputManager.Instance.GetHorizontalMouse() * playerConfig.sensibilityX;
        }

        mouseY = -PlayerInputManager.Instance.GetVerticalMouse() * playerConfig.sensibilityY;
    }

    private float ClampFloatAround360(float value)
    {
        if (value < 0)
        {
            return 360 + value;
        }
        else if (value > 360)
        {
            return value - 360;
        }
        else
        {
            return value;
        }
    }
    
    private void ClampCamera()
    {
        if(!CameraManager.Instance.IsCameraUpdateAllowed())
        {
            return;
        }
        else if(wallrunCameraCoroutine != null)
        {
            StopCoroutine(wallrunCameraCoroutine);
            wallrunCameraCoroutine = null;
        }
        
        //--->CLAMP HORIZONTAL<---//
        if (CameraManager.Instance.GetIsClampingCameraHorizontal())
        { 
            mouseX = Mathf.Clamp(mouseX, -19, 19);
            if (playerMovement.wallState == PlayerMovement.WallState.leftWall)
            {
               maxX = CameraManager.Instance.GetPlayerRotationYOnClamp() + 40;
               minX = CameraManager.Instance.GetPlayerRotationYOnClamp() - 10;
            }
            else
            {
                maxX = CameraManager.Instance.GetPlayerRotationYOnClamp() - 10;
                minX = CameraManager.Instance.GetPlayerRotationYOnClamp() + 40;
            }

           maxX = ClampFloatAround360(maxX);
           minX = ClampFloatAround360(minX);
           
           float cannotAddToX = playerCamera.transform.eulerAngles.y + mouseX;
           float deadZoneMaxX = maxX + 20;
           float deadZoneMinX = minX - 20;
           
           cannotAddToX = ClampFloatAround360(cannotAddToX);
           deadZoneMaxX = ClampFloatAround360(deadZoneMaxX);
           deadZoneMinX = ClampFloatAround360(deadZoneMinX);

           //HERE ACTUAL CLAMP
           if (cannotAddToX < minX && cannotAddToX > deadZoneMinX)
           {
               playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, minX, playerCamera.transform.eulerAngles.z);
               print("Por debajo de" + minX);
           }
           else if (cannotAddToX > maxX && cannotAddToX < deadZoneMaxX)
           {
               playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, maxX, playerCamera.transform.eulerAngles.z);
               print("Por encima de" + maxX);
           }
           else
           {
               playerCamera.transform.eulerAngles += new Vector3(0f, mouseX, 0f);
               //print("mouse x sumando-> " + mouseX);
           }
           
        }
        else
        {
            playerCamera.transform.eulerAngles += new Vector3(0f, mouseX, 0f);
        }

        //--->CLAMP VERTICAL<---//
        if (PlayerInputManager.Instance.IsHorizontalMouseAllowed)
        {
            mouseY = Mathf.Clamp(mouseY, -19, 19);
            float cannotAddTo = playerCamera.transform.eulerAngles.x + mouseY;
            if (cannotAddTo < maxY && cannotAddTo > 270)
            {
                playerCamera.transform.eulerAngles = new Vector3(maxY, playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);
            }
            else if (cannotAddTo > minY && cannotAddTo < 90)
            {
                playerCamera.transform.eulerAngles = new Vector3(minY, playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);
            }
            else
            {
                playerCamera.transform.eulerAngles += new Vector3(mouseY, 0f, 0f);
            }
        }
        else
        {
            playerCamera.transform.eulerAngles += new Vector3(mouseY, 0f, 0f);
        }

        //Update player transform
        if (!playerMovement.OnWallrun)
        {
            transform.eulerAngles = new Vector3(0, playerCamera.transform.rotation.eulerAngles.y, 0);
        }
    }


    public void LerpToWallrunForward(){
        wallrunCameraCoroutine = LerpWallrunCameraToForward();
        StartCoroutine(wallrunCameraCoroutine);
    }
    
    IEnumerator LerpWallrunCameraToForward(){
        //Vector3 newRotEuler = newRot.eulerAngles;
        Quaternion currentRot = playerCamera.transform.rotation;
        Vector3 currentRotEuler = currentRot.eulerAngles;
        Quaternion newRot = Quaternion.LookRotation(CameraManager.Instance.CameraWallrunLookForward - playerCamera.transform.position, Vector3.up);
        if (newRot != null)
        {
            float elapsedTime = 0f;
            float lerpTime = .2f;

            while(elapsedTime < lerpTime){
                Quaternion rot = Quaternion.Lerp(currentRot, newRot, (elapsedTime / lerpTime));
                playerCamera.transform.rotation = new Quaternion(
                    rot.x,
                    rot.y,
                    rot.z,
                    rot.w
                    );
                playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            wallrunCameraCoroutine = null;
            playerCamera.transform.rotation = newRot;
            playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0f);
            //CameraManager.Instance.SetIsClampingCameraHorizontal(true);
            CameraManager.Instance.SetIsCameraUpdateAllowed(true);
        }
    }
}
