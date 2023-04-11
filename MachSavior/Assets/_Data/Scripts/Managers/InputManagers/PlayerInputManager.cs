using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    //Input actions
    private const string VERTICAL_MOVEMENT = "VerticalMovement";
    private const string HORIZONTAL_MOVEMENT = "HorizontalMovement";
    private const string JUMP = "Jump";

    //Camera mouse
    private const string VERTICAL_MOUSE = "VerticalMouse";
    private const string HORIZONTAL_MOUSE = "HorizontalMouse";

    private Player playerInput;

    private bool isMovementAllowed = true;
    private bool isJumpAllowed = true;
    private bool isCameraAllowed = true;

    public bool IsMovementAllowed
    {
        set { isMovementAllowed = value; }
    }

    public bool IsJumpAllowed
    {
        set { isJumpAllowed = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerInput = ReInput.players.GetPlayer(0);
    }

    public float GetVerticalMovement()
    {
        if (!isMovementAllowed)
            return 0f;

        return playerInput.GetAxis(VERTICAL_MOVEMENT);
    }

    public float GetHorizontalMovement()
    {
        if (!isMovementAllowed)
            return 0f;

        return playerInput.GetAxis(HORIZONTAL_MOVEMENT);
    }

    public bool IsJumpPressed()
    {
        if (!isJumpAllowed)
            return false;

        return playerInput.GetButtonDown(JUMP);
    }

    public float GetVerticalMouse()
    {
        if (!isCameraAllowed)
            return 0f;

        return playerInput.GetAxis(VERTICAL_MOUSE);
    }

    public float GetHorizontalMouse()
    {
        if (!isCameraAllowed)
            return 0f;

        return playerInput.GetAxis(HORIZONTAL_MOUSE);
    }
}
