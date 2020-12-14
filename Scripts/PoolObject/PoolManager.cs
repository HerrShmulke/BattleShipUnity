using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    protected List<Pool> PoolList;
    private Dictionary<PoolType, Pool> _pools;
    private Transform _transform;

    public Pool GetPool(PoolType poolType)
    {
        return _pools[poolType];
    }

    public void AddPool(PoolType poolType, Pool pool)
    {
        _pools[poolType] = pool;
    }


    private void Awake()
    { 
        _pools = new Dictionary<PoolType, Pool>();
        _transform = transform;

        for (int i = 0; i < PoolList.Count; ++i)
        {
            PoolList[i].Populate(_transform);
            _pools[PoolList[i].PoolType] = PoolList[i];
        }
    }
}
