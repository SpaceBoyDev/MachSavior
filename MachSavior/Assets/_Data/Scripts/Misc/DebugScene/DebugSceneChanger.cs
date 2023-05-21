using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneChanger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        ChangeScene();
    }

    void ChangeScene()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneLoader.Instance.Load(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneLoader.Instance.Load(1);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneLoader.Instance.Load(2);
        }
    }
}
