using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectPool : CustomBehaviour
{
    [Serializable]
    public class Pool
    {
        public GameObject PooledPrefab;
        public int SpawnedSize = 1;
        public DeactiveParents PrefabParent;
    }
    [SerializeField] private List<Pool> m_Pools = new List<Pool>();
    public Dictionary<PooledObjectType, Queue<IPooledObject>> m_poolDictionary;

    private IPooledObject m_SpawnOnPool;
    private IPooledObject m_TempSpawned;

    public override void Initialize()
    {
        m_poolDictionary = new Dictionary<PooledObjectType, Queue<IPooledObject>>();

        for (int i = 0; i < m_Pools.Count; i++)
        {
            Queue<IPooledObject> activeOnPool = new Queue<IPooledObject>();
            Queue<IPooledObject> objectsOnPool = new Queue<IPooledObject>();

            for (int j = 0; j < m_Pools[i].SpawnedSize; j++)
            {
                m_TempSpawned = Instantiate(m_Pools[i].PooledPrefab, GameManager.Instance.Entities.GetDeactiveParent(m_Pools[i].PrefabParent)).GetComponent<IPooledObject>();
                m_TempSpawned.GetGameObject().gameObject.SetActive(false);
                m_TempSpawned.GetGameObject().Initialize();
                m_TempSpawned.SetDeactiveParent(m_Pools[i].PrefabParent);
                objectsOnPool.Enqueue(m_TempSpawned);
            }
            m_poolDictionary.Add(m_Pools[i].PooledPrefab.GetComponent<PooledObject>().PooledObjectType, objectsOnPool);

        }
    }
    public IPooledObject SpawnFromPool(PooledObjectType _prefabType,
                                    Vector3 _position = new Vector3(),
                                    Quaternion _rotation = new Quaternion(),
                                    Transform _parent = null)
    {
        if (!m_poolDictionary.ContainsKey(_prefabType))
        {
            return null;
        }

        if (m_poolDictionary[_prefabType].Count > 0)
        {
            m_SpawnOnPool = m_poolDictionary[_prefabType].Dequeue();
        }
        else
        {
            for (int i = m_Pools.Count - 1; i >= 0; i--)
            {
                if (m_Pools[i].PooledPrefab.GetComponent<PooledObject>().PooledObjectType == _prefabType)
                {
                    m_SpawnOnPool = Instantiate(m_Pools[i].PooledPrefab, GameManager.Instance.Entities.GetDeactiveParent(m_Pools[i].PrefabParent)).GetComponent<IPooledObject>();
                    m_SpawnOnPool.GetGameObject().Initialize();
                    break;
                }
            }
        }
        if (_position != null)
        {
            m_SpawnOnPool.GetGameObject().transform.position = _position;
        }
        if (_rotation != null)
        {
            m_SpawnOnPool.GetGameObject().transform.rotation = _rotation;
        }

        m_SpawnOnPool.GetGameObject().transform.SetParent(_parent);

        m_SpawnOnPool.GetGameObject().gameObject.SetActive(true);
        m_SpawnOnPool.OnObjectSpawn();

        return m_SpawnOnPool;
    }

    public void AddObjectPool(PooledObjectType _prefabType, IPooledObject _pooledObject)
    {
        if (!m_poolDictionary[_prefabType].Contains(_pooledObject))
            m_poolDictionary[_prefabType].Enqueue(_pooledObject);
    }
}