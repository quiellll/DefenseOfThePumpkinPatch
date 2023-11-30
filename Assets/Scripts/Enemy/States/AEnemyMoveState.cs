using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyMoveState : AEnemyState //clase para agrupar func. comunes de ambos estados de movimiento
{
    protected GridCell _currentWaypoint;
    protected int _currentWaypointIndex;
    private float _distanceToWaypoint;
    private Transform _transform;
    private Vector2 _gridPosition, _gridDirection; //posicion y direccion en el plano xz

    public AEnemyMoveState(IEnemyStateContext enemy) : base(enemy) { }

    public override void Enter(IState previousState)
    {
        _transform = _enemy.GameObject.transform;
    }

    public override void Update()
    {
        //comprobar distancia al siguiente waypoint y si esta suficientemente cerca cambiar de waypoint
        _gridPosition = new Vector2(_transform.position.x, _transform.position.z);
        _distanceToWaypoint = Vector2.Distance(_gridPosition, _currentWaypoint.XY);
        if (_distanceToWaypoint <= 0.1f) NextWaypoint(); //cambiar de waypoint segun la direccion de movimiento

        //calcular la direccion normalizada y llamar al movimiento en el contexto
        _gridDirection = (_currentWaypoint.XY - _gridPosition);
        _enemy.Move(new Vector3(_gridDirection.x, 0f, _gridDirection.y).normalized);
    }

    protected abstract void NextWaypoint(); //funcion a implementar que determina la direccíon de movimiento
}