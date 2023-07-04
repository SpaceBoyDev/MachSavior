using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour {

    [System.Serializable]
    public class PrefabPool {
        public Transform prefab;
        public GameObject prefabGO;
        public int preloadAmount;

        private HashSet<Transform> spawnedObjects = new HashSet<Transform>();
        private HashSet<Transform> despawnedObjects = new HashSet<Transform>();

        private bool preload = false;

        public PrefabPool(Transform prefab) {
            this.prefab = prefab;
            prefabGO = prefab.gameObject;
        }

        public Transform SpawnInstance(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null) {
            Transform instance;

            if (despawnedObjects.Count == 0) {
                instance = SpawnNew(pos, rot, parent);
            } else {
                HashSet<Transform>.Enumerator despawnedEnumerator = this.despawnedObjects.GetEnumerator();
                despawnedEnumerator.MoveNext();
                instance = despawnedEnumerator.Current;

                despawnedObjects.Remove(instance);
                spawnedObjects.Add(instance);

                instance.parent = parent;
                if (parent != null) {
                    instance.localPosition = pos;
                    instance.localRotation = rot;
                } else {
                    instance.SetPositionAndRotation(pos, rot);
                }
                instance.localScale = scale;
            }

            instance.gameObject.SetActive(true);
            return instance;
        }

        public Transform SpawnNew(Vector3 pos, Quaternion rot, Transform parent = null) {
            GameObject instGO;
            instGO = GameObject.Instantiate(this.prefabGO, pos, rot, parent);
            if (parent != null) {
                instGO.transform.localPosition = pos;
                instGO.transform.localRotation = rot;
            }
            Transform inst = instGO.transform;

            // Start tracking the new instance
            this.spawnedObjects.Add(inst);

            return inst;
        }

        public void DespawnInstance(Transform trans) {
            this.spawnedObjects.Remove(trans);
            this.despawnedObjects.Add(trans);
            trans.parent = null;
            trans.gameObject.SetActive(false);
        }

        public void PreloadInstances() {
            if (!preload) {

                preload = true;

                spawnedObjects = new HashSet<Transform>();
                despawnedObjects = new HashSet<Transform>();

                for (int i = 0; i < preloadAmount; i++) {
                    Transform trans = SpawnNew(Vector3.zero, Quaternion.identity);
                    DespawnInstance(trans);
                }
            }
        }

    }


    public static SpawnPool Instance;

    [SerializeField]
    private List<PrefabPool> prefabPools = new List<PrefabPool>();
    public List<PrefabPool> PrefabPools {
        get {
            return prefabPools;
        }
    }

    private Dictionary<GameObject, PrefabPool> _prefabToPoolDict = new Dictionary<GameObject, PrefabPool>();
    private Dictionary<Transform, PrefabPool> _spawned = new Dictionary<Transform, PrefabPool>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != null) {
            Destroy(this);
        }

        //Initialization 
        for (int i = 0; i < prefabPools.Count; i++) {
            PrefabPool p = prefabPools[i];
            p.PreloadInstances();
            if (!this._prefabToPoolDict.ContainsKey(p.prefabGO)) {
                this._prefabToPoolDict.Add(p.prefabGO, p);
            }
        }
    }

    public Transform Spawn(Transform prefab, Transform parent) 
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, Vector3.one, parent);
    }

    public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null) {
        Transform inst;
        PrefabPool prefabPool;
        if (_prefabToPoolDict.TryGetValue(prefab.gameObject, out prefabPool)) {
            inst = prefabPool.SpawnInstance(pos, rot, scale, parent);
            this._spawned.Add(inst, prefabPool);
            return inst;
        }

        PrefabPool newPrefabPool = new PrefabPool(prefab);
        CreatePrefabPool(newPrefabPool);

        inst = newPrefabPool.SpawnInstance(pos, rot, scale, parent);

        this._spawned.Add(inst, newPrefabPool);

        return inst;
    }

    public void CreatePrefabPool(PrefabPool prefabPool) {
        this._prefabToPoolDict.Add(prefabPool.prefabGO, prefabPool);
    }

    public void Despawn(Transform instance) 
    {
        
        PrefabPool prefabPool;
        
        if (this._spawned.TryGetValue(instance, out prefabPool)) 
        {
            prefabPool.DespawnInstance(instance);
        }

        _spawned.Remove(instance);

    }
}
