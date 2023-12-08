using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackwards : AEnemyMoveState
{
    public MoveBackwards(AEnemyController enemy) : base(enemy)
    {
        _currentWaypointIndex = WorldGrid.Instance.Waypoints.Count - 1;
        _currentWaypoint = WorldGrid.Instance.Waypoints[_currentWaypointIndex];
    }

    protected override void NextWaypoint() //hace que se mueva por el camino hacia atras
    {
        _currentWaypointIndex--;

        if (_currentWaypointIndex >= 0)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[_currentWaypointIndex];
            return;
        }

        _enemy.Despawn();
    }
}
