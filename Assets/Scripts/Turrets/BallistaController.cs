using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaController : ATurretController
{
    private TurretProjectileSpawner _projectileSpawner; // Spawner/Generador de proyectiles

    protected override void Start()
    {
        base.Start();

        foreach (TurretProjectileSpawner spawner in FindObjectsOfType<TurretProjectileSpawner>())
        {
            // Si el spawner de balas es igual que la bala de mi torreta
            if (spawner.ProjectileType == _turret.ProjectilePrefab.GetType())
            {
                // Asigno el spawner a la variable para conocer sus datos
                _projectileSpawner = spawner;
                break;
            }
        }
    }

    protected override void Fire()
    {
        // Crea un proyectil en el spawner y lo inicializa llamando al método del Spawner (Object Pool)
        var projectile = _projectileSpawner.SpawnProjectile();
        projectile.Init(_projectileSpawner, _currentTarget, _projectileSpawnPoint.position, _partToRotate.rotation,
            _turret, this);
    }
}
