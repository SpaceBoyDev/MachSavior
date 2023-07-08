using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [SerializeField] private GameObject BulletToSpawn;
    
    [SerializeField] private bool torretIsActive = false;
    
    void Start()
    {
        
        if (torretIsActive)
        {
            RespawnBullet();
        }

    }
    
    public void RespawnBullet() // SPAWN / RESPAWN BULLET 
    {
        if (torretIsActive && BulletToSpawn.activeInHierarchy == false)
        {
            BulletToSpawn.transform.position = transform.position;
            BulletToSpawn.SetActive(true);
            BulletToSpawn.GetComponent<Rigidbody>().isKinematic = false;
            
            BulletToSpawn.GetComponent<BulletLogic>().bulletDirection = transform.forward;
        }
    }

    public void ActiveTorret()
    {
        torretIsActive = true;
    }
    public void DesactiveTorret()
    {
        torretIsActive = false;
    }
}
