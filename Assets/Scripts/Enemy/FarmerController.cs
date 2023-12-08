using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : AEnemyController
{
    protected override void Start()
    {
        base.Start();

        SetInitialState(new MoveForward(this));
    }

    protected override void Die()
    {
        CurrentCell.BuildGrave(_stats.GravePrefab, new(XY.x, 0f, XY.y), _body.rotation);

        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, WaveSpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        SetInitialState(new MoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
    }
}
