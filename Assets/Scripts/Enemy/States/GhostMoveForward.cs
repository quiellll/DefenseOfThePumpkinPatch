using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMoveForward : AEnemyMoveState
{
    private WorldGrid.GraveAtPathIndex _grave;
    private GridCell _graveCell;
    private bool _graveIsCurrentWp = false;
    private bool _reachedDestination = false;

    public GhostMoveForward(AEnemyController enemy) : base(enemy) {}

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);

        _currentWaypointIndex = -1;
        int wpIndex = -1, cellIndex = 0;

        while (wpIndex < cellIndex)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[++_currentWaypointIndex];
            wpIndex = WorldGrid.Instance.GetIndexOfPathCell(_currentWaypoint);
            cellIndex = WorldGrid.Instance.GetIndexOfPathCell(_enemy.CurrentCell);
        }

        _grave = WorldGrid.Instance.GetNearestGrave();
        _graveCell = WorldGrid.Instance.Path[_grave.PathIndex];
        WorldGrid.Instance.GravesUpdated.AddListener(UpdateGrave);

        if(_grave.PathIndex >= cellIndex &&  _grave.PathIndex <= wpIndex)
        {
            _currentWaypointIndex = Mathf.Max(_currentWaypointIndex - 1, 0);
            _currentWaypoint = _graveCell;
        }
    }

    public override void Exit(IState nextState)
    {
        base.Exit(nextState);

        WorldGrid.Instance.GravesUpdated.RemoveListener(UpdateGrave);

    }

    protected override void NextWaypoint()
    {
        if(!_enemy.Active || _reachedDestination) return;

        if(_graveIsCurrentWp && _reachedWaypoint && _grave != null)
        {
            //hemos llegado a la tumba!
            _reachedDestination = true;
            WorldGrid.Instance.RemoveGrave(_grave);
            _grave = null;
            Exit(null);
            _enemy.Despawn();
            Debug.Log("TUMBA FANTASMA");
            return;
        }


        int previousWaypointIndex = _currentWaypointIndex;
        _currentWaypointIndex++;

        int clampedIndex = Mathf.Min(_currentWaypointIndex, WorldGrid.Instance.Waypoints.Count - 1);

        _currentWaypoint = WorldGrid.Instance.Waypoints[clampedIndex];

        //indice del waypoint en el Path
        int wpIndex = WorldGrid.Instance.GetIndexOfPathCell(_currentWaypoint);

        if (_grave != null && _grave.PathIndex <= wpIndex)
        {
            _currentWaypointIndex = previousWaypointIndex;
            _currentWaypoint = _graveCell;
            _graveIsCurrentWp = true;
            return;
        }

        _graveIsCurrentWp = false;

        if(_currentWaypointIndex >= WorldGrid.Instance.Waypoints.Count)
        {
            //hemos llegado al ultimo waypoint
            _reachedDestination = true;
            _enemy.State = new MoveBackwards(_enemy);
        }

    }

    private void UpdateGrave(WorldGrid.GraveAtPathIndex grave)
    {
        if (grave == _grave) return;

        _grave = grave;

        if (_grave == null)
        {
            if(_graveIsCurrentWp) NextWaypoint();
            return;
        }

        _graveCell = WorldGrid.Instance.Path[_grave.PathIndex];

        if (_graveIsCurrentWp) NextWaypoint();
    }
}
