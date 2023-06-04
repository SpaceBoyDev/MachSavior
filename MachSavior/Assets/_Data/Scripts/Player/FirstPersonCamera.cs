using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig playerConfig;

    private Camera playerCamera;

    private float mouseX, mouseY;

    private int maxY = 89;
    private int minY = -89;
    private int maxX = 89;
    private int minX = -89;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = CameraManager.Instance.GetPlayerCamera();

    }

    // Update is called once per frame
    void Update()
    {

        mouseX += PlayerInputManager.Instance.GetHorizontalMouse() * playerConfig.sensibilityX;
        mouseY -= PlayerInputManager.Instance.GetVerticalMouse() * playerConfig.sensibilityY;

        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        if (CameraManager.Instance.clampCameraHorizontal)
        {
            //mouseX = Mathf.Clamp(mouseX, minX, maxX);
        }

        playerCamera.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);

        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }
}
