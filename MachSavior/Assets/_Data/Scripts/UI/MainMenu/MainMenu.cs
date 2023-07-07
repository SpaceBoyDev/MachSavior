using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnButtonPlay()
    {
        SceneLoader.Instance.Load(1);
    }
    
    public void OnButtonOptions()
    {
        
    }
    
    public void OnButtonExit()
    {
        Application.Quit();
        print("Quit app");
    }
}
