using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : AEnemyController ///controlador del granjero que hereda del controlador comun
{
    [SerializeField] private GameObject _pumpkin;

    protected override void Start()
    {
        base.Start();

        //SetInitialState(new MoveForward(this));
    }

    protected override void Die()
    {
        //al morir spawnea una tumba
        GameManager.Instance.CellManager.BuildGrave(_stats.GravePrefab, CurrentCell, XY, _body.rotation);

        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        _pumpkin.SetActive(false);
        SetInitialState(new MoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void InteractWithPumpkin(GridCell pumpkinCell)
    {
        State = null;
        SetAnimation(_pickUpAnim);
        StartCoroutine(WaitAndReturnToPath(pumpkinCell));
    }

    private IEnumerator WaitAndReturnToPath(GridCell pumpkinCell)
    {
        yield return new WaitForSeconds(.4f);
        pumpkinCell.DestroyPumpkin();
        _pumpkin.SetActive(true);
        State = new ReturnToPath(this);
    }
}
