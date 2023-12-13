using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : AEnemyController
{
    protected override void Start()
    {
        base.Start();

        SetInitialState(new GhostMoveForward(this));
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, WaveSpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        SetInitialState(new GhostMoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
    }
}
