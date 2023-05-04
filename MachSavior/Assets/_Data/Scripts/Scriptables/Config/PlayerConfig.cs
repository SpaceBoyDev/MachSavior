using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfiguration", menuName = "MachSavior/Player/PlayerConfiguration", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private float playerAcceleration = 500f;
    public float PlayerAcceleration
    {
        get { return playerAcceleration; }
    }
    [SerializeField]
    [Range(1f, 2f)]
    private float playerGroundDrag = 1f;
    public float PlayerGroundDrag
    {
        get { return playerGroundDrag; }
    }
    [SerializeField]
    [Range(1f, 2f)]
    private float playerAirDrag = 3f;
    public float PlayerAirDrag
    {
        get { return playerAirDrag; }
    }

    [SerializeField]
    [Range(1f, 2f)]
    private float playerSlopeDrag = 1.5f;
    public float PlayerSlopeDrag
    {
        get { return playerSlopeDrag; }
    }

    [Header("Jump/Gravity")]
    [SerializeField]
    private float jumpForce = 1.5f; //Initial force applied when jumping
    public float JumpForce
    {
        get { return jumpForce; }
    }
    [SerializeField]
    [Range(0.1f, 0.7f)]
    private float jumpHover = 0.1f; //When it has to be applied the jumpHoverPercent (between 0.1 and -0.1 rb.velocity.y)
    public float JumpHover
    {
        get { return jumpHover; }
    }
    [SerializeField]
    [Range(0.1f, 0.7f)]
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
    [Range(1f,5f)]
    private float gravity = 1f; //Initial gravity to apply
    public float Gravity
    {
        get { return gravity; }
    }

    [Header("Camera")]
    public float sensibilityX = 1;
    public float sensibilityY = 1;


}
