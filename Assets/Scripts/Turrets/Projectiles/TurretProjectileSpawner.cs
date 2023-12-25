using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectileSpawner : MonoBehaviour
{

    // Obtiene el tipo de proyectil a generar, �til para cuando tengamos varios tipos de torre
    public Type ProjectileType { get; private set; }

    // El prefab de la bala
    [SerializeField] private ATurretProjectile _projectilePrefab;

    // N�mero m�ximo de proyectiles en la escena
    [SerializeField] private int _maxProjectileInScene;

    // Pool de proyectiles
    private ObjectPool _projectilePool;


    private void Awake()
    {
        // Se crea la Pool
        _projectilePool = new ObjectPool(_projectilePrefab, _maxProjectileInScene, true, transform);

        // Se asigna el tipo de proyectil
        ProjectileType = _projectilePrefab.GetType();
    }

    // M�todo para instanciar un proyectil
    public ATurretProjectile SpawnProjectile() => _projectilePool.Get() as ATurretProjectile;

    // M�todo para eliminar un proyectil de la escena y guardarlo en la Pool
    public void DespawnProjectile(ATurretProjectile projectile)
    {
        if(!projectile.Active)
        {
            //Debug.Log("ya esta despawneado el proyectil");
            return;
        }
        _projectilePool.Return(projectile);
    }
}
