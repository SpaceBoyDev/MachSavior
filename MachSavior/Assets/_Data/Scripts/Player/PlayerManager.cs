using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Transform[] raycastOrigin;
    [SerializeField]
    private LayerMask groundMask;
    private RaycastHit slopeHit;
    public float maxSlopeAngle = 40f;

    public Camera playerCamera;
    public Transform cameraPos;

    [SerializeField]
    int raycastHits = 0;

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
        raycastHits = 0;
        Debug.DrawLine(root.transform.position, new Vector3(root.transform.position.x, root.transform.position.y - 0.3f, root.transform.position.z), Color.red);

        //Slope raycast
        if (Physics.Raycast(root.transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out slopeHit, 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            OnSlope();
        }

        //CheckGround raycast
        for (int i = 0; i < raycastOrigin.Length; i++)
        {
            if (Physics.Raycast(raycastOrigin[i].transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.25f, groundMask, QueryTriggerInteraction.Ignore))
                raycastHits++;

            Debug.DrawLine(raycastOrigin[i].transform.position, new Vector3(raycastOrigin[i].transform.position.x, raycastOrigin[i].transform.position.y - 0.1f, raycastOrigin[i].transform.position.z));
        }

        return raycastHits >= 1;
    }

    public bool OnSlope()
    {
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;
    }

    public Vector3 SlopeDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    public GameObject GetRoot()
    {
        return root;
    }

    public RaycastHit GetSlopeHit()
    {
        return slopeHit;
    }
}
