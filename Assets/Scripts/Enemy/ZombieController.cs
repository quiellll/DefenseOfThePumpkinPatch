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

    public override void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        SetInitialState(new MoveForward(this));
    }

    public override void InteractWithPumpkin(GridCell pumpkinCell)
    {
        base.InteractWithPumpkin(pumpkinCell);
        State = null;
        SetAnimation(_pickUpAnim, false);
        StartCoroutine(WaitAndDie(pumpkinCell));
    }

    private IEnumerator WaitAndDie(GridCell pumpkinCell)
    {
        GameManager.Instance.ServiceLocator.Get<IAudioManager>().PlaySoundEffect(_stats.DieSound);
        yield return new WaitForSeconds(0.1f);
        SetAnimation(_deadAnim, false);
        pumpkinCell.DestroyPumpkin();
        yield return new WaitForSeconds(0.6f);
        Die();
    }

}
