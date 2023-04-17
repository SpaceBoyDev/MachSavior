using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfiguration", menuName = "MachSavior/Player/PlayerConfiguration", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]

    [SerializeField]
    private float playerAcceleration = 12f;
    public float PlayerAcceleration
    {
        get { return playerAcceleration; }
    }

    private float playerGroundDrag = 3f;
    public float PlayerGroundDrag
    {
        get { return playerGroundDrag; }
    }

    private float playerAirDrag = 3f;
    public float PlayerAirDrag
    {
        get { return playerAirDrag; }
    }

    private float playerMaxHorizontalSpeed = 3f;

    public float PlayerMaxHorizontalSpeed
    {
        get { return playerMaxHorizontalSpeed; }
    }

    [SerializeField]
    private float jumpForce = 3f;
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
    private float gravity = 3f;
    public float Gravity
    {
        get { return gravity; }
    }

    [Header("Camera")]
    public float sensibilityX = 1;
    public float sensibilityY = 1;


}
