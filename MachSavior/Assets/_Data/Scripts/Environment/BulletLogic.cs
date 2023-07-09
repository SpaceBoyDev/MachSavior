using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using Unity.VisualScripting;
using UnityEngine;

public class BulletLogic : PickableObject
{
    [SerializeField] public Vector3 bulletDirection;
    [SerializeField] private BulletSpawn bulletSpawn;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameEvent _despawn;

    [SerializeField] private PhysicsTimeObject physicsTimeObject;
    
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            BulletMovement();
            ChangeBulletDirection();
            
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
            gameObject.SetActive(false);
            bulletSpawn.RespawnBullet();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
           Despawn();
        }

        if (other.gameObject.tag == "Destructible")
        {
            if (GetMyTimeState() == false)
            {
                // TO DO Animacion desaparicion puerta 
                other.gameObject.SetActive(false);

            }

            Despawn();
        }
        gameObject.GetComponent<PhysicsTimeObject>().StopTime(); // reset time logic

    }

    bool GetMyTimeState()
    {
        return physicsTimeObject.isStopped;
    }
}
