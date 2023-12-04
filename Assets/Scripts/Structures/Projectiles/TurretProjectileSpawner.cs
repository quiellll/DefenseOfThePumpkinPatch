using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectileSpawner : MonoBehaviour
{

    public Type ProjectileType { get; private set; }

    // El prefab de la bala
    [SerializeField] private ATurretProjectile _projectilePrefab;

    //
    [SerializeField] private int _maxProjectileInScene;

    private ObjectPool _projectilePool;


    private void Awake()
    {
        _projectilePool = new ObjectPool(_projectilePrefab, _maxProjectileInScene, true, transform);

        ProjectileType = _projectilePrefab.GetType();
    }

    public ATurretProjectile SpawnProjectile() => _projectilePool.Get() as ATurretProjectile;

    public void DespawnProjectile(ATurretProjectile projectile)
    {
        _projectilePool.Return(projectile);
    }
}
