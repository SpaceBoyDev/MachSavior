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
            RespanwBullet();
        }
    }

    void Update()
    {
        
        if (torretIsActive)
        {
            RespawnCooldawn += Time.deltaTime;
            
            if (RespawnCooldawn >= TimeToRespawn)
            {
                RespanwBullet(); 
                RespawnCooldawn = 0;
            }
        }
    }

    void RespanwBullet() // SPAWN / RESPAWN BULLET 
    {

        instance = SpawnPool.Instance.Spawn(BulletToSpawn.transform, this.transform);    
        instance.GetComponent<BulletLogic>().bulletDirection = transform.forward;
    }

    public void ActiveTorret()
    {
        torretIsActive = true;
    }
}
