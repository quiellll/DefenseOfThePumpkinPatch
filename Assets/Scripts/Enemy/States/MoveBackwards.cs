using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackwards : AEnemyPathState //estado de moverse hacia atras en el path (tanto para granjero como para fantasma de momento)
{
    public MoveBackwards(AEnemyController enemy) : base(enemy) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);
        //selecciona el ultimo waypoint porque siempre que se entra a este estado se esta al final del camino
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

        _enemy.Despawn(); //despawnea si ha llegado al waypoint 0 (el primero)
    }
}
