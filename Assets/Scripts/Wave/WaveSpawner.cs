using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private int _enemiesPerWave;
    [SerializeField] private float _spawnDelay;

    private ObjectPool _pool;
    private int _enemiesSpawned = 0;
    private bool _spawnFinished = false;
    private int _enemiesAlive = 0;


    private void Awake()
    {
        _pool = new ObjectPool(_enemyPrefab, _enemiesPerWave, false, transform);
    }

    private void Start()
    {
        SpawnWave();
    }


    private void SpawnWave()
    {
        var enemy = _pool.Get() as EnemyController;

        if (!enemy) return;

        enemy.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        enemy.Spawner = this;

        _enemiesSpawned++;
        _enemiesAlive++;

        if(_enemiesSpawned >= _enemiesPerWave)
        {
            //lanzar evento de fin de spawn
            _spawnFinished = true;
            Debug.Log("fin de spawn de waveee");
        }

        else Invoke(nameof(SpawnWave), _spawnDelay);
    }

    public void DespawnEnemy(EnemyController enemy)
    {
        _pool.Return(enemy);
        if (enemy.IsAlive) return; 

        _enemiesAlive--;
        if(_spawnFinished && _enemiesAlive >= 0)
        {
            //evento de fin de oleada (victoria)
            Debug.Log("Ganaste la waveee");
        }
    }
}
