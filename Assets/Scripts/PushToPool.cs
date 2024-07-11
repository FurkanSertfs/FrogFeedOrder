using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushToPool : MonoBehaviour
{
    [SerializeField]
    PoolSO poolSO;

    [SerializeField]
    bool isDebug;

    private void OnEnable()
    {
        UIPresenter.OnPrepareNextLevel += PushObjToPool;
    }

    private void OnDisable()
    {
        UIPresenter.OnPrepareNextLevel -= PushObjToPool;
    }

    void PushObjToPool()
    {
        PoolManager.Instance.Push(poolSO, gameObject);
    }
}
