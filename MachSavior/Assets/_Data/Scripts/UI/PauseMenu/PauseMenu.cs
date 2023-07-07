using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using Rewired;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;

    private void Awake()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void OnPause()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);

        if (pauseMenuPanel.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            PlayerInputManager.Instance.IsInputAllowed = false;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerInputManager.Instance.IsInputAllowed = true;
        }
    }
    
    public void OnButtonResume()
    {
        pauseMenuPanel.SetActive(false);
        PlayerInputManager.Instance.IsInputAllowed = true;
        Time.timeScale = 1f;
    }

    public void OnButtonExit()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.Load(0);
        print("Quit app");
    }
}
