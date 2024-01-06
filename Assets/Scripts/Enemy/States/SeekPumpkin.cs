using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPumpkin : AEnemyState
{
    public SeekPumpkin(AEnemyController enemy) : base(enemy) { }

    private GridCell _currentPumpkin;
    private Vector3 _directionToPumpkin;
    private float _distanceToPumpkin;


    public override void Enter(IState previousState)
    {
        _currentPumpkin = GameManager.Instance.CellManager.GetNearestPumpkinCell();

        if (_currentPumpkin == null)
        {
            Debug.Log("No hay calabazas");
            //nuevo estado de no hacer nada porque el jugador perdio?
            _enemy.State = null;
            return;
        }

        GameManager.Instance.CellManager.PumpkinsUpdated.AddListener(OnPumpkinsUpdated);

        var gridDirection = _currentPumpkin.XY - _enemy.XY;
        _directionToPumpkin = new Vector3(gridDirection.x, 0f, gridDirection.y).normalized;
    }

    public override void Update()
    {
        if (_currentPumpkin == null) return;

        _distanceToPumpkin = Vector2.Distance(_enemy.XY, _currentPumpkin.XY);

        if(_distanceToPumpkin < 0.3f)
        {
            _enemy.InteractWithPumpkin(_currentPumpkin);
            return;
        }

        _enemy.Move(_directionToPumpkin);

    }

    public override void Exit(IState nextState)
    {
        base.Exit(nextState);

        GameManager.Instance.CellManager.PumpkinsUpdated.RemoveListener(OnPumpkinsUpdated);
    }

    private void OnPumpkinsUpdated(GridCell newPumpkinCell)
    {
        _currentPumpkin = newPumpkinCell;

        if (_currentPumpkin == null)
        {
            Debug.Log("No hay calabazas");
            //nuevo estado de no hacer nada porque el jugador perdio?
            _enemy.State = null;
            return;
        }

        var gridDirection = (_currentPumpkin.XY - _enemy.XY);
        _directionToPumpkin = new Vector3(gridDirection.x, 0f, gridDirection.y).normalized;


    }
}
