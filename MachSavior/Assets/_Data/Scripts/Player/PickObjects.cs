using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField]
    private Transform[] pickPositions;

    private enum PickObjectWeightPosition { lightWeightPos = 0, midWeightPos = 1, heavyWeightPos = 2 }
    [SerializeField] private PickObjectWeightPosition pickObjectWeight;


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

    #region CheckPick
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
    #endregion

    #region Pick
    void PickObject(GameObject objectToPick)
    {
        objectToPick.layer = LayerMask.NameToLayer("IgnorePlayer");

        objectToPick.GetComponent<Rigidbody>().useGravity = false;
        objectToPick.GetComponent<Rigidbody>().isKinematic = true;
        objectToPick.GetComponent<Rigidbody>().velocity = Vector3.zero;

        pickedObject = objectToPick;


        // TO DO ANIMACIÓN COGER OBJETO

        // Delay se puede usar para posteriormente cuando en cierto momento de la animación
        // el objeto se attache a la mano o al cuerpo

        StartCoroutine(PickCourutine(0.1f));
    }

    IEnumerator PickCourutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (pickedObject.GetComponent<PickableObject>().ObjectWeight() == 0)
        {
            pickedObject.transform.position = pickPositions[0].position;
            pickObjCollider.transform.position = pickPositions[0].position;
            pickObjCollider.GetComponent<Collider>().isTrigger = false;

            pickedObject.transform.SetParent(pickPositions[0]);
        }
        
        if (pickedObject.GetComponent<PickableObject>().ObjectWeight() == 1)
        {
            pickedObject.transform.position = pickPositions[1].position;
            pickObjCollider.transform.position = pickPositions[1].position;
            pickObjCollider.GetComponent<Collider>().isTrigger = false;

            pickedObject.transform.SetParent(pickPositions[1]);
        }
        
        if (pickedObject.GetComponent<PickableObject>().ObjectWeight() == 2)
        {
            pickedObject.transform.position = pickPositions[2].position;
            pickObjCollider.transform.position = pickPositions[2].position;
            pickObjCollider.GetComponent<Collider>().isTrigger = false;

            pickedObject.transform.SetParent(pickPositions[2]);
        }
    }

    #endregion

    #region Release
    void ReleaseObject()
    {
        if (PlayerInputManager.Instance.IsPickButtonPressed())
        {
            pickedObject.layer = LayerMask.NameToLayer("Default");

            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickObjCollider.GetComponent<Collider>().isTrigger = true;

            pickedObject.gameObject.transform.SetParent(null);
            pickedObject = null;
            
            pickAndReleaseCooldawn =+ 0.5f;       
        }
    }
    #endregion

}
