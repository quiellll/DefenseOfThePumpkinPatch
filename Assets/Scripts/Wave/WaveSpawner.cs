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
        enemy?.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);

        Invoke(nameof(SpawnWave), _spawnDelay);
    }
}
