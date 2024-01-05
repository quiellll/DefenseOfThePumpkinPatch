using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAscend : AEnemyState //estado de movimiento hacia delante del fantasma
{
    public GhostAscend(AEnemyController enemy) : base(enemy) { }

    public override void Enter(IState previousState) { }

    public override void Update()
    {
        _enemy.Move(Vector3.up, false);
        if(_enemy.transform.position.y > 15f) _enemy.Despawn();
    }
}
