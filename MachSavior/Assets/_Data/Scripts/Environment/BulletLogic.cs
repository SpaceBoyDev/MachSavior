using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class BulletLogic : PickableObject
{
    [SerializeField] 
    public Vector3 bulletDirection;
    
    [SerializeField] 
    private float bulletSpeed;

    [SerializeField] private float lifeTime;
    [SerializeField] private float currentLifeTime;

    [SerializeField] private GameEvent _despawn;

    //[SerializeField]
    //private LineRenderer bulletDirectionLine;
    
    //[SerializeField]
    //private float bulletLineRange;
    
    void OnEnable()
    {
        currentLifeTime = 0;
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
           transform.position += bulletDirection * bulletSpeed * this.GetComponent<PhysicsTimeObject>().slowtime 
                                 *Time.timeScale;
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
                _despawn.Raise();
                transform.parent = null;
                gameObject.SetActive(false);
                SpawnPool.Instance.Despawn(transform);
                
            }
        }
        gameObject.GetComponent<PhysicsTimeObject>().StopTime(); // reset time logic
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
           Despawn();
        }

        if (other.gameObject.tag == "Destructible")
        {
            // TO DO Animacion desaparicion puerta 
            other.gameObject.SetActive(false);
            Despawn();
        }
    }
}
