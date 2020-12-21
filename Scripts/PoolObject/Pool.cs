using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string Name;
    public int Count;

    public PoolType PoolType;
    public GameObject ObjectPrefab;

    private int _tempCount;
    private Transform _transform;

    private List<GameObject> _objectList;

    public void Populate(Transform parentTransform)
    {
        _objectList = new List<GameObject>();
        _tempCount = 0;
        _transform = parentTransform;

        for (int i = 0; i < Count; ++i)
        {
            _objectList.Add(GetInstantiateObject());
        }
    }

    public T GetPooledObject<T>() where T : Component
    {
        for (int i = 0; i < _objectList.Count; ++i)
        {
            if (!_objectList[i].activeInHierarchy)
            {
                _objectList[i].SetActive(true);
                
                return _objectList[i].GetComponent<T>();
            }
        }

        return null;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _objectList.Count; ++i)
        {
            if (!_objectList[i].activeInHierarchy)
            {
                return _objectList[i];
            }
        }

        return null;
    }

    public void ClearTempPool()
    {
        for (int i = 0; _objectList.Count != Count; i = ++i % _objectList.Count)
        {
            if (_objectList[i].activeInHierarchy)
            {
                _objectList.RemoveAt(i);
                --_tempCount;
            }
        }
    }

    public void TempExpandPool(int count)
    {
        _tempCount += count;

        for (int i = _objectList.Count; i < _objectList.Count + count; ++i)
        {
            _objectList.Add(GetInstantiateObject());
        }
    }

    private GameObject GetInstantiateObject()
    {
        GameObject instantiatePrefab = Object.Instantiate(ObjectPrefab);
        instantiatePrefab.transform.SetParent(_transform);
        instantiatePrefab.SetActive(false);

        return instantiatePrefab;
    }
}
