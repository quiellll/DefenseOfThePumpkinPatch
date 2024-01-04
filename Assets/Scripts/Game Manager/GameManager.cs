using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//singleton que se encarga de manejar todo el juego, cambiar estados de juego, construir torretas,
//establecer el obj seleccionado
public class GameManager : Singleton<GameManager>
{

    public UnityEvent<int> PumpkinsChanged, GoldChanged;

    public AGameState GameState { get => _gameState; set => ChangeState(value); }
    public int Gold { get => _gold; set => SetGold(value); }
    public int Pumpkins { get => _pumpkins; set => SetPumpkins(value); }
    public bool IsOnDefense { get => _gameState.GetType() != typeof(BuildMode); }
    public int TimeScale { get; private set; }
    public bool Paused { get; set; }

    //spawners de cada tipo de enemigo
    public WaveSpawner FarmerWaveSpawner { get; private set; }
    public WaveSpawner GhostWaveSpawner { get; private set; }
    public ZombieSpawner ZombieSpawner { get; private set; }
    public HUDMenu HUD { get; private set; } //ref al hud
    public ShopMenu Shop { get; private set; }
    public SelectionManager SelectionManager { get; private set; }
    public CommandManager CommandManager { get; private set; }
    public BuildManager BuildManager { get; private set; }
    public ContextMenuManager ContextMenuManager { get; private set; }


    private AGameState _gameState;
    private int _gold;
    private int _pumpkins;


    protected override void Awake()
    {
        base.Awake();

        HUD = transform.parent.GetComponentInChildren<HUDMenu>();
        Shop = HUD.GetComponentInChildren<ShopMenu>(true);

        ContextMenuManager = transform.parent.GetComponentInChildren<ContextMenuManager>();

        foreach(var spawner in transform.parent.GetComponentsInChildren<WaveSpawner>())
        {
            if (spawner.EnemyPrefab as GhostController) GhostWaveSpawner = spawner;
            else if(spawner.EnemyPrefab as FarmerController) FarmerWaveSpawner = spawner;
        }

        ZombieSpawner = transform.parent.GetComponentInChildren<ZombieSpawner>();

        SelectionManager = new();
        CommandManager = new();
        BuildManager = new(SelectionManager);

        Gold = 200;
        TimeScale = 1;
    }

    private void Start()
    {
        GameState = new BuildMode(this); //inicia en construccion
    }

    private void Update()
    {
        BuildManager.DummyPlacing();
    }



    //cambia de estado de juego
    private void ChangeState(AGameState newState)
    {
        _gameState?.Exit(newState);
        var lastState = _gameState;
        _gameState = newState;
        _gameState.Enter(lastState);
    }

    private void SetGold(int amount)
    {
        if (amount < 0) return;
        GoldChanged.Invoke(amount);
        _gold = amount;
    }

    private void SetPumpkins(int amount)
    {
        if(amount < 0) return;
        PumpkinsChanged.Invoke(amount); 
        _pumpkins = amount;

        if(_pumpkins == 0)
        {
            //game over
        }
    }

    public void ToggleTimeScale()
    {
        TimeScale = TimeScale == 1 ? 2 : 1;
        Time.timeScale = TimeScale;
    }

    public GameObject SpawnDummy(GameObject prefab) => Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    public void DestroyDummy(GameObject dummy) => Destroy(dummy);

}
