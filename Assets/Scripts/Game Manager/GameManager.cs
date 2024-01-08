using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//singleton que se encarga de manejar todo el juego, cambiar estados de juego, construir torretas,
//establecer el obj seleccionado
public class GameManager : Singleton<GameManager>
{

    public UnityEvent<int> PumpkinsChanged, GoldChanged;
    public UnityEvent StartDefenseMode, StartBuildMode;

    public Level Level { get => _level; }
    public AGameState GameState { get => _gameState; set => ChangeState(value); }
    public int Gold { get => _gold; set => SetGold(value); }
    public int Pumpkins { get => _pumpkins; set => SetPumpkins(value); }
    public bool IsOnDefense { get => _gameState.GetType() != typeof(BuildMode); }
    public int TimeScale { get; private set; }
    public bool Paused { get; set; }

    public FarmerSpawner FarmerSpawner { get; private set; }
    public GhostSpawner GhostSpawner { get; private set; }
    public ZombieSpawner ZombieSpawner { get; private set; }
    public HUDMenu HUD { get; private set; } //ref al hud
    public ShopMenu Shop { get; private set; }
    public SelectionManager SelectionManager { get; private set; }
    public CommandManager CommandManager { get; private set; }
    public BuildManager BuildManager { get; private set; }
    public ContextMenuManager ContextMenuManager { get; private set; }
    public CellManager CellManager { get; private set; }

    public ServiceLocator ServiceLocator { get => _serviceLocator; }

    
    
    [SerializeField] private Level _level;
    [SerializeField] private string _saveFileName;
    [SerializeField] private Turret[] _turrets;
    [SerializeField] private Pumpkin _pumpkin;
    [SerializeField] private bool _saveGame;
    [SerializeField] private bool _loadGame;


    private ServiceLocator _serviceLocator;

    private IGameDataUpdater _gameDataUpdater;
    private IGameDataSaver _gameDataSaver;
    private IAudioManager _audioManager;

    private AGameState _gameState;
    private int _gold;
    private int _pumpkins;

    private bool _nextIsDay;


    protected override void Awake()
    {
        base.Awake();


        HUD = transform.parent.GetComponentInChildren<HUDMenu>();
        Shop = HUD.GetComponentInChildren<ShopMenu>(true);
        ContextMenuManager = transform.parent.GetComponentInChildren<ContextMenuManager>();
        CellManager = transform.parent.GetComponentInChildren<CellManager>();
        FarmerSpawner = transform.parent.GetComponentInChildren<FarmerSpawner>();
        GhostSpawner = transform.parent.GetComponentInChildren<GhostSpawner>();
        ZombieSpawner = transform.parent.GetComponentInChildren<ZombieSpawner>();

        SelectionManager = new();
        CommandManager = new();
        BuildManager = new(SelectionManager);
        TimeScale = 1;


        _serviceLocator = new ServiceLocator();
        ServicesBootstraper.BootstrapServices();
        _gameDataSaver = _serviceLocator.Get<IGameDataSaver>();
        _gameDataUpdater = _serviceLocator.Get<IGameDataUpdater>();
        _audioManager = _serviceLocator.Get<IAudioManager>();
        _audioManager.PlayMusic();


        if (_loadGame && _gameDataSaver.ExistsSave(_saveFileName))
            if (LoadSaveToGame()) return;
        

        Gold = 200;
        _level.SetDay(0);
        _nextIsDay = true;
        
    }

    private void Start()
    {
        GameState = new BuildMode(this, _nextIsDay); //inicia en construccion
    }

    private void Update()
    {
        BuildManager.DummyPlacing();


        if(_gameDataUpdater.IsDirty() && !IsOnDefense)
        {
            if(_saveGame) _gameDataSaver.Save(_saveFileName, _gameDataUpdater.GetDataToSave<GameData>());
            _gameDataUpdater.ClearDirty();
        }
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

        _gameDataUpdater.UpdateGold(amount);
    }

    private void SetPumpkins(int amount)
    {
        if(amount < 0) return;
        PumpkinsChanged.Invoke(amount); 
        _pumpkins = amount;

        if(_pumpkins == 0)
        {
            HUD.GameOver();
        }
    }

    public void ToggleTimeScale()
    {
        TimeScale = TimeScale == 1 ? 2 : 1;
        Time.timeScale = TimeScale;
    }

    public GameObject SpawnDummy(GameObject prefab) => Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    public void DestroyDummy(GameObject dummy) => Destroy(dummy);


    public void AdvanceDay()
    {
        if (_level.NextDay() != null)
        {
            _gameDataUpdater.UpdateDay(_level.CurrentDayIndex);

            return;
        }

        //FIN DE NIVEL GG
    }


    private bool LoadSaveToGame()
    {

        bool loaded = _gameDataSaver.Load<GameData>(_saveFileName, out var data);

        if(!loaded) return false;

        Gold = data.Gold;

        _level.SetDay(data.DayIndex);

        _nextIsDay = data.NextDefenseIsDay;

        if(!_nextIsDay)
        {
            FindObjectOfType<LightingManager>().InitialTimePercent += 0.5f;
        }


        //pumpkins

        foreach(var pc in FindObjectsOfType<PumpkinController>(true))
        {
            Destroy(pc.gameObject);
        }

        foreach(var p in data.Pumpkins)
        {
            if(!data.ParseToVector2(p, out var pos)) continue;

            var cell = CellManager.GetCellAt(pos);
            if(cell == null) continue;
            cell.BuildPumpkinOnLoad(_pumpkin);
        }


        //sprouts

        foreach (var kv in data.Sprouts)
        {
            if (!data.ParseToVector2(kv.Key, out var pos)) continue;

            var cell = CellManager.GetCellAt(pos);
            if (cell == null) continue;

            cell.BuildWare(_pumpkin);

            //-1 porque justo en el start se llama el evento de build start y se le suma 1, entonces queda como se guardo (o no xd)
            cell.ElementOnTop.GetComponent<PumpkinSprout>().Journeys = kv.Value;// - 1; 
        }


        //turrets

        foreach (var kv in data.Turrets)
        {
            if (!data.ParseToVector2(kv.Key, out var pos)) continue;

            Turret turret = null;
            foreach(var t in _turrets)
                if(t.Name == kv.Value) turret = t;
            
            if(turret == null) continue;

            var cell = CellManager.GetCellAt(pos);
            if (cell == null) continue;

            cell.BuildWare(turret);
        }


        //graves

        foreach(var g in data.Graves)
        {
            if (!data.ParseToVector2(g.Item1, out var pos)) continue;

            var cell = CellManager.GetCellAt(pos);
            if (cell == null) continue;

            CellManager.BuildGrave(cell, pos, Quaternion.Euler(0f, g.Item2, 0f));

        }

        return true;
    }

}
