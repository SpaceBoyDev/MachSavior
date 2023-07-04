using DG.Tweening;
using Rewired;
using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scriptable PlayerConfig")]
    [SerializeField]
    private PlayerConfig playerConfig;

    [Header("Player References")]
    private Rigidbody rb;
    private GameObject root;

    [Header("Axis")]
    private float horizontalAxis;
    private float verticalAxis;

    [Header("Vertical Velocity")]
    private float verticalSpeed = 0;
    private float xSpeed = 0;
    private float zSpeed = 0;
    [SerializeField] private Vector2 wallrunSpeed = Vector2.zero;
    private float gravityToApply;

    [Header("Movement bools")]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool onSlope = false;
    [SerializeField] private bool onWallrun = false;
    [HideInInspector] public bool OnWallrun { get { return onWallrun; } }

    public enum WallState { none = 0, leftWall = 1, rightWall = 2}
    [SerializeField] public WallState wallState;
    private bool canCheckGround = true; //If the raycast can check for ground and slopes
    private bool canCheckWall = true; //If the raycast can check for walls
    private int wallNormalMultiplier;

    private Camera playerCamera;

    [Header("Ground Raycasts")]
    [SerializeField]
    [Tooltip("Points of origin for the ground raycast")]
    private Transform[] raycastOrigin;
    [SerializeField] int raycastHits = 0;
    [SerializeField] private LayerMask groundMask;
    private RaycastHit slopeHit;
    private RaycastHit centralGroundHit;
    [SerializeField] private float secForEnableGroundCheck;
    [SerializeField] private float secForEnableWallCheck;

    [Header("Raycast Origins")]
    [SerializeField] private Transform[] raycastWallrunOriginsRight;
    [SerializeField] private Transform[] raycastWallrunOriginsLeft;

    private RaycastHit hitWallrun;

    [Header("Step raycasts")]
    [SerializeField] private Transform stepRayUp;
    [SerializeField] private Transform stepRayLow;
    [SerializeField] private float stepOffset;
    [SerializeField] private float stepSmooth;

    [Header("Raycast Colors")]
    [SerializeField] private bool showDebugRay = false;
    [SerializeField] private Color stepUpRaysColor;
    [SerializeField] private Color groundRaysColor;
    [SerializeField] private Color slopeRayColor;
    [SerializeField] private Color wallrunRayColor;

    Vector3 cameraWallrunLookForward;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        root = PlayerManager.Instance.GetRoot();
        playerCamera = CameraManager.Instance.GetPlayerCamera();

        stepRayUp.position = new Vector3(stepRayLow.position.x, stepRayLow.position.y + stepOffset, stepRayLow.position.z);

        verticalSpeed = 0;
        rb.velocity = -Vector3.up;
    }

    private void Update()
    {
        verticalAxis = PlayerInputManager.Instance.GetVerticalMovement();
        horizontalAxis = PlayerInputManager.Instance.GetHorizontalMovement();

        ClampCamera();
        JumpInput();
        ApplyGravity();
        //StepClimb();
        ReduceWallrunSpeed();
    }

    void ClampCamera()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (CameraManager.Instance.GetIsClampingCameraHorizontal())
            {
                CameraManager.Instance.SetIsClampingCameraHorizontal(false);
                return;
            }
            CameraManager.Instance.SetIsClampingCameraHorizontal(true);
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (canCheckGround)
        {
            isGrounded = checkFloor();
            onSlope = OnSlope();
            AlignFloor();
        }

        if (canCheckWall)
            WallrunRaycast();
        
    }

    private void Move()
    {
        if (onSlope)
            root.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, slopeHit.normal), slopeHit.normal);
        else if (onWallrun)
        {
            if (wallState == WallState.rightWall)
                wallNormalMultiplier = -1;
            else wallNormalMultiplier = 1;

            Vector3 wallForward = Vector3.Cross(hitWallrun.normal * wallNormalMultiplier, transform.up);
            Vector3 moveWallrunFinal = (wallForward * verticalAxis * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
            rb.velocity = new Vector3(moveWallrunFinal.x, verticalSpeed, moveWallrunFinal.z);
            
            return;
        }
        else
            root.transform.rotation = transform.rotation;

        Vector3 verticalMovement = root.transform.forward * verticalAxis;
        Vector3 horizontalMovement = root.transform.right * horizontalAxis;

        Vector3 combinedMovement = (verticalMovement + horizontalMovement);
        combinedMovement = Mathf.Clamp01(combinedMovement.sqrMagnitude) * combinedMovement.normalized;

        xSpeed = combinedMovement.x;
        zSpeed = combinedMovement.z;

        Vector3 moveDirection = new Vector3(xSpeed + wallrunSpeed.x, verticalSpeed, zSpeed + wallrunSpeed.y);

        if (onSlope)
            moveDirection = (SlopeDirection(moveDirection) * playerConfig.PlayerAcceleration / playerConfig.PlayerSlopeDrag) * Time.fixedDeltaTime;
        else if (isGrounded)
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
        else if (!isGrounded)
            moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerAirDrag) * Time.fixedDeltaTime;

        rb.velocity = moveDirection;
    }

    private void AlignFloor()
    {
        if (centralGroundHit.transform == null)
            return;

        //root.transform.position = centralGroundHit.point;

        Vector3 alignWithParent = new Vector3(rb.position.x, centralGroundHit.point.y + 1, rb.position.z);
        rb.MovePosition(alignWithParent);

        ResetVerticalSpeed();
        print("alineando");

        //if (transform.parent != slopeHit.transform && slopeHit.transform.tag == ("MovingObject"))
        //{
        //    transform.SetParent(slopeHit.transform);
        //    Vector3 alignWithParent = new Vector3(rb.position.x, centralGroundHit.point.y + 1, rb.position.z);
        //    rb.position = alignWithParent;
        //    print("padreando");
        //    return;
        //}
    }

    private void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLow.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUp.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.7f, groundMask, QueryTriggerInteraction.Ignore))
            {
                print("Step up no angle");
                rb.position -= new Vector3(0f, -stepSmooth, 0f) * Time.deltaTime;
                rb.position += root.transform.forward * Time.deltaTime;
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLow.position, transform.TransformDirection(1.5f,0,1f), out hitLower45, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUp.position, transform.TransformDirection(1.5f, 0, 1f), out hitUpper45, 0.7f, groundMask, QueryTriggerInteraction.Ignore))
            {
                print("Step up 45");
                rb.position -= new Vector3(0f, -stepSmooth, 0f) * Time.deltaTime;
                rb.position += root.transform.forward * Time.deltaTime;
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLow.position, transform.TransformDirection(-1.5f, 0, 1f), out hitLowerMinus45, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUp.position, transform.TransformDirection(-1.5f, 0, 1f), out hitUpperMinus45, 0.7f, groundMask, QueryTriggerInteraction.Ignore))
            {
                print("Step up Minus45");
                rb.position -= new Vector3(0f, -stepSmooth, 0f) * Time.deltaTime;
                rb.position += root.transform.forward * Time.deltaTime;
            }
        }

        RaycastHit hitLowerBack;
        if (Physics.Raycast(stepRayLow.position, transform.TransformDirection(-Vector3.forward), out hitLowerBack, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            RaycastHit hitUpperBack;
            if (!Physics.Raycast(stepRayUp.position, transform.TransformDirection(-Vector3.forward), out hitUpperBack, 0.7f, groundMask, QueryTriggerInteraction.Ignore))
            {
                print("Step up no angle back");
                rb.position -= new Vector3(0f, -stepSmooth, 0f) * Time.deltaTime;
                rb.position += -root.transform.forward * Time.deltaTime;
            }
        }

        if (showDebugRay)
        {
            Debug.DrawRay(stepRayLow.position, transform.TransformDirection(Vector3.forward), stepUpRaysColor, 0.6f);
            Debug.DrawRay(stepRayUp.position, transform.TransformDirection(Vector3.forward), stepUpRaysColor, 0.7f);

            Debug.DrawRay(stepRayLow.position, transform.TransformDirection(1.5f, 0, 1f), stepUpRaysColor, 0.6f);
            Debug.DrawRay(stepRayUp.position, transform.TransformDirection(1.5f, 0, 1f), stepUpRaysColor, 0.7f);

            Debug.DrawRay(stepRayLow.position, transform.TransformDirection(-1.5f, 0, 1f), stepUpRaysColor, 0.6f);
            Debug.DrawRay(stepRayUp.position, transform.TransformDirection(-1.5f, 0, 1f), stepUpRaysColor, 0.7f);
        }
    }

    #region Jump/Gravity
    private void JumpInput()
    {
        if (PlayerInputManager.Instance.IsJumpDown() && isGrounded || PlayerInputManager.Instance.IsJumpDown() && onSlope) //JUMP
        {
            isGrounded = false;
            onSlope = false;
            canCheckGround = false;
            StartCoroutine(EnableCheckGround());
            transform.SetParent(null);
            gravityToApply = playerConfig.Gravity;
            verticalSpeed = playerConfig.JumpForce;
        }
        else if (PlayerInputManager.Instance.IsJumpDown() && onWallrun) //WALLJUMP
        {
            isGrounded = false;
            onSlope = false;
            canCheckGround = false;
            ChangeOnWallrun(false);
            StartCoroutine(EnableCheckGround());
            gravityToApply = playerConfig.Gravity;
            Vector3 tempDirection = transform.up * playerConfig.WallJumpForce + hitWallrun.normal * playerConfig.WallJumpSideForce;
            verticalSpeed = tempDirection.y;
            wallrunSpeed = new Vector2(tempDirection.x, tempDirection.z);
        }
    }

    private void ReduceWallrunSpeed()
    {
        if (wallrunSpeed.x > 0f)
        {
            wallrunSpeed -= new Vector2(1,0) * Time.deltaTime;

            if (wallrunSpeed.x < 0f)
            {
                wallrunSpeed = new Vector2(0f, wallrunSpeed.y);
            }
        }
        else if (wallrunSpeed.x < 0f)
        {
            wallrunSpeed += new Vector2(1, 0) * Time.deltaTime;

            if (wallrunSpeed.x > 0f)
            {
                wallrunSpeed = new Vector2(0f, wallrunSpeed.y);
            }
        }

        if (wallrunSpeed.y > 0f)
        {
            wallrunSpeed -= new Vector2(0, 1) * Time.deltaTime;

            if (wallrunSpeed.y < 0f)
            {
                wallrunSpeed = new Vector2(wallrunSpeed.x, 0f);
            }
        }
        else if (wallrunSpeed.y < 0f)
        {
            wallrunSpeed += new Vector2(0, 1) * Time.deltaTime;

            if (wallrunSpeed.y > 0f)
            {
                wallrunSpeed = new Vector2(wallrunSpeed.x, 0f);
            }
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded || onSlope)
        {
            ResetVerticalSpeed();
            gravityToApply = playerConfig.Gravity;
        }
        else if (onWallrun)
        {
            if (verticalSpeed >= -playerConfig.JumpHoverWallrun && verticalSpeed <= playerConfig.JumpHoverWallrun)
            {
                gravityToApply = playerConfig.Gravity * playerConfig.JumpHoverPercentWallrun;
            }
            else if (verticalSpeed < -playerConfig.JumpHoverWallrun)
            {
                gravityToApply = playerConfig.Gravity;
            }

            verticalSpeed -= gravityToApply * Time.deltaTime;
        }
        else 
        {
            transform.SetParent(null);
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

            verticalSpeed -= gravityToApply * Time.deltaTime;
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

        //Slope raycast
        if (Physics.Raycast(root.transform.position + new Vector3(0, 0.3f, 0), Vector3.down, out slopeHit, 0.9f, groundMask, QueryTriggerInteraction.Ignore))
            OnSlope();
        if (showDebugRay)
            Debug.DrawLine(root.transform.position, new Vector3(root.transform.position.x, root.transform.position.y - 0.3f, root.transform.position.z), slopeRayColor);
        

        //Central ground raycast
        Physics.Raycast(transform.position, Vector3.down, out centralGroundHit, 1.1f, groundMask, QueryTriggerInteraction.Ignore);

            if (showDebugRay)
                Debug.DrawLine(transform.position, transform.position - new Vector3(0, 1.1f, 0), groundRaysColor);
        

        //CheckGround raycast
        for (int i = 0; i < raycastOrigin.Length; i++)
        {
            if (Physics.Raycast(raycastOrigin[i].transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.25f, groundMask, QueryTriggerInteraction.Ignore))
                raycastHits++;
            if (showDebugRay)
            Debug.DrawLine(raycastOrigin[i].transform.position, new Vector3(raycastOrigin[i].transform.position.x, raycastOrigin[i].transform.position.y - 0.1f, raycastOrigin[i].transform.position.z), groundRaysColor);
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

    private void OnEnterWallrun()
    {
        print("onenter wall run");

        if (rb.velocity.y < 0)
        {
            ResetVerticalSpeed();
        }
        else
        {
            verticalSpeed = playerConfig.VerticalSpeedBoost;
            print("boost");
        }

        //Vector3 wallForward = Vector3.Cross(hitWallrun.normal * wallNormalMultiplier, transform.up);
        //cameraWallrunLookForward = transform.position + (wallForward * 2f);
        //CameraManager.Instance.SetCameraWallrunLookForward(cameraWallrunLookForward);
        //CameraManager.Instance.SetIsCameraUpdateAllowed(false);
        //CameraManager.Instance.SetIsClampingCameraHorizontal(true);
        StartCoroutine(OnEnterRotateCamera());
    }

    private IEnumerator OnEnterRotateCamera()
    {
        yield return null;
        yield return null;  //Hay que esperar dos frames para esperar a que se actualicen el hitWallrun y wallNormalMultiplier

        if (hitWallrun.transform != null)
        {
            Vector3 wallForward = Vector3.Cross(hitWallrun.normal * wallNormalMultiplier, transform.up);
            cameraWallrunLookForward = (CameraManager.Instance.GetPlayerCamera().transform.position + (wallForward * 2f));
            CameraManager.Instance.SetCameraWallrunLookForward(cameraWallrunLookForward);
            CameraManager.Instance.SetIsCameraUpdateAllowed(false);
            CameraManager.Instance.SetIsClampingCameraHorizontal(true);
        }
    }

    private void OnExitWallrun()
    {
        print("onexit wall run");
        ResetVerticalSpeed();
        canCheckWall = false;
        StartCoroutine(EnableCheckWall());
        wallState = WallState.none;
        CameraManager.Instance.SetIsCameraUpdateAllowed(true);
        CameraManager.Instance.SetIsClampingCameraHorizontal(false);
    }

    private void ChangeOnWallrun(bool state)
    {
        if (state == onWallrun)
        {
            return;
        }
        else if (state == false && onWallrun == true)
        {
            OnExitWallrun();
        }
        else if (state == true && onWallrun == false)
        {
            OnEnterWallrun();
        }

        onWallrun = state;
    }

    private void WallrunRaycast()
    {
        
        if (!isGrounded && !onSlope && verticalAxis > 0.9f)
        {
            //Check left side
            int hits = 0;
            for (int i = 0; i < raycastWallrunOriginsLeft.Length; i++)
            {
                if (showDebugRay)
                {
                    Debug.DrawRay(raycastWallrunOriginsLeft[i].position, -root.transform.right, wallrunRayColor);
                }

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
                    wallrunSpeed = Vector2.zero;
                    ChangeOnWallrun(true);
                    wallState = WallState.leftWall;

                    return;
                }
            }
            else
            {

                //Check right side
                hits = 0;
                for (int i = 0; i < raycastWallrunOriginsRight.Length; i++)
                {
                    if (showDebugRay)
                    {
                        Debug.DrawRay(raycastWallrunOriginsRight[i].position, root.transform.right, wallrunRayColor);
                    }

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
                        wallrunSpeed = Vector2.zero;
                        ChangeOnWallrun(true);
                        wallState = WallState.rightWall;

                        return;
                    }
                    else
                    {
                        ChangeOnWallrun(false);
                    }
                }
                else
                {
                    ChangeOnWallrun(false);
                }
            }
        }
        else
        {
            ChangeOnWallrun(false);
        }
    }

    #endregion

    #region Coroutines
    private IEnumerator EnableCheckGround()
    {
        yield return new WaitForSecondsRealtime(secForEnableGroundCheck);
        canCheckGround = true;
    }

    private IEnumerator EnableCheckWall()
    {
        yield return new WaitForSecondsRealtime(secForEnableWallCheck);
        canCheckWall = true;
    }
    #endregion
}
