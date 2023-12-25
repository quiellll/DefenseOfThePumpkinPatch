using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATurretProjectile : MonoBehaviour, IPoolObject
{
    // Referencia a su Spawner
    public TurretProjectileSpawner Spawner { get; private set; }
    // Referencia al enemigo al que debe perseguir
    public AEnemyController EnemyTarget { get; private set; }

    // Da�o que realiza el proyectil
    [SerializeField] protected int _projectileDamage;

    // M�todo de PoolObject para conocer su estado
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    // M�todo de PoolObject para clonar una instancia del objeto
    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this, parent) : Instantiate(this);

        clone.Active = active;
        return clone;
    }

    // M�todo de PoolObject (no implementado porque no es necesario)
    public virtual void Reset() {}

    // M�todo de PoolObject para inicializar el objeto
    public virtual void Init(TurretProjectileSpawner spawner, AEnemyController target, Vector3 projectileSpawnPos) 
    {
        Spawner = spawner;
        EnemyTarget = target;
        // Se coloca el proyectil en la posici�n del spawner de la torre (punta del ca��n, base de la ballesta, cuchara de la catapulta...)
        // Y se rota para que su vector forward apunte hacia el enemigo.
        transform.SetPositionAndRotation(projectileSpawnPos, Quaternion.LookRotation(target.transform.position - projectileSpawnPos));
    }

    // M�todo para da�ar al enemigo
    protected void Damage(AEnemyController enemy) 
    {
        // Indica al enemigo cu�nto da�o debe recibir en funci�n del proyectil
        enemy.TakeDamage(_projectileDamage);
    }

    // M�todo de PoolObject para sacar un objeto de la escena
    protected void DespawnProjectile()
    {
        Spawner.DespawnProjectile(this);
    }

}