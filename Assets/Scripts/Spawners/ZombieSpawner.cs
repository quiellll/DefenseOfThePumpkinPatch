using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : MonoBehaviour, IEnemySpawner
{
    //evento llamado cuando no queden zombies por spawnear ni zombies activos
    public UnityEvent WaveFinished;

    public ZombieController ZombiePrefab { get => _zombiePrefab; } //propiedad que devuelve el prefab del zombie
    

    [SerializeField] private Transform _spawnPoint; //punto donde agrupar a los enemigos
    [SerializeField] private ZombieController _zombiePrefab; //prefab del zombie
    [SerializeField] private Level _level;

    private GhostSpawner _ghostSpawner;
    private int _poolSize;


    private ObjectPool _zombiePool;

    private bool _ghostWaveFinished = false;
    private int _aliveEnemies = 0;
    private int _activeEnemies = 0;


    private void Awake()
    {
        _ghostSpawner = GetComponent<GhostSpawner>();
        _poolSize = _level.Days[^1].Ghosts;
        _zombiePool = new ObjectPool(_zombiePrefab, _poolSize, false, _spawnPoint);
    }

    private void Start()
    {
        //Inicializa el pool de zombies
        _ghostSpawner.WaveFinished.AddListener(OnGhostWaveFinished);
    }

    // Método para spawnear un zombie en una posición específica
    public void SpawnZombie(Vector3 spawnPos, Quaternion rotation)
    {
        // Obtiene un zombie del pool
        var zombie = _zombiePool.Get() as ZombieController;

        if (!zombie) return;

        // Inicializa el zombie en la posición de spawn
        zombie.InitEnemy(spawnPos, rotation, this);
        _aliveEnemies++;
        _activeEnemies++;
    }
  
    // Método que se llama cuando un zombie muere o completa su recorrido
    public void DespawnEnemy(AEnemyController zombie)
    {
        _zombiePool.Return(zombie);
        _activeEnemies--;

        if (!zombie.IsAlive) _aliveEnemies--;

        if (_ghostWaveFinished && _activeEnemies == 0)
        {
            _ghostWaveFinished = false;
            _aliveEnemies = 0;
            WaveFinished.Invoke();//fin de la oleada 
        }
    }

    private void OnGhostWaveFinished()
    {
        _ghostWaveFinished = true;

        if(_activeEnemies == 0)
        {
            _ghostWaveFinished = false;
            _aliveEnemies = 0;
            WaveFinished.Invoke();//fin de la oleada 
        }
    }

    private void OnDestroy()
    {
        _ghostSpawner.WaveFinished.RemoveListener(OnGhostWaveFinished);
    }
}