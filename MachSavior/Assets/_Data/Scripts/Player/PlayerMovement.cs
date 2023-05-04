using Rewired;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scriptable PlayerConfig")]
    [SerializeField]
    private PlayerConfig playerConfig;

    [Header("References")]
    private Rigidbody rb;
    private GameObject root;

    [Header("Movement bools")]
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private bool onSlope = false;
    private bool canCheckGround = true; //If the raycast can check for ground and slopes

    private Camera playerCamera;

    [Header("Vertical Velocity")]
    private float verticalSpeed = 0;
    private float gravityToApply;

    [Header("Axis")]
    private float horizontalAxis;
    private float verticalAxis;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        root = PlayerManager.Instance.GetRoot();
        playerCamera = CameraManager.Instance.GetPlayerCamera();
        verticalSpeed = 0;
        rb.velocity = -Vector3.up;
    }

    private void Update()
    {
        verticalAxis = PlayerInputManager.Instance.GetVerticalMovement();
        horizontalAxis = PlayerInputManager.Instance.GetHorizontalMovement();

        Move();
        JumpInput();
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        if (canCheckGround)
        {
            isGrounded = PlayerManager.Instance.checkFloor();
            onSlope = PlayerManager.Instance.OnSlope();
        }
        
    }

    private void Move()
    {
        if (onSlope)
        {
            RaycastHit slopeHit = PlayerManager.Instance.GetSlopeHit();
            root.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, slopeHit.normal), slopeHit.normal);
        }
        else
            root.transform.rotation = transform.rotation;


        Vector3 verticalMovement = root.transform.forward * verticalAxis;
        Vector3 horizontalMovement = root.transform.right * horizontalAxis;

        Vector3 combinedMovement = (verticalMovement + horizontalMovement);
        combinedMovement = Mathf.Clamp01(combinedMovement.sqrMagnitude) * combinedMovement.normalized;

        Vector3 moveDirection = new Vector3(combinedMovement.x, verticalSpeed, combinedMovement.z);

        if (onSlope)
            moveDirection = (PlayerManager.Instance.SlopeDirection(moveDirection) * playerConfig.PlayerAcceleration / playerConfig.PlayerSlopeDrag) * Time.fixedDeltaTime;
        else if (isGrounded)
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
        else
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerAirDrag) * Time.fixedDeltaTime;

        rb.velocity = moveDirection;
    }

    private void JumpInput()
    {
        if (PlayerInputManager.Instance.IsJumpDown() && isGrounded || PlayerInputManager.Instance.IsJumpDown() && onSlope)
        {
            isGrounded = false;
            onSlope = false;
            canCheckGround = false;
            StartCoroutine(EnableCheckGround());
            verticalSpeed = playerConfig.JumpForce;
            gravityToApply = playerConfig.Gravity;
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded)
        {
            ResetVerticalSpeed();
            gravityToApply = playerConfig.Gravity;
        }
        else if (onSlope)
        {
            ResetVerticalSpeed();
            gravityToApply = playerConfig.Gravity;
        }
        else 
        {
            if (verticalSpeed >= -playerConfig.JumpHover && verticalSpeed <= playerConfig.JumpHover)
            {
                gravityToApply = playerConfig.Gravity * playerConfig.JumpHoverPercent;
            }
            else if (verticalSpeed < -playerConfig.JumpHover)
                gravityToApply = playerConfig.Gravity;

            if (rb.velocity.y > 0 && !isGrounded && !onSlope && !PlayerInputManager.Instance.IsJumpPressed())
            {
                gravityToApply = playerConfig.Gravity * 3;
            }

            verticalSpeed -= gravityToApply * Time.fixedDeltaTime;
        }
    }

    private void ResetVerticalSpeed()
    {
        verticalSpeed = 0;
    }

    private IEnumerator EnableCheckGround()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        canCheckGround = true;
    }
}
