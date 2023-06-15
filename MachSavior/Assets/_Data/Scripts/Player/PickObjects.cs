using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField] 
    private Transform[] pickPositions;

    private enum PickObjectWeightPosition
    {   lightWeightPos = 0,
        midWeightPos = 1,
        heavyWeightPos = 2 
    }
    
    [SerializeField] 
    private PickObjectWeightPosition pickObjectWeight;
    
    [SerializeField]
    private GameObject pickObjCollider;

    [SerializeField] 
    private float distanceToPick;
    
    private GameObject _pickedObject = null;

    private float _pickAndReleaseCooldawn;
    
    void Update()
    {
        if (_pickedObject == null)
        {
            CheckPickObject();
        }

        else
        {
            ReleaseObject();
        }      

        if (_pickAndReleaseCooldawn > 0)
            _pickAndReleaseCooldawn -= 0.3f * Time.deltaTime;
             
    }

    #region CheckPick
    void CheckPickObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distanceToPick))
        {   
            if (hit.collider.gameObject.GetComponent<PickableObject>())
            {
                if (hit.collider.gameObject.GetComponent<PickableObject>().CanPick() && !_pickedObject && _pickAndReleaseCooldawn <= 0)
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

        objectToPick.GetComponent<PickableObject>()._isPicked = true;

        if (objectToPick.GetComponent<PickableObject>().UseGravity()) { 
            objectToPick.GetComponent<Rigidbody>().useGravity = false; }
        
        objectToPick.GetComponent<Rigidbody>().isKinematic = true;
        objectToPick.GetComponent<Rigidbody>().velocity = Vector3.zero;

        _pickedObject = objectToPick;
        
        StartCoroutine(PickCourutine(0.1f));
    }

    IEnumerator PickCourutine(float delay)
    { 
        float pickDuration = 0.5f;
        float rotationDuration = 0.1f;
        float startTime = 0f;
        
        while (startTime < pickDuration)
        {
            if (_pickedObject.GetComponent<PickableObject>().ObjectWeight() == 0 && _pickAndReleaseCooldawn <= 0)
            {
                //Position
                 Vector3 startPosition = _pickedObject.transform.position;
                 Vector3 finalPosition = pickPositions[0].position + _pickedObject.GetComponent<PickableObject>().PositionWhenPick();
                 
                 //Rotation
                 Vector3 startRotation = _pickedObject.transform.rotation.eulerAngles;
                 Vector3 finalRotation = _pickedObject.GetComponent<PickableObject>().RotationWhenPick();

                 //Lerps
                _pickedObject.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                pickObjCollider.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                _pickedObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation), (startTime/rotationDuration));
                pickObjCollider.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation), (startTime/rotationDuration));
                
                pickObjCollider.GetComponent<Collider>().isTrigger = false;

                _pickedObject.transform.SetParent(pickPositions[0]);
                startTime += Time.deltaTime;
                yield return null;
            }
            
            if (_pickedObject.GetComponent<PickableObject>().ObjectWeight() == 1 && _pickAndReleaseCooldawn <= 0)
            { 
                //Position
                Vector3 startPosition = _pickedObject.transform.position;
                Vector3 finalPosition = pickPositions[1].position + _pickedObject.GetComponent<PickableObject>().PositionWhenPick();
                 
                //Rotation
                Vector3 startRotation = _pickedObject.transform.rotation.eulerAngles;
                Vector3 finalRotation = _pickedObject.GetComponent<PickableObject>().RotationWhenPick();
                 
                //Lerps
                _pickedObject.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                pickObjCollider.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                _pickedObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation), startTime/rotationDuration);
                pickObjCollider.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation),startTime/rotationDuration);
                
                pickObjCollider.GetComponent<Collider>().isTrigger = false;

                _pickedObject.transform.SetParent(pickPositions[1]);
                startTime += Time.deltaTime;
                yield return null;
            }
            
            if (_pickedObject.GetComponent<PickableObject>().ObjectWeight() == 2 && _pickAndReleaseCooldawn <= 0)
            {
                //Position
                Vector3 startPosition = _pickedObject.transform.position;
                Vector3 finalPosition = pickPositions[2].position + _pickedObject.GetComponent<PickableObject>().PositionWhenPick();
                 
                //Rotation
                Vector3 startRotation = _pickedObject.transform.rotation.eulerAngles;
                Vector3 finalRotation = _pickedObject.GetComponent<PickableObject>().RotationWhenPick();
              
                //Lerps
                _pickedObject.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                pickObjCollider.transform.position = Vector3.Lerp(startPosition, finalPosition, startTime/pickDuration);
                _pickedObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation),startTime/rotationDuration);
                pickObjCollider.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(finalRotation), startTime/rotationDuration);
                
                pickObjCollider.GetComponent<Collider>().isTrigger = false;

                _pickedObject.transform.SetParent(pickPositions[2]);
                startTime += Time.deltaTime;
                yield return null;
            }
        }
    }
    
    #endregion

    #region Release
    void ReleaseObject()
    {
        if (PlayerInputManager.Instance.IsPickButtonPressed())
        {
            _pickedObject.layer = LayerMask.NameToLayer("Default");

            _pickedObject.GetComponent<PickableObject>()._isPicked = false;

            if (_pickedObject.GetComponent<PickableObject>().UseGravity()){
                _pickedObject.GetComponent<Rigidbody>().useGravity = true; }

            _pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickObjCollider.GetComponent<Collider>().isTrigger = true;

            _pickedObject.gameObject.transform.SetParent(null);
            _pickedObject = null;
            
            _pickAndReleaseCooldawn =+ 0.5f;       
        }
    }
    #endregion

}
