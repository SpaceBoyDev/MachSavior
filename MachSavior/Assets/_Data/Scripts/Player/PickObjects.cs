using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject pickPosition;

    [SerializeField]
    private GameObject pickObjCollider;

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
        objectToPick.layer = LayerMask.NameToLayer("IgnorePlayer");

        objectToPick.GetComponent<Rigidbody>().useGravity = false;
        objectToPick.GetComponent<Rigidbody>().isKinematic = true;
        objectToPick.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // TO DO Cambio de textura
        //objectToPick.GetComponent<Material>().color = pickedObject.GetComponent<PickableObject>().Active.color;

        objectToPick.transform.position = pickPosition.transform.position;
        pickObjCollider.transform.position = pickPosition.transform.position;
        pickObjCollider.GetComponent<Collider>().isTrigger = false;

        objectToPick.transform.SetParent(pickPosition.gameObject.transform);
        pickedObject = objectToPick;

    }

    void ReleaseObject()
    {
        if (PlayerInputManager.Instance.IsPickButtonPressed())
        {
            pickedObject.layer = LayerMask.NameToLayer("Default");

            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickObjCollider.GetComponent<Collider>().isTrigger = true;

            // TO DO Cambio de textura
            //pickedObject.GetComponent<Material>().color = pickedObject.GetComponent<PickableObject>().CanInteraccion.color;


            pickedObject.gameObject.transform.SetParent(null);
            pickedObject = null;
            
            pickAndReleaseCooldawn =+ 0.5f;       
        }
    }
}
