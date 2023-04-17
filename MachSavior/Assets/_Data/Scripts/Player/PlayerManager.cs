using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField]
    private GameObject root;

    public Camera playerCamera;
    public Transform cameraPos;

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

    public bool checkFloor()
    {
        Debug.DrawLine(root.transform.position, new Vector3(root.transform.position.x, root.transform.position.y - 0.1f, root.transform.position.z));
        return Physics.Raycast(root.transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.2f);
    }
}
