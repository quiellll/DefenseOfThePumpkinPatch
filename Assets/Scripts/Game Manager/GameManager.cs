using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR;

//singleton que se encarga de manejar todo el juego, cambiar estados de juego, construir torretas,
//establecer el obj seleccionado
public class GameManager : Singleton<GameManager>
{
    public AGameState GameState { get => _gameState; set => ChangeState(value); }
    public bool IsOnDefense { get => _gameState.GetType() != typeof(BuildMode); }
    public IWare WareToBuild { get => _wareToBuild; }
    //spawners de cada tipo de enemigo
    public WaveSpawner FarmerWaveSpawner { get; private set; }
    public WaveSpawner GhostWaveSpawner { get; private set; }
    public HUDMenu HUD { get; private set; } //ref al hud (botones de empezar oleada y construir torretas de momento)

    public Selectable SelectedObject { get; private set; } //obeto seleccionable seleccionado 
    public CommandManager CommandManager { get => _commandManager; }
    public int Gold { get => _gold; }

    private IWare _wareToBuild;
    private GameObject _dummy; //figura de la torreta o brote de calabaza semitransparente para elegir donde construirla

    private GridCell _selectedCell; //celda seleccionada para construir la torreta

    private TextMeshProUGUI _testTxt; //texto de debug para saber que objerto esta seleccionado actualmente

    
    private AGameState _gameState;

    private CommandManager _commandManager;

    private int _gold;

    protected override void Awake()
    {
        base.Awake();

        HUD = transform.parent.GetComponentInChildren<HUDMenu>();

        _testTxt = GameObject.Find("TestGMSelected").GetComponent<TextMeshProUGUI>();

        foreach(var spawner in transform.parent.GetComponentsInChildren<WaveSpawner>())
        {
            if (spawner.EnemyPrefab as GhostController) GhostWaveSpawner = spawner;
            else if(spawner.EnemyPrefab as FarmerController) FarmerWaveSpawner = spawner;
        }

        _commandManager = new();
    }

    private void Start()
    {
        GameState = new BuildMode(this); //inicia en contruccion
    }

    private void Update()
    {
        _testTxt.text = $"Selected Object: {SelectedObject}"; //debug

        DummyPlacing();
    }

    //cambia de estado de juego
    private void ChangeState(AGameState newState)
    {
        _gameState?.Exit(newState);
        var lastState = _gameState;
        _gameState = newState;
        _gameState.Enter(lastState);
    }


    //comprueba si se ha elegido construir una torreta y mueve el dummy, lo desactiva o activa segun corresponda
    private void DummyPlacing()
    {
        if (!_dummy) return;

        if (!_selectedCell)
        {
            if (_dummy.activeSelf) _dummy.SetActive(false);
            return;
        }

        if (!_dummy.activeSelf) _dummy.SetActive(true);
        _dummy.transform.position = _selectedCell.transform.position + Vector3.up * 0.1f;
    }

    //instancia el dummy cuando se decide connstruir una torreta
    public void SetWareToBuild(IWare ware)
    {
        if (_dummy != null)
        {
            Destroy(_dummy);
        }

        _wareToBuild = ware;
        _dummy = Instantiate(ware.Dummy, Vector3.zero, ware.Dummy.transform.rotation);


        if (SelectedObject && SelectedObject.TryGetComponent<GridCell>(out var cell) && cell.Type == _wareToBuild.CellType)
        {
            _dummy.transform.position = cell.transform.position;
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
        return _wareToBuild != null && _wareToBuild == ware && _dummy != null && _dummy.activeSelf && _selectedCell != null
            && _selectedCell.ElementOnTop == null && _selectedCell.Type == _wareToBuild.CellType;
    }

    //#region Turrets
    //se llama cuando se hace clic para construir en un sitio valido, construye la torreta y
    //destruye el dummy
    //public void BuildTurret(InputAction.CallbackContext context)
    //{
    //    if (!context.started) return;
    //    if(!_turretToBuild || !_turretDummy || !_turretDummy.activeSelf || !_selectedCell) return;

    //    if(_selectedCell.ElementOnTop || _selectedCell.Type != GridCell.CellType.Turret) return;

    //    CommandManager.ExecuteCommand(new BuildTurret(_turretToBuild, _selectedCell));

    //    Destroy(_turretDummy);
    //    _turretDummy = null;
    //    _turretToBuild = null;
    //}
    //#endregion

    #region Selection
    //cuando se pasa el raton sobre un objeto seleccionable, se establece como el objeto seleccionado
    public void SetSelectedObject(Selectable selectable)
    {
        SelectedObject = selectable;
        if(SelectedObject.TryGetComponent<GridCell>(out var cell))
            _selectedCell = cell;
        else _selectedCell = null;
    }

    //cuando se hace quita el raton de un seleccionable, se quita
    public bool RemoveSelectedObject(Selectable selectable)
    {
        if(SelectedObject != selectable) return false;

        SelectedObject = null;
        _selectedCell = null;
        return true;
    }
    #endregion

}
