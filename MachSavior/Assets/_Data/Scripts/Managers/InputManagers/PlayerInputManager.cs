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

    //Player PickObjects
    private const string PICK_OBJECTS = "Pick";
    
    //Player Time Control
    private const string CHANGE_TIME_STATE = "ChangeTimeState";

    private const string SELECT_MODE = "SelectMode";
    private const string SELECT_TIME_OBJECT = "SelectTimeObject";

    private Player playerInput;

    private bool isMovementAllowed = true;
    private bool isJumpAllowed = true;
    private bool isCameraAllowed = true;
    private bool isPickAllowed = true;
    private bool isTimeChangeAllowed = true;

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

    public bool IsJumpDown()
    {
        if (!isJumpAllowed)
            return false;

        return playerInput.GetButtonDown(JUMP);
    }

    public bool IsJumpPressed()
    {
        if (!isJumpAllowed)
            return false;
        
        return playerInput.GetButton(JUMP);
    }

    public bool IsJumpUp()
    {
        if (!isJumpAllowed)
            return false;

        return playerInput.GetButtonUp(JUMP);
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

    public bool IsPickButtonPressed()
    {
        if (!isPickAllowed)
            return false;

        return playerInput.GetButtonDown(PICK_OBJECTS);
    }
    
    //------ TIME SYSTEM ------//
    public bool ChangeTimeState()
    {
        if (!isTimeChangeAllowed)
            return false;

        return playerInput.GetButtonDown(CHANGE_TIME_STATE);
    }

    //-------------------------[SELECT MODE CURRENTLY UNUSED]--------------------------//

    #region SelectMode

    public bool EnterSelectMode()
    {
        return playerInput.GetButton((SELECT_MODE));
    }

    public bool ExitSelectMode()
    {
        return playerInput.GetButtonUp((SELECT_MODE));
    }

    public bool IsSelectTimeObject()
    {
        return playerInput.GetButtonDown(SELECT_TIME_OBJECT);
    }
    

    #endregion
}
