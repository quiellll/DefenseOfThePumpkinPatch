using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaProjectile : ATurretProjectile
{
    private Transform _body;
    // Almacena todos los objetos con los que colisiona
    private Collider[] _overlaps = new Collider[5];
    // Radio del proyectil
    private float _colliderRadius;
    // Dirección del movimiento
    private Vector3 _direction;
    private bool _hasTarget = true;

    private void Awake()
    {
        // Se obtiene el collider del objeto hijo y se usa su radio para el OverlapSphere
        var col = GetComponentInChildren<SphereCollider>();
        _colliderRadius = col.radius * col.transform.localScale.x;
        _body = col.transform;
    }

    public override void Init(TurretProjectileSpawner spawner, AEnemyController target, 
        Vector3 spawnPos, Turret turret, ATurretController controller)
    {
        base.Init(spawner, target, spawnPos, turret, controller);
        _body.rotation = Quaternion.LookRotation(EnemyTarget.transform.position - _body.position);
        _hasTarget = true;
    }

    private void Update()
    {
        // Si el enemigo al que estaba siendo disparado muere, se mueve hacia delante (porque de normal la bala sigue a los enemigos).
        if (!EnemyTarget.IsAlive || !EnemyTarget.gameObject.activeSelf)
        {
            if(_hasTarget)
            {
                _hasTarget = false;
                _direction = transform.forward;
                Invoke(nameof(DespawnProjectile), 3f);
            }
            CheckCollisions();
        }
        if(_hasTarget) _direction = (EnemyTarget.transform.position - _body.position);

        // Si el enemigo sigue vivo, la dirección será el vector hacia la posición del propio enemigo.
        transform.Translate(_turret.ProjectileMoveSpeed * Time.deltaTime * _direction.normalized, Space.World);

        //rotar en la direccion de movimiento
        _body.rotation = Quaternion.LookRotation(_direction);

        // Comprueba si ha alcanzado al target

        if(_hasTarget && _direction.magnitude <= 0.1f)
        {
            Damage(EnemyTarget);
            DespawnProjectile();
        }
        
    }

    private void CheckCollisions()
    {
        // Crea la OverlapSphere para saber con qué colisiona.
        var c = Physics.OverlapSphereNonAlloc(transform.position, _colliderRadius, _overlaps);

        for (int i = 0; i < c; i++)
        {
            // Si choca con otra bala o con otra torre, ignora la colisión
            if (_overlaps[i].GetComponentInParent<ATurretProjectile>() != null) continue;
            if (_overlaps[i].GetComponent<ATurretController>() != null) continue;

            // Si choca con un enemigo, lo daña
            if (_overlaps[i].TryGetComponent<AEnemyController>(out var e)) Damage(e);

            // Si choca con cualquier cosa que no sea torre o bala, desaparece (incluso con enemigos)
            DespawnProjectile();
            break;
        }

    }
}
