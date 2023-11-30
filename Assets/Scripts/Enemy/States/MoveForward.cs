using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : AEnemyMoveState
{
    public MoveForward(IEnemyStateContext enemy) : base(enemy)
    {
        _currentWaypointIndex = 0;
        _currentWaypoint = WorldGrid.Instance.Waypoints[0];
    }

    protected override void NextWaypoint() //hace que se mueva por el camino hacia delante
    {
        _currentWaypointIndex++;

        if(_currentWaypointIndex < WorldGrid.Instance.Waypoints.Count)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[_currentWaypointIndex];
            return;
        }

        _enemy.State = new MoveBackward(_enemy);
    }
}
