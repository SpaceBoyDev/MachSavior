using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefabReference;

    private Transform instance;

    private void OnEnable() 
    {
        instance = SpawnPool.Instance.Spawn(prefabReference.transform, this.transform);
    }

    private void OnDisable() 
    {
        Despawn();
        instance = null;
    }

    public void Despawn() 
    {
        if (instance != null && instance.gameObject.activeInHierarchy) {
            instance.parent = null;
            SpawnPool.Instance.Despawn(instance);
        }
    }
}
