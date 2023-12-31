using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//singleton que se encarga de manejar todo el juego, cambiar estados de juego, construir torretas,
//establecer el obj seleccionado
public class GameManager : Singleton<GameManager>
{
    public AGameState GameState { get => _gameState; set => ChangeState(value); }
    public int Gold { get => _gold; set => SetGold(value); }
    public int Pumpkins { get => _pumpkins; set => SetPumpkins(value); }
    public bool IsOnDefense { get => _gameState.GetType() != typeof(BuildMode); }
    public IWare WareToBuild { get => _wareToBuild; }
    //spawners de cada tipo de enemigo
    public WaveSpawner FarmerWaveSpawner { get; private set; }
    public WaveSpawner GhostWaveSpawner { get; private set; }
    public ZombieSpawner ZombieSpawner { get; private set; }
    public HUDMenu HUD { get; private set; } //ref al hud (botones de empezar oleada y construir torretas de momento)
    public SelectionManager SelectionManager { get => _selectionManager; }
    public CommandManager CommandManager { get => _commandManager; }


    private IWare _wareToBuild;
    private GameObject _dummy; //figura de la torreta o brote de calabaza semitransparente para elegir donde construirla
    
    private AGameState _gameState;
    private int _gold;
    private int _pumpkins;

    private SelectionManager _selectionManager;
    private CommandManager _commandManager;


    protected override void Awake()
    {
        base.Awake();

        HUD = transform.parent.GetComponentInChildren<HUDMenu>();

        foreach(var spawner in transform.parent.GetComponentsInChildren<WaveSpawner>())
        {
            if (spawner.EnemyPrefab as GhostController) GhostWaveSpawner = spawner;
            else if(spawner.EnemyPrefab as FarmerController) FarmerWaveSpawner = spawner;
        }

        ZombieSpawner = transform.parent.GetComponentInChildren<ZombieSpawner>();

        _selectionManager = new();
        _commandManager = new();

        Gold = 200;
    }

    private void Start()
    {
        GameState = new BuildMode(this); //inicia en contruccion
    }

    private void Update()
    {
        DummyPlacing();
        Debug.Log(Gold);
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

        _gold = amount;
    }

    private void SetPumpkins(int amount)
    {
        if(amount < 0) return;
        _pumpkins = amount;

        if(_pumpkins == 0)
        {
            //game over
        }
    }


    //comprueba si se ha elegido construir una torreta o calabaza y mueve el dummy, lo desactiva o activa segun corresponda
    private void DummyPlacing()
    {
        if (!_dummy) return;

        if (!_selectionManager.SelectedCell)
        {
            if (_dummy.activeSelf) _dummy.SetActive(false);
            return;
        }

        if (!_dummy.activeSelf) _dummy.SetActive(true);
        _dummy.transform.position = _selectionManager.SelectedCell.transform.position + Vector3.up * 0.1f;
    }

    //instancia el dummy cuando se decide construir una torreta o calabaza
    public void SetWareToBuild(IWare ware)
    {
        if (_dummy != null)
        {
            Destroy(_dummy);
        }

        _wareToBuild = ware;
        _dummy = Instantiate(ware.Dummy, Vector3.zero, ware.Dummy.transform.rotation);


        if (_selectionManager.SelectedObject && _selectionManager.SelectedCell
            && _selectionManager.SelectedCell.Type == _wareToBuild.CellType)
        {
            _dummy.transform.position = _selectionManager.SelectedCell.transform.position;
            return;
        }

        _dummy.SetActive(false);
    }

    public void RemoveWareToBuild()
    {
        if (_dummy != null)
        {
            Destroy(_dummy);
            _dummy = null;
        }
        _wareToBuild = null;
    }

    public bool CanBuildWare(IWare ware)
    {
        return 
            _wareToBuild != null && 
            _wareToBuild == ware && 
            _dummy != null && 
            _dummy.activeSelf && 
            _selectionManager.SelectedCell != null && 
            _selectionManager.SelectedCell.ElementOnTop == null && 
            _selectionManager.SelectedCell.Type == _wareToBuild.CellType && 
            (Gold - _wareToBuild.BuyPrice) >= 0;
    }

}
