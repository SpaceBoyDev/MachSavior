using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig playerConfig;

    [SerializeField]
    private PlayerMovement playerMovement;

    private Camera playerCamera;

    private float mouseX, mouseY;

    private int maxY = 89;
    private int minY = -89;

    float rotationY;
    bool change = false;

    private Vector2 horizontalClamp;

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
            mouseX += PlayerInputManager.Instance.GetHorizontalMouse() * playerConfig.sensibilityX;
        }

        mouseY -= PlayerInputManager.Instance.GetVerticalMouse() * playerConfig.sensibilityY;
    }

    private void ClampCamera()
    {
        if(!CameraManager.Instance.IsCameraUpdateAllowed()){
            return;
        }else if(wallrunCameraCoroutine != null){
            StopCoroutine(wallrunCameraCoroutine);
            wallrunCameraCoroutine = null;
        }

        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        if (CameraManager.Instance.GetIsClampingCameraHorizontal())
        {
            if (!change)
            {
                change = true;
                print("A true-> " + change);
                if (playerMovement.wallState == PlayerMovement.WallState.rightWall)
                {
                    horizontalClamp = new Vector2(mouseX - 40, mouseX + 5);
                }
                else if (playerMovement.wallState == PlayerMovement.WallState.leftWall)
                {
                    horizontalClamp = new Vector2(mouseX - 5, mouseX + 40);
                }
            }

            mouseX = Mathf.Clamp(mouseX, horizontalClamp.x, horizontalClamp.y);
        }
        else if (change)
        {
            change = false;
            print("A false-> " + change);

        }

        if (PlayerInputManager.Instance.IsHorizontalMouseAllowed)
        {
            playerCamera.transform.eulerAngles = new Vector3(mouseY, mouseX, playerCamera.transform.eulerAngles.z);
        }
        else
        {
            playerCamera.transform.eulerAngles = new Vector3(mouseY, playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.z);
        }

        if (!playerMovement.OnWallrun && PlayerInputManager.Instance.IsHorizontalMouseAllowed)
        {
            transform.eulerAngles = new Vector3(0, playerCamera.transform.rotation.eulerAngles.y, 0);
        }
    }


    public void LerpToWallrunForward(){
        wallrunCameraCoroutine = LerpWallrunCameraToForward();
        StartCoroutine(wallrunCameraCoroutine);        
    }
    
    IEnumerator LerpWallrunCameraToForward(){
        Quaternion newRot = Quaternion.LookRotation(CameraManager.Instance.CameraWallrunLookForward - playerCamera.transform.position, Vector3.up);
        Vector3 newRotEuler = newRot.eulerAngles;
        Quaternion currentRot = playerCamera.transform.rotation;
        Vector3 currentRotEurler = currentRot.eulerAngles;
        float elapsedTime = 0f;
        float lerpTime = .5f;
        while(elapsedTime < lerpTime){
            playerCamera.transform.rotation = Quaternion.Lerp(currentRot, newRot, (elapsedTime / lerpTime));
            transform.eulerAngles = new Vector3(0, playerCamera.transform.rotation.eulerAngles.y, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        wallrunCameraCoroutine = null;
        playerCamera.transform.rotation = newRot;
        CameraManager.Instance.SetIsCameraUpdateAllowed(true);
    }
}
