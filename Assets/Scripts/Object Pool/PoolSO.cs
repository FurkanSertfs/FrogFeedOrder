using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSO", menuName = "PoolSO", order = 0)]
public class PoolSO : ScriptableObject
{
    public string poolName;
    public GameObject prefab;
    public int size;
    public int maxSize = 10;
}
