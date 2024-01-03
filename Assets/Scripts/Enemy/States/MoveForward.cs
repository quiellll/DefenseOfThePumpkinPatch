using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : AEnemyPathState //estado de moverse hacia delante por el path (para el granjero de momento)
{
    public MoveForward(AEnemyController enemy) : base(enemy) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);

        //busca el waypoint mas cercano
        _currentWaypointIndex = -1;
        int wpIndex = -1, cellIndex = 0;

        while (wpIndex < cellIndex)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[++_currentWaypointIndex];
            wpIndex = WorldGrid.Instance.GetIndexOfPathCell(_currentWaypoint);
            cellIndex = WorldGrid.Instance.GetIndexOfPathCell(_enemy.CurrentCell);
        }
    }

    protected override void NextWaypoint() //hace que se mueva por el camino hacia delante
    {
        _currentWaypointIndex++;

        if(_currentWaypointIndex < WorldGrid.Instance.Waypoints.Count)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[_currentWaypointIndex];
            return;
        }

        //_enemy.State = new MoveBackwards(_enemy);
        _enemy.State = new SeekPumpkin(_enemy);
    }
}
