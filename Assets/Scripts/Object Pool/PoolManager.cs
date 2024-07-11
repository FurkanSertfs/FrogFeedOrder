using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<string, ObjectPool<GameObject>> poolDictionary = new Dictionary<string, ObjectPool<GameObject>>();

    [SerializeField]
    PoolSO[] pools;

    private void Start()
    {
        InitiliazePools();
    }

    void InitiliazePools()
    {
        foreach (var pool in pools)
        {
            CreatePool(pool.poolName, pool.size, pool.maxSize, pool.prefab);
        }
    }

    private GameObject CreatePooledItem(GameObject prefab)
    {
        var go = Instantiate(prefab);
        go.SetActive(false);
        return go;
    }

    void CreatePool(string poolName, int initialPoolSize, int maxPoolSize, GameObject prefab)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            Debug.LogError($"Pool with name {poolName} already exists.");
            return;
        }

        var pool = new ObjectPool<GameObject>(createFunc: () => CreatePooledItem(prefab), actionOnGet: OnTakeFromPool, actionOnRelease: OnReturnedToPool, actionOnDestroy: OnDestroyPoolObject, defaultCapacity: initialPoolSize, maxSize: maxPoolSize);

        poolDictionary[poolName] = pool;
    }

    public GameObject Pull(string poolName, Vector3 position, Quaternion rotation, Transform parent)
    {
        var poolObject = Pull(poolName);
        poolObject.transform.SetPositionAndRotation(position, rotation);
        poolObject.transform.SetParent(parent);
        return poolObject;
    }

    public GameObject Pull(PoolSO poolSO, Vector3 position, Quaternion rotation, Transform parent)
    {
        return Pull(poolSO.poolName, position, rotation, parent);
    }

    public GameObject Pull(string poolName)
    {
        if (!poolDictionary.TryGetValue(poolName, out var pool))
        {
            Debug.LogError($"No pool found for {poolName}. Create a pool first.");
            return null;
        }

        return pool.Get();
    }

    public void Push(PoolSO poolSO, GameObject poolObject)
    {
        Push(poolSO.poolName, poolObject);
    }

    public void Push(String poolName, GameObject poolObject)
    {
        if (!poolDictionary.TryGetValue(poolName, out var pool))
        {
            Debug.LogError($"No pool found for {poolName}. Create a pool first.");
            return;
        }
        poolObject.transform.SetParent(transform);
        pool.Release(poolObject);
    }

    private void OnReturnedToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }

    private void OnTakeFromPool(GameObject poolObject)
    {
        poolObject.SetActive(true);
    }

    private void OnDestroyPoolObject(GameObject poolObject)
    {
        Destroy(poolObject);
    }
}
