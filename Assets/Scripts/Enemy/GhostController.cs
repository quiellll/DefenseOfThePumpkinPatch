using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : AEnemyController //controlador del fantasma que hereda del comun abstracto
{
    public bool Transformed {  get; set; }

    private ZombieSpawner _zombieSpawner;

    private Vector3 _transformPositon;
    private Quaternion _transformRotation;

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
        Transformed = false;
        _zombieSpawner = (spawner as WaveSpawner).gameObject.GetComponent<ZombieSpawner>();
        SetInitialState(new GhostMoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
        Transformed = false;
    }

    public void TransformToZombie(WorldGrid.GraveAtPath grave)
    {
        Transformed = true;

        _transformPositon = transform.position;
        _transformRotation = transform.rotation;

        Despawn(); //despawneamos al fantasma

        StartCoroutine(SpawnZombie(grave));

    }

    private IEnumerator SpawnZombie(WorldGrid.GraveAtPath grave)
    {
        yield return new WaitForSeconds(0.5f);
        WorldGrid.Instance.DestroyGrave(grave); //destuimos la tumba
        _zombieSpawner.SpawnZombie(_transformPositon, _transformRotation);
    }
}

