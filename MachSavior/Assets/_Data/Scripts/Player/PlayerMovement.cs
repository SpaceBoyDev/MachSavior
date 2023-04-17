using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig playerConfig;

    private Rigidbody rb;

    [SerializeField]
    private bool isGrounded = false;

    private Camera playerCamera;

    private float horizontalAxis;
    private float verticalAxis;

    private float playerVelocityY = 0;

    public float gravityToApply;
    [SerializeField]
    float gravityAcceleration = 0;
    float gravityDrag = 1.5f;

    public float jumpForceToApply;
    private float jumpAcceleration = 1;

    private bool canCheckGround = true;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        playerCamera = CameraManager.Instance.GetPlayerCamera();
        //Initialize game in stop time.
        TimeManager.Instance.StopTime();
    }

    private void Update()
    {
        verticalAxis = PlayerInputManager.Instance.GetVerticalMovement();
        horizontalAxis = PlayerInputManager.Instance.GetHorizontalMovement();

        Move();
        JumpInput();
    }
 
    private void FixedUpdate()
    {
        if (canCheckGround)
        {
            if (PlayerManager.Instance.checkFloor())
                isGrounded = true;
            else
                isGrounded = false;
        }

        if (!isGrounded)
            ApplyGravity();
        else
        {
            gravityAcceleration = 0;
            gravityToApply = 0;
            playerVelocityY = 0;
        }
    }

    private void Move()
    {
        Vector3 verticalMovement = rb.transform.forward * verticalAxis;
        Vector3 horizontalMovement = rb.transform.right * horizontalAxis;
        
        
        Vector3 combinedMovement = verticalMovement + horizontalMovement;
        combinedMovement = Mathf.Clamp01(combinedMovement.sqrMagnitude) * combinedMovement.normalized;
        
        Vector3 moveDirection = new Vector3(combinedMovement.x, playerVelocityY, combinedMovement.z);
        moveDirection = (moveDirection * playerConfig.PlayerAcceleration / playerConfig.PlayerGroundDrag) * Time.fixedDeltaTime;
        
        rb.velocity = moveDirection;

    }

    private void JumpInput()
    {
        if (PlayerInputManager.Instance.IsJumpPressed() && isGrounded)
        {
            isGrounded = false;
            canCheckGround = false;
            jumpForceToApply = playerConfig.JumpForce;
            jumpAcceleration++;
            playerVelocityY = playerConfig.JumpForce;
            print("Salto");
            StartCoroutine(EnableCheckGround());
        }
    }

    private void ApplyGravity()
    {
        gravityToApply = playerConfig.Gravity + gravityAcceleration;

        if (rb.velocity.y <= playerConfig.MaxGravity)
        {
            //playerVelocityY = playerConfig.MaxGravity;
            rb.velocity = new Vector3(rb.velocity.x, playerConfig.MaxGravity, rb.velocity.z);
        }
        else
        {
            //gravityAcceleration += 0.3f;
            playerVelocityY -= gravityToApply / gravityDrag;
        }

        //rb.velocity = new Vector3(rb.velocity.x, gravity, rb.velocity.z);
    }

    private IEnumerator EnableCheckGround()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        canCheckGround = true;
    }
}
