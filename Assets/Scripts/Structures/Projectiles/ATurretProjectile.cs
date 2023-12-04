using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATurretProjectile : MonoBehaviour, IPoolObject
{
    public TurretProjectileSpawner Spawner { get; private set; }
    public EnemyController EnemyTarget { get; private set; }

    [SerializeField] private int _projectileDamage;
    [SerializeField] private int _projectileRadius;

    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this, parent) : Instantiate(this);

        clone.Active = active;
        return clone;
    }

    public virtual void Reset()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Init(TurretProjectileSpawner spawner, EnemyController target, Vector3 projectileSpawnPos) 
    {
        Spawner = spawner;
        EnemyTarget = target;
        transform.SetPositionAndRotation(projectileSpawnPos, Quaternion.LookRotation(target.transform.position - projectileSpawnPos));
    }

    protected void Damage(EnemyController enemy) 
    {
        enemy.TakeDamage(_projectileDamage);
    }

    protected void DespawnProjectile()
    {
        Spawner.DespawnProjectile(this);
    }

}
