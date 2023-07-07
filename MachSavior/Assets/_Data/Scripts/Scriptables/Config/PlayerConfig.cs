using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfiguration", menuName = "MachSavior/Player/PlayerConfiguration", order = 1)]
public class PlayerConfig : ScriptableObject
{
    #region Movement
    [Header("Movement")]
    [SerializeField]
    [Tooltip("Player Acceleration")]
    private float playerAcceleration = 500f;
    public float PlayerAcceleration
    {
        get { return playerAcceleration; }
    }
    [SerializeField]
    [Tooltip("Movement is divided by this amount when on ground")]
    [Range(1f, 2f)]
    private float playerGroundDrag = 1f;
    public float PlayerGroundDrag
    {
        get { return playerGroundDrag; }
    }
    [SerializeField]
    [Tooltip("Movement is divided by this amount when not grounded")]
    [Range(1f, 2f)]
    private float playerAirDrag = 3f;
    public float PlayerAirDrag
    {
        get { return playerAirDrag; }
    }
    
    [SerializeField]
    [Tooltip("Gravity at which additionalSpeed will decay")]
    private float additionalSpeedGravity = 3f;
    public float AdditionalSpeedGravity
    {
        get { return additionalSpeedGravity; }
    }

    #endregion
    #region Slopes
    [Header("Slopes")]
    [SerializeField]
    [Tooltip("Movement is divided by this amount when on slope")]
    [Range(1f, 2f)]
    private float playerSlopeDrag = 1f;
    public float PlayerSlopeDrag
    {
        get { return playerSlopeDrag; }
    }

    [SerializeField]
    [Tooltip("Maximum slope angle the player can walk on")]
    [Range(1f, 45f)]
    private float maximumSlopeAngle = 35f;
    public float MaximumSlopeAngle
    {
        get { return maximumSlopeAngle; }
    }

    [SerializeField]
    [Tooltip("Minimum number of raycast hits needed for the player to consider itself grounded")]
    [Range(1f, 9f)]
    private int minimumRaycastHits = 1;
    public float MinimumRaycastHits
    {
        get { return minimumRaycastHits; }
    }

    #endregion
    #region Wallrun Movement

    [Header("Wallrun Movement")]
    [SerializeField]
    [Tooltip("Movement is divided by this amount when wallrunning")]
    [Range(1f, 2f)]
    private float playerWallrunDrag = 1.2f;
    public float PlayerWallrunDrag
    {
        get { return playerWallrunDrag; }
    }

    [SerializeField]
    [Tooltip("Vertical jump force when walljumping")]
    [Range(0f, 4f)]
    private float wallJumpForce = 3f;
    public float WallJumpForce
    {
        get { return wallJumpForce; }
    }

    [SerializeField]
    [Tooltip("Multiplier applied to the wall's normal when walljumping")]
    [Range(0f, 2f)]
    private float wallJumpSideForce = 1f;
    public float WallJumpSideForce
    {
        get { return wallJumpForce; }
    }

    [SerializeField]
    [Tooltip("Boost applied to vertical speed when entering wallrun")]
    [Range(0f, 5f)]
    private float verticalSpeedBoost = 2f;
    public float VerticalSpeedBoost
    {
        get { return wallJumpForce; }
    }

    [SerializeField]
    [Tooltip("Minimum rigidbody velocity required to start wallrunning")]
    [Range(0f, 10f)]
    private float minimumWallrunVelocity = 5f;
    public float MinimumWallrunVelocity
    {
        get { return minimumWallrunVelocity; }
    }

    [SerializeField]
    [Tooltip("Minimum height from ground required to start wallrunning")]
    [Range(0f, 10f)]
    private float minimumWallrunHeight = 2f;
    public float MinimumWallrunHeight
    {
        get { return minimumWallrunHeight; }
    }

    [SerializeField]
    [Range(0.01f, 0.7f)]
    private float jumpHoverWallrun = 0.1f; //When it has to be applied the jumpHoverPercent (between 0.1 and -0.1 rb.velocity.y)
    public float JumpHoverWallrun
    {
        get { return jumpHoverWallrun; }
    }
    [SerializeField]
    [Range(0.01f, 0.99f)]
    private float jumpHoverPercentWallrun = 0.3f; //Percent of the gravity that will be applied when climax of the jump
    public float JumpHoverPercentWallrun
    {
        get { return jumpHoverPercentWallrun; }
    }

    #endregion
    #region Jump/Gravity

    [Header("Jump/Gravity")]
    [SerializeField]
    private float jumpForce = 1.5f; //Initial force applied when jumping
    public float JumpForce
    {
        get { return jumpForce; }
    }
    [SerializeField]
    [Range(0.01f, 0.7f)]
    private float jumpHover = 0.1f; //When it has to be applied the jumpHoverPercent (between 0.1 and -0.1 rb.velocity.y)
    public float JumpHover
    {
        get { return jumpHover; }
    }
    [SerializeField]
    [Range(0.1f, 0.99f)]
    private float jumpHoverPercent = 0.3f; //Percent of the gravity that will be applied when climax of the jump
    public float JumpHoverPercent
    {
        get { return jumpHoverPercent; }
    }

    [SerializeField]
    [Range(-100f, -50f)]
    private float maxGravity = -80f; //Max gravity (UNUSED FOR NOW)
    public float MaxGravity
    {
        get { return maxGravity; }
    }

    [SerializeField]
    [Range(1f,20f)]
    private float gravity = 1f; //Initial gravity to apply
    public float Gravity
    {
        get { return gravity; }
    }

    #endregion
    #region Camera

    [Header("Camera")]
    public float sensibilityX = 1;
    public float sensibilityY = 1;

    #endregion
}
