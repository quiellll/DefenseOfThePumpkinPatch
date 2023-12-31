using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : MonoBehaviour, IEnemySpawner
{
    //evento llamado cuando no queden zombies por spawnear ni zombies activos
    public UnityEvent WaveFinished;

    public ZombieController ZombiePrefab { get => _zombiePrefab; } //propiedad que devuelve el prefab del zombie
    

    [SerializeField] private Transform _spawnPoint; //punto donde agrupar a los enemigos
    [SerializeField] private ZombieController _zombiePrefab; //prefab del zombie
    
    private int _reachedGraves; // fantasmas que han llegado a la tumba;

    private WorldGrid.GraveAtPath _grave; //tumba

    private GridCell _graveCell; //celda en la que esta la tumbs
    private bool _reachedDestination = false; //si ha llegado al destino (tumba o ultimo waypoint del camino)

    private ObjectPool _zombiePool;

    private bool _spawnFinished = false;
    private int _aliveEnemies = 0;
    private int _activeEnemies = 0;

    private void Awake()
    {
        //Inicializa el pool de zombies
        _zombiePool = new ObjectPool(_zombiePrefab, _reachedGraves, false, _spawnPoint);
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

        // Si no hay tumbas ni fantasmas se termina el spawn
        if (!_spawnFinished)
        {
            _spawnFinished = true; // Fin de spawn
            WaveFinished.Invoke();
        }
    }
  
    // Método que se llama cuando un zombie muere o completa su recorrido
    public void DespawnEnemy(AEnemyController zombie)
    {
        _zombiePool.Return(zombie);
        _activeEnemies--;

        if (!zombie.IsAlive) _aliveEnemies--;

        if (_spawnFinished && _activeEnemies == 0)
        {
            WaveFinished.Invoke();//fin de la oleada 
        }
    }
}