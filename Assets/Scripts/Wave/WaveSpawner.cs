using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public UnityEvent WaveFinished;
    public AEnemyController EnemyPrefab { get => _enemyPrefab; }

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AEnemyController _enemyPrefab;
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

    public void SpawnWave()
    {
        _spawnFinished = false;
        _spawnedEnemies = 0;
        _aliveEnemies = 0;
        _activeEnemies = 0;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        var enemy = _pool.Get() as AEnemyController;

        if (!enemy) return;

        enemy.InitEnemy(_spawnPoint.position, _spawnPoint.rotation, this);

        _spawnedEnemies++;
        _aliveEnemies++;
        _activeEnemies++;

        if(_spawnedEnemies >= _enemiesPerWave)
        {
            //fin de spawn
            _spawnFinished = true;
        }

        else Invoke(nameof(SpawnEnemy), _spawnDelay);
    }

    public void DespawnEnemy(AEnemyController enemy)
    {
        _pool.Return(enemy);
        _activeEnemies--;

        if (!enemy.IsAlive) _aliveEnemies--;

        if (_spawnFinished && _activeEnemies == 0)
        {
            //fin de oleada 
            WaveFinished.Invoke();
        }
    }
}
