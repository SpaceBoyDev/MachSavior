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
    private float playerGroundDrag = 1f;
    public float PlayerGroundDrag
    {
        get { return playerGroundDrag; }
    }
    [SerializeField]
    private float playerAirDrag = 3f;
    public float PlayerAirDrag
    {
        get { return playerAirDrag; }
    }

    [SerializeField]
    private float playerSlopeDrag = 1.5f;
    public float PlayerSlopeDrag
    {
        get { return playerSlopeDrag; }
    }
    [SerializeField]
    private float playerMaxHorizontalSpeed = 3f;

    public float PlayerMaxHorizontalSpeed
    {
        get { return playerMaxHorizontalSpeed; }
    }

    [SerializeField]
    private float jumpForce = 10.5f;
    public float JumpForce
    {
        get { return jumpForce; }
    }

    [SerializeField]
    private float maxGravity = -80f;
    public float MaxGravity
    {
        get { return maxGravity; }
    }

    [SerializeField]
    private float gravity = 1f;
    public float Gravity
    {
        get { return gravity; }
    }

    [Header("Camera")]
    public float sensibilityX = 1;
    public float sensibilityY = 1;


}
