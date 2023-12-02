using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IPoolObject
{
    public BulletSpawner Spawner { get; set; }

    public bool Active { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this) : Instantiate(this, parent);

        clone.Active = active;
        return clone;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
