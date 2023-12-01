using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public int PoolSize { get => _poolSize; }
    public int ActiveCount {  get  => _activeObjects; }

    private IPoolObject _prototype;
    private int _poolSize;
    private bool _allowAddNew;
    private int _activeObjects;
    private Transform _parent;

    private List<IPoolObject> _pool;


    public ObjectPool
        (IPoolObject prototype, int poolSize, bool allowAddNew = false, Transform parent = null)
    {
        _prototype = prototype;
        _poolSize = poolSize;
        _allowAddNew = allowAddNew;
        _parent = parent;
        _activeObjects = 0;

        _pool = new(_poolSize);

        for (int i = 0; i < _poolSize; i++) _pool.Add(_prototype.Clone(_parent));
    }

    public IPoolObject Get()
    {
        foreach(var obj in _pool)
        {
            if (obj.Active) continue;

            obj.Active = true;
            _activeObjects++;
            return obj;
        }

        if (!_allowAddNew) return null;

        var newObj = _prototype.Clone(_parent, true);
        _pool.Add(newObj);
        _activeObjects++;
        _poolSize++;
        return newObj;
    }

    public bool Return(IPoolObject obj)
    {
        if(!_pool.Contains(obj)) return false;

        obj.Active = false;
        _activeObjects--;
        obj.Reset();
        return true;
    }


}
