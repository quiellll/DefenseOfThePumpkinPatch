using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStateContext : IStateContext
{
    public GameObject GameObject { get; }
    public void Move(Vector3 direction);
}
