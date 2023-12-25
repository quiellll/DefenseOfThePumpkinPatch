using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectile : ATurretProjectile
{
    private Vector3 _startPosition;
    private float _stepSize;
    private float _progress;
    private Vector3 _targetPos;
    private Catapult _catapult;
    private bool _hasTarget;
    private float _maxHeight;

    public override void Init(TurretProjectileSpawner spawner, AEnemyController target,
        Vector3 spawnPos, Turret turret, ATurretController controller)
    {
        base.Init(spawner, target, spawnPos, turret, controller);

        _catapult = turret as Catapult;

        _startPosition = transform.position;

        float distance = Vector3.Distance(_startPosition, EnemyTarget.transform.position);

        // This is one divided by the total flight duration, to help convert it to 0-1 progress.
        _stepSize = _catapult.ProjectileMoveSpeed / distance;

        _progress = 0f;
        _hasTarget = true;
        _maxHeight = _catapult.MaxProjectileHeight * distance;
    }

    void Update()
    {
        //Si el enemigo esta vivo y activo, actualiza la nueva posicion, sino nos quedamos con la ultima que 
        //actualizo antes de morir
        if (!EnemyTarget.IsAlive || !EnemyTarget.gameObject.activeSelf)
        {
            if(_hasTarget) _hasTarget = false;
        } 
        else if (_hasTarget)
        {
            if(Vector2.Distance(_controller.XY, EnemyTarget.XY) >= _turret.InnerRadius)
                _targetPos = EnemyTarget.transform.position;
            else _hasTarget = false;
        }


        // Increment our progress from 0 at the start, to 1 when we arrive.
        _progress = Mathf.Min(_progress + Time.deltaTime * _stepSize, 1.0f);

        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

        // Travel in a straight line from our start position to the target.        
        Vector3 nextPos = Vector3.Lerp(_startPosition, _targetPos, _progress);

        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * _maxHeight;

        // Continue as before.
        transform.LookAt(nextPos, transform.forward);
        transform.position = nextPos;

        if (_progress == 1.0f)
        {
            if(_hasTarget) Damage(EnemyTarget);
            DespawnProjectile();
        }
    }
}

