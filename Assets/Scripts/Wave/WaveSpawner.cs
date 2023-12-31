using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour, IEnemySpawner //spawner de enemigos
{

    //evento llamado cuando no queden enemigos por spawnear ni activos
    public UnityEvent WaveFinished; 
    public AEnemyController EnemyPrefab { get => _enemyPrefab; } //prefab a spawnear

    [SerializeField] private Transform _spawnPoint; //punto donde spawnean
    [SerializeField] private AEnemyController _enemyPrefab;
    [SerializeField] private int _enemiesPerWave; //enemigos a spawnear en la oleada
    [SerializeField] private float _spawnDelay; //tiempo entre cada spawn

    private ObjectPool _pool;
    private bool _spawnFinished = false;
    private int _spawnedEnemies = 0;
    private int _aliveEnemies = 0;
    private int _activeEnemies = 0;


    private void Awake()
    {
        _pool = new ObjectPool(_enemyPrefab, _enemiesPerWave, false, _spawnPoint);
    }

    public void SpawnWave() //se llama desde los estados de juego
    {
        _spawnFinished = false;
        _spawnedEnemies = 0;
        _aliveEnemies = 0;
        _activeEnemies = 0;

        SpawnEnemy();
    }

    //funcion recursiva que spawnea un enemigo, espera el delay y se llama a si misma,
    //Asi hasta spawnear todos los enemigos que le toca
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

    //funcion que invocan los enemigos al morir o hacer todo su recorrido
    //que los devuelve a la pool y limpia
    public void DespawnEnemy(AEnemyController enemy)
    {
        enemy.transform.position = _spawnPoint.position;
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
