using UnityEngine;

public interface IPoolObject
{
    public bool Active { get; set; }

    public void Reset();

    public IPoolObject Clone(Transform parent = null, bool active = false);


}