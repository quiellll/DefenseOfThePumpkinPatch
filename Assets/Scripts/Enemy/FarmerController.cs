using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : AEnemyController ///controlador del granjero que hereda del controlador comun
{
    protected override void Start()
    {
        base.Start();

        //SetInitialState(new MoveForward(this));
    }

    protected override void Die()
    {
        //al morir spawnea una tumba
        WorldGrid.Instance.BuildGrave(_stats.GravePrefab, CurrentCell, new(XY.x, 0f, XY.y), _body.rotation);

        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        SetInitialState(new MoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
    }
}
