using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject pickPosition;

    [SerializeField] private float distanceToPick;
    private GameObject pickedObject = null;

    private float pickAndReleaseCooldawn;



    // Update is called once per frame
    void Update()
    {
        if (!pickedObject)
            CheckPickObject();
        
        else        
            ReleaseObject();

        if (pickAndReleaseCooldawn > 0)
            pickAndReleaseCooldawn -= 0.3f * Time.deltaTime;
             
    }

    void CheckPickObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distanceToPick))
        {   
            if (hit.collider.gameObject.GetComponent<PickableObject>())
            {
                if (hit.collider.gameObject.GetComponent<PickableObject>().CanPick() && pickedObject == null && pickAndReleaseCooldawn <= 0)
                {
                    if (PlayerInputManager.Instance.IsPickButtonPressed())
                    {
                        pickPosition.transform.position = hit.transform.position;

                        PickObject(hit.collider.gameObject);
                    }
                }

                Debug.Log(hit.collider.name);
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);
            }
            else
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red);
        }

    }

    void PickObject(GameObject objectToPick)
    {
        objectToPick.GetComponent<Rigidbody>().useGravity = false;
        objectToPick.GetComponent<Rigidbody>().isKinematic = true;
        objectToPick.GetComponent<Collider>().isTrigger = true;
        objectToPick.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToPick.transform.position = pickPosition.transform.position;
        objectToPick.transform.SetParent(pickPosition.gameObject.transform);
        pickedObject = objectToPick;

    }

    void ReleaseObject()
    {
        if (PlayerInputManager.Instance.IsPickButtonPressed())
        {
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.GetComponent<Collider>().isTrigger = false;
            
            pickedObject.gameObject.transform.SetParent(null);
            pickedObject = null;
            
            pickAndReleaseCooldawn =+ 0.5f;       
        }
    }
}
