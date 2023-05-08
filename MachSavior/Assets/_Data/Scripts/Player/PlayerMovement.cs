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
    [Header("Raycast Origins")]
    [SerializeField] private Transform[] raycastWallrunOriginsRight;
    [SerializeField] private Transform[] raycastWallrunOriginsLeft;

    private RaycastHit hitWallrun;

    [Header("Axis")]
    private float horizontalAxis;
    private float verticalAxis;

    [Header("Vertical Velocity")]
    private float verticalSpeed = 0;
    private float gravityToApply;

    [Header("Movement bools")]
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private bool onSlope = false;
    [SerializeField]
    private bool onWallrun = false;
    [SerializeField] private bool leftWall = false;
    [SerializeField] private bool rightWall = false;
    private bool canCheckGround = true; //If the raycast can check for ground and slopes

    private Camera playerCamera;

    private int wallNormalMultiplier;

    [Header("Ground Raycasts")]
    [SerializeField]
    [Tooltip("Points of origin for the ground raycast")]
    private Transform[] raycastOrigin;
    [SerializeField]
    int raycastHits = 0;
    [SerializeField]
    private LayerMask groundMask;
    private RaycastHit slopeHit;


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
            isGrounded = checkFloor();
            onSlope = OnSlope();
            WallrunRaycast();
        }
        
    }

    private void Move()
    {
        if (onSlope)
        {
            root.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, slopeHit.normal), slopeHit.normal);
        }
        else if (onWallrun)
        {
            if (rightWall)
                wallNormalMultiplier = -1;
            else wallNormalMultiplier = 1;

            Vector3 wallForward = Vector3.Cross(hitWallrun.normal * wallNormalMultiplier, transform.up);
            Vector3 moveWallrun = (wallForward * verticalAxis * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
            rb.velocity = moveWallrun;
            return;
        }
        else
            root.transform.rotation = transform.rotation;

        Vector3 verticalMovement = root.transform.forward * verticalAxis;
        Vector3 horizontalMovement = root.transform.right * horizontalAxis;

        Vector3 combinedMovement = (verticalMovement + horizontalMovement);
        combinedMovement = Mathf.Clamp01(combinedMovement.sqrMagnitude) * combinedMovement.normalized;

        Vector3 moveDirection = new Vector3(combinedMovement.x, verticalSpeed, combinedMovement.z);

        if (onSlope)
            moveDirection = (SlopeDirection(moveDirection) * playerConfig.PlayerAcceleration / playerConfig.PlayerSlopeDrag) * Time.fixedDeltaTime;
        else if (isGrounded)
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
        else if (!isGrounded)
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerAirDrag) * Time.fixedDeltaTime;

        rb.velocity = moveDirection;
    }
    #region Jump/Gravity
    private void JumpInput()
    {
        if (PlayerInputManager.Instance.IsJumpDown() && isGrounded || PlayerInputManager.Instance.IsJumpDown() && onSlope)
        {
            isGrounded = false;
            onSlope = false;
            canCheckGround = false;
            StartCoroutine(EnableCheckGround());
            gravityToApply = playerConfig.Gravity;
            verticalSpeed = playerConfig.JumpForce;
        }
        else if (PlayerInputManager.Instance.IsJumpDown() && onWallrun)
        {
            isGrounded = false;
            onSlope = false;
            canCheckGround = false;
            onWallrun = false;
            leftWall = false;
            rightWall = false;
            StartCoroutine(EnableCheckGround());
            gravityToApply = playerConfig.Gravity;
            Vector3 wallrunJump = transform.up * playerConfig.JumpForce + (hitWallrun.normal * 20);
            rb.velocity = wallrunJump;
            
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded || onWallrun ||onSlope)
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
    #endregion

    #region Raycast

    public bool checkFloor()
    {
        raycastHits = 0;
        Debug.DrawLine(root.transform.position, new Vector3(root.transform.position.x, root.transform.position.y - 0.3f, root.transform.position.z), Color.red);

        //Slope raycast
        if (Physics.Raycast(root.transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out slopeHit, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            OnSlope();
        }

        //CheckGround raycast
        for (int i = 0; i < raycastOrigin.Length; i++)
        {
            if (Physics.Raycast(raycastOrigin[i].transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.25f, groundMask, QueryTriggerInteraction.Ignore))
                raycastHits++;

            Debug.DrawLine(raycastOrigin[i].transform.position, new Vector3(raycastOrigin[i].transform.position.x, raycastOrigin[i].transform.position.y - 0.1f, raycastOrigin[i].transform.position.z));
        }

        return raycastHits >= playerConfig.MinimumRaycastHits;
    }

    public bool OnSlope()
    {
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle < playerConfig.MaximumSlopeAngle && angle != 0;
    }

    public Vector3 SlopeDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void WallrunRaycast()
    {
        
        if (!isGrounded && !onSlope && rb.velocity.magnitude >= playerConfig.MinimumWallrunVelocity)
        {
            int hits = 0;
            for (int i = 0; i < raycastWallrunOriginsLeft.Length; i++)
            {
                Debug.DrawRay(raycastWallrunOriginsLeft[i].position, -root.transform.right, Color.red);
                if (Physics.Raycast(raycastWallrunOriginsLeft[i].position, -root.transform.right, out hitWallrun, 0.5f))
                {
                    hits++;
                }
            }

            if (hits == raycastWallrunOriginsLeft.Length)
            {
                if (onWallrun)
                {
                    return;
                }
                else if (!Physics.Raycast(root.transform.position, Vector3.down, playerConfig.MinimumWallrunHeight))
                {
                    onWallrun = true;
                    leftWall = true;
                    return;
                }
            }
            else
            {
                onWallrun = false;
                leftWall = false;
                hits = 0;
            }

            for (int i = 0; i < raycastWallrunOriginsRight.Length; i++)
            {
                Debug.DrawRay(raycastWallrunOriginsRight[i].position, root.transform.right, Color.red);
                if (Physics.Raycast(raycastWallrunOriginsRight[i].position, root.transform.right, out hitWallrun, 0.5f))
                {
                    hits++;
                }
            }

            if (hits == raycastWallrunOriginsRight.Length)
            {
                if (onWallrun)
                {
                    return;
                }
                else if (!Physics.Raycast(root.transform.position, Vector3.down, playerConfig.MinimumWallrunHeight))
                {
                    onWallrun = true;
                    rightWall = true;
                    return;
                }
            }
            else
            {
                onWallrun = false;
                rightWall = false;
            }
        }
        else
        {
            onWallrun = false;
            rightWall = false;
            leftWall = false;
        }
    }

    #endregion

    #region Coroutines
    private IEnumerator EnableCheckGround()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        canCheckGround = true;
    }
    #endregion
}
