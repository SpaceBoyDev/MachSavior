using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : PickableObject
{
    [SerializeField] 
    private Vector3 bulletDirection;
    
    [SerializeField] 
    private float bulletSpeed;

    //[SerializeField]
    //private LineRenderer bulletDirectionLine;
    
    //[SerializeField]
    //private float bulletLineRange;
    
    void Start()
    {
        if (bulletDirection == Vector3.zero)
        {
            bulletDirection = transform.forward;
        }        
    }

    void Update()
    {
        BulletMovement();
        ChangeBulletDirection();

        DirectionRaycast();
    }

    void DirectionRaycast()
    {
        Debug.DrawRay(transform.position, bulletDirection, Color.green);
        //bulletDirectionLine.SetPosition(1, bulletDirection + (bulletDirection * bulletLineRange));
    }

    void BulletMovement()
    {
        if (!IsPicked())
        {
            transform.position += bulletDirection * bulletSpeed * Time.deltaTime;
        }
    }

    void ChangeBulletDirection()
    {
        if (IsPicked())
        {
            bulletDirection = Camera.main.transform.forward;
        }
    }
}
