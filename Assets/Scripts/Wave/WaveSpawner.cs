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
    private bool _spawnFinished = false;
    private int _spawnedEnemies = 0;
    private int _aliveEnemies = 0;
    private int _activeEnemies = 0;


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

        _spawnedEnemies++;
        _aliveEnemies++;
        _activeEnemies++;

        if(_spawnedEnemies >= _enemiesPerWave)
        {
            //fin de spawn
            _spawnFinished = true;
            Debug.Log("fin de spawn");
        }

        else Invoke(nameof(SpawnWave), _spawnDelay);
    }

    public void DespawnEnemy(EnemyController enemy)
    {
        _pool.Return(enemy);
        _activeEnemies--;

        if (!enemy.IsAlive) _aliveEnemies--;

        if (_spawnFinished && _activeEnemies == 0)
        {
            //fin de oleada 
            Debug.Log("fin de wave");
        }
    }
}
