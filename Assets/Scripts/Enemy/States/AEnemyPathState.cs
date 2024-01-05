using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyPathState : AEnemyState //clase para agrupar func. comunes de todos los estados de movimiento
{
    protected GridCell _currentWaypoint; //waypoint actual
    protected int _currentWaypointIndex; //indice en el array de waypoint de worldgrid del waypoint actual
    protected float _distanceToWaypoint; //distancia a waypoint actual
    private Transform _transform; //ref al transform del enemigo
    private Vector2 _gridPosition, _gridDirection; //posicion y direccion en el plano xz (sin la y)

    public AEnemyPathState(AEnemyController enemy) : base(enemy) { }

    public override void Enter(IState previousState)
    {
        _transform = _enemy.gameObject.transform;
    }

    public override void Update()
    {
        //comprobar distancia al siguiente waypoint y si esta suficientemente cerca cambiar de waypoint
        _gridPosition = new Vector2(_transform.position.x, _transform.position.z);
        _distanceToWaypoint = Vector2.Distance(_gridPosition, _currentWaypoint.XY);
        if (_distanceToWaypoint <= 0.04f) NextWaypoint(); //cambiar de waypoint segun el estado de movimiento hijo (forward-backward)

        //calcular la direccion normalizada al waypoint y llamar al movimiento en el enemigo
        _gridDirection = (_currentWaypoint.XY - _gridPosition);
        _enemy.Move(new Vector3(_gridDirection.x, 0f, _gridDirection.y).normalized);
    }

    protected abstract void NextWaypoint(); //funcion a implementar que determina la direccion de movimiento
}