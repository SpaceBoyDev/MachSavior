using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    private void Awake()
    {
        LoadUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadUI();
        }
    }

    private void LoadUI()
    {
        if (SceneManager.GetSceneByName("UI").isLoaded == false)
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        else
            SceneManager.UnloadSceneAsync("UI");
    }
}
