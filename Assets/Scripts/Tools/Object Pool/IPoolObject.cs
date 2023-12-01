using UnityEngine;

public interface IPoolObject
{
    public bool Active { get; set; }
    public ObjectPool Pool { set; }

    public void Reset();

    public IPoolObject Clone(ObjectPool pool, Transform parent = null, bool active = false);


}