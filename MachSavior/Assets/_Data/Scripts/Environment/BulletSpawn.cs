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

        RespawnCooldawn += TimeToRespawn;
    }

    void Update()
    {
        RespawnCooldawn += Time.deltaTime;
        
        if (torretIsActive && RespawnCooldawn >= TimeToRespawn)
        {
           RespawnBullet();
        }
    }

    public void RespawnBullet() // SPAWN / RESPAWN BULLET 
    {
        if (torretIsActive)
        {
            instance = SpawnPool.Instance.Spawn(BulletToSpawn.transform, this.transform);    
            instance.GetComponent<BulletLogic>().bulletDirection = transform.forward;
            RespawnCooldawn = 0;
        }
    }

    public void ActiveTorret()
    {
        torretIsActive = true;
    }
    public void DesactiveTorret()
    {
        torretIsActive = false;
        RespawnCooldawn = TimeToRespawn;
    }
}
