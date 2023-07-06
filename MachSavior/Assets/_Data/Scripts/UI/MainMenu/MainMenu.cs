using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
