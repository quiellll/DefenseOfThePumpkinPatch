using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR;

public class GameManager : Singleton<GameManager>
{
    public AGameState GameState { get => _gameState;  set => ChangeState(value); }
    public bool IsOnDefense { get => _gameState.GetType() != typeof(BuildMode); }
    public Turret TurretToBuild {get => _turretToBuild; set => SetTurretToBuild(value); }
    public WaveSpawner WaveSpawner { get; private set; }
    public HUDMenu HUD { get; private set; }

    public Selectable SelectedObject { get; private set; }

    private Turret _turretToBuild;
    private GameObject _turretDummy;

    private GridCell _selectedCell;

    private TextMeshProUGUI _testTxt;

    
    private AGameState _gameState;

    protected override void Awake()
    {
        base.Awake();

        WaveSpawner = transform.parent.GetComponentInChildren<WaveSpawner>();
        HUD = transform.parent.GetComponentInChildren<HUDMenu>();

        _testTxt = GameObject.Find("TestGMSelected").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameState = new BuildMode(this);
    }

    private void Update()
    {
        _testTxt.text = $"Selected Object: {SelectedObject}"; //debug

        TurretPlacing();
    }

    private void ChangeState(AGameState newState)
    {
        _gameState?.Exit(newState);
        var lastState = _gameState;
        _gameState = newState;
        _gameState.Enter(lastState);
    }

    #region Turrets
    private void TurretPlacing()
    {
        if (!_turretToBuild || !_turretDummy) return;

        if (!_selectedCell)
        {
            if (_turretDummy.activeSelf) _turretDummy.SetActive(false);
            return;
        }

        if (!_turretDummy.activeSelf) _turretDummy.SetActive(true);
        _turretDummy.transform.position = _selectedCell.transform.position;
    }

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
    public void SetSelectedObject(Selectable selectable)
    {
        SelectedObject = selectable;
        if(SelectedObject.TryGetComponent<GridCell>(out var cell))
            _selectedCell = cell;
        else _selectedCell = null;
    }

    public bool RemoveSelectedObject(Selectable selectable)
    {
        if(SelectedObject != selectable) return false;

        SelectedObject = null;
        _selectedCell = null;
        return true;
    }
    #endregion

}
