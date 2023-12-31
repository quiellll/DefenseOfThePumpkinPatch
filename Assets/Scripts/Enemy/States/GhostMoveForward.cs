using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMoveForward : AEnemyPathState //estado de movimiento hacia delante del fantasma
{
    private WorldGrid.GraveAtPath _grave; //tumba hacia la que el fantasma se mueve
    private GridCell _graveCell; //celda en la que esta la tumbs
    private bool _graveIsCurrentWaypoint = false; //indica si el fantasma se dirige a una tumba, o a un waypoint (si no hay tumbas mas cerca)
    private bool _reachedDestination = false; //si ha llegado al destino (tumba o ultimo waypoint del camino)

    public GhostMoveForward(AEnemyController enemy) : base(enemy) {}

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);


        //elige si ir a una tumba o a un waypoint inicialmente
        //guarda la celda del waypoint o de la tumba en currentWaypoint

        _currentWaypointIndex = -1;
        int waypointIndexAtPath = -1, currentCellIndexAtPath = 0;

        while (waypointIndexAtPath < currentCellIndexAtPath)
        {
            _currentWaypoint = WorldGrid.Instance.Waypoints[++_currentWaypointIndex];
            waypointIndexAtPath = WorldGrid.Instance.GetIndexOfPathCell(_currentWaypoint);
            currentCellIndexAtPath = WorldGrid.Instance.GetIndexOfPathCell(_enemy.CurrentCell);
        }

        _grave = WorldGrid.Instance.GetNearestGrave();
        _graveCell = WorldGrid.Instance.Path[_grave.PathIndex];
        WorldGrid.Instance.GravesUpdated.AddListener(UpdateGrave);

        if(_grave.PathIndex >= currentCellIndexAtPath &&  _grave.PathIndex <= waypointIndexAtPath)
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

        if(_graveIsCurrentWaypoint && _reachedWaypoint && _grave != null)
        {
            //hemos llegado a la tumba!
            _reachedDestination = true;
            WorldGrid.Instance.DestroyGrave(_grave); //destuimos la tumba
            _grave = null;
            Exit(null);
            _enemy.Despawn(); //despawneamos al fantasma
            //aqui falta spawnear un zombie
            return;
        }

        //si no hemos llegado al destino y estamos vivos:

        //buscamos el siguiente waypoint
        int previousWaypointIndex = _currentWaypointIndex;
        _currentWaypointIndex++;

        int clampedIndex = Mathf.Min(_currentWaypointIndex, WorldGrid.Instance.Waypoints.Count - 1);

        _currentWaypoint = WorldGrid.Instance.Waypoints[clampedIndex];

        //indice del waypoint en el Path
        int waypointIndexAtPath = WorldGrid.Instance.GetIndexOfPathCell(_currentWaypoint);

        //revisamos si hay una tumba mas cerca en el path que el waypoint
        if (_grave != null && _grave.PathIndex <= waypointIndexAtPath)
        {//si la hay, la seleccionamos como waypoint
            _currentWaypointIndex = previousWaypointIndex;
            _currentWaypoint = _graveCell;
            _graveIsCurrentWaypoint = true;
            return;
        }

        _graveIsCurrentWaypoint = false;

        //si no la hay, revisamos si hemos llegado al ultimo waypoint
        if(_currentWaypointIndex >= WorldGrid.Instance.Waypoints.Count)
        {
            //si hemos llegado al ultimo waypoint cambiamos de estado a moverse hacia atras
            _reachedDestination = true;
            _enemy.State = new MoveBackwards(_enemy);
        }

    }

    //metodo llamado por un evento de worldgrid cuando un fantasma llega a una tumba y esta se despawnea
    //porque todos los fantasmas tienen que recalcular su waypoint y/o tumba a la que van
    private void UpdateGrave(WorldGrid.GraveAtPath grave) //el argumento es la nueva tumba mas cercana
    {
        if (grave == _grave) return;

        _grave = grave;

        //ignorar esto xd
        //if (_grave == null)
        //{
        //    if(_graveIsCurrentWaypoint) NextWaypoint();
        //    return;
        //}
        //_graveCell = WorldGrid.Instance.Path[_grave.PathIndex];
        //if (_graveIsCurrentWaypoint) NextWaypoint();

        if(_grave != null) _graveCell = WorldGrid.Instance.Path[_grave.PathIndex];
        if (_graveIsCurrentWaypoint) NextWaypoint();

    }
}
