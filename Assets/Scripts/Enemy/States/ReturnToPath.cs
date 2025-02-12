using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPath : AEnemyState
{
    private GridCell _endWaypoint;
    private Vector3 _directionToWaypoint;
    private float _distanceToWaypoint;

    public ReturnToPath(AEnemyController enemy) : base(enemy) { }


    public override void Enter(IState previousState)
    {
        _endWaypoint = GameManager.Instance.CellManager.Waypoints[GameManager.Instance.CellManager.Waypoints.Count - 1];
        var gridDirection = _endWaypoint.XY - _enemy.XY;
        _directionToWaypoint = new Vector3(gridDirection.x, 0f, gridDirection.y).normalized;
        _enemy.ChangeRotationSpeed(3f);
    }

    public override void Exit(IState nextState)
    {
        base.Exit(nextState);
        _enemy.ResetRotationSpeed();
    }


    public override void Update()
    {
        _distanceToWaypoint = Vector2.Distance(_enemy.XY, _endWaypoint.XY);

        if(_distanceToWaypoint <= 0.04f)
        {
            _enemy.State = new MoveBackwards(_enemy);
            return;
        }

        _enemy.Move(_directionToWaypoint);
    }
}