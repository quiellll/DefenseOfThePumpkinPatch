using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public Turret TurretToBuild {get => _turretToBuild; set => SetTurretToBuild(value); }



    private Turret _turretToBuild;
    private GameObject _turretDummy;

    private void SetTurretToBuild(Turret turret)
    {
        _turretToBuild = turret;
        _turretDummy = Instantiate(turret.Dummy, Vector3.zero, turret.Dummy.transform.rotation);
        if (WorldGrid.Instance.CellOnPointer)
        {
            _turretDummy.transform.position = WorldGrid.Instance.CellOnPointer.transform.position;
        }
        else _turretDummy.SetActive(false);
    }


    private void Update()
    {
        if (!_turretToBuild || !_turretDummy) return;

        var cell = WorldGrid.Instance.CellOnPointer;
        if (!cell || cell.Type != GridCell.CellType.Turret)
        {
            if(_turretDummy.activeSelf) _turretDummy.SetActive(false);
            return;
        }

        if(!_turretDummy.activeSelf) _turretDummy.SetActive(true);
        _turretDummy.transform.position = WorldGrid.Instance.CellOnPointer.transform.position;

    }

    public void Build(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if(!_turretToBuild || !_turretDummy || !_turretDummy.activeSelf
            || !WorldGrid.Instance.CellOnPointer) return;

        //Instantiate(_turretToBuild.Prefab, _turretDummy.transform.position, 
        //    _turretToBuild.Prefab.transform.rotation);

        bool built = WorldGrid.Instance.CellOnPointer.BuildTurret(_turretToBuild);

        if (!built) return;

        Destroy(_turretDummy);
        _turretDummy = null;
        _turretToBuild = null;
    }

}
