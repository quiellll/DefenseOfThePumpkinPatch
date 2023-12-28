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
    public Turret TurretToBuild { get => _turretToBuild; set => SetTurretToBuild(value); } //torreta establecida para construir
    
    //spawners de cada tipo de enemigo
    public WaveSpawner FarmerWaveSpawner { get; private set; }
    public WaveSpawner GhostWaveSpawner { get; private set; }
    public HUDMenu HUD { get; private set; } //ref al hud (botones de empezar oleada y construir torretas de momento)

    public Selectable SelectedObject { get; private set; } //obeto seleccionable seleccionado 

    private Turret _turretToBuild;
    private GameObject _turretDummy; //figura de la torreta semitransparente para elegir donde construirla

    private GridCell _selectedCell; //celda seleccionada para construir la torreta

    private TextMeshProUGUI _testTxt; //texto de debug para saber que objerto esta seleccionado actualmente

    
    private AGameState _gameState;

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
    }

    private void Start()
    {
        GameState = new BuildMode(this); //inicia en contruccion
    }

    private void Update()
    {
        _testTxt.text = $"Selected Object: {SelectedObject}"; //debug

        TurretPlacing();
    }

    //cambia de estado de juego
    private void ChangeState(AGameState newState)
    {
        _gameState?.Exit(newState);
        var lastState = _gameState;
        _gameState = newState;
        _gameState.Enter(lastState);
    }

    #region Turrets

    //comprueba si se ha elegido construir una torreta y mueve el dummy, lo desactiva o activa segun corresponda
    private void TurretPlacing()
    {
        if (!_turretToBuild || !_turretDummy) return;

        if (!_selectedCell)
        {
            if (_turretDummy.activeSelf) _turretDummy.SetActive(false);
            return;
        }

        if (!_turretDummy.activeSelf) _turretDummy.SetActive(true);
        _turretDummy.transform.position = _selectedCell.transform.position + Vector3.up * 0.1f;
    }

    //se llama con el setter de TurretToBuild, instancia el dummy cuando se decide connstruir una torreta
    private void SetTurretToBuild(Turret turret)
    {
        _turretToBuild = turret;
        _turretDummy = Instantiate(turret.Dummy, Vector3.zero, turret.Dummy.transform.rotation);

        if(SelectedObject && SelectedObject.TryGetComponent<GridCell>(out var cell))
        {
            _turretDummy.transform.position = cell.transform.position;
            return;
        }        
        
        _turretDummy.SetActive(false);
    }

    //se llama cuando se hace clic para construir en un sitio valido, construye la torreta y
    //destruye el dummy
    public void Build(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if(!_turretToBuild || !_turretDummy || !_turretDummy.activeSelf || !_selectedCell) return;


        bool built = _selectedCell.BuildTurret(_turretToBuild);

        if (!built) return;

        Destroy(_turretDummy);
        _turretDummy = null;
        _turretToBuild = null;
    }
    #endregion


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
