using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : AEnemyController
{
    protected override void Start()
    {
        base.Start();
        SetInitialState(new MoveForward(this));
    }

    protected override void Die()
    {
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

    public override void InteractWithPumpkin(GridCell pumpkinCell)
    {
        State = null;
        SetAnimation(_pickUpAnim, false);
        StartCoroutine(WaitAndDie(pumpkinCell));
    }

    private IEnumerator WaitAndDie(GridCell pumpkinCell)
    {
        yield return new WaitForSeconds(.4f);
        pumpkinCell.DestroyPumpkin();
        StartCoroutine(StartDeath());
    }

}
