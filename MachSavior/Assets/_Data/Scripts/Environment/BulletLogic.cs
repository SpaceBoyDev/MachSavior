using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : PickableObject
{
    [SerializeField] 
    public Vector3 bulletDirection;
    
    [SerializeField] 
    private float bulletSpeed;

    [SerializeField] private float lifeTime;
    [SerializeField] private float currentLifeTime;

    //[SerializeField]
    //private LineRenderer bulletDirectionLine;
    
    //[SerializeField]
    //private float bulletLineRange;
    
    void OnEnable()
    {
        currentLifeTime = 0;
        gameObject.GetComponent<PhysicsTimeObject>().isStopped = true; // reset time logic
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            BulletMovement();
            ChangeBulletDirection();
            
            currentLifeTime += Time.deltaTime;
            
            if (currentLifeTime >= lifeTime)
            {
                Despawn();
                currentLifeTime = 0;
            }
        }
        DirectionRaycast();
    }

    void DirectionRaycast() // A RAYCAST WITH THE DIRECTION OF THE BULLET 
    {
        Debug.DrawRay(transform.position, bulletDirection, Color.green);
        //bulletDirectionLine.SetPosition(1, bulletDirection + (bulletDirection * bulletLineRange));
    }

    void BulletMovement() // BULLET FORWARD MOVEMENT
    {
        if (!IsPicked())
        {
           transform.position += bulletDirection * bulletSpeed * this.GetComponent<PhysicsTimeObject>().slowtime;
        }
    }

    void ChangeBulletDirection() // CHANGE THE BULLET DIRECTION WHEN PICK
    {
        if (IsPicked())
        {
            bulletDirection = Camera.main.transform.forward;
        }
    }

    void Despawn() // DESPAWN BULLET 
    {
        if (!IsPicked())
        {
            if (transform != null && transform.gameObject.activeInHierarchy) 
            {
                transform.parent = null;
                gameObject.SetActive(false);
                SpawnPool.Instance.Despawn(transform);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
}
