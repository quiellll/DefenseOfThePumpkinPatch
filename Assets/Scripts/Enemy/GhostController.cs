using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : AEnemyController //controlador del fantasma que hereda del comun abstracto
{
    private ZombieSpawner _zombieSpawner;
    protected override void Start()
    {
        base.Start();

        SetInitialState(new GhostMoveForward(this));
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        _zombieSpawner = (spawner as WaveSpawner).gameObject.GetComponent<ZombieSpawner>();
        SetInitialState(new GhostMoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
    }

    public void SpawnZombie()
    {
        _zombieSpawner.SpawnZombie(transform.position, transform.rotation);
    }
}
