using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [SerializeField] private GameObject BulletToSpawn;
    [SerializeField] private float TimeToRespawn;
    float RespawnCooldawn = 0;
    
    [HideInInspector]    
    public Transform instance;

    private bool torretIsActive = false;
    
    void Start()
    {
        if (torretIsActive)
        {
            RespawnBullet();
        }
    }

    void Update()
    {
        RespawnCooldawn += Time.deltaTime;
        
        if (torretIsActive)
        {
           RespawnBullet();         
           RespawnCooldawn = 0;
           
        }
    }

    public void RespawnBullet() // SPAWN / RESPAWN BULLET 
    {
        instance = SpawnPool.Instance.Spawn(BulletToSpawn.transform, this.transform);    
        instance.GetComponent<BulletLogic>().bulletDirection = transform.forward;
    }

    public void ActiveTorret()
    {
        torretIsActive = true;
    }
}
