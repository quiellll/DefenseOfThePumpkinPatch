using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public Turret TurretToBuild {get => _turretToBuild; set => SetTurretToBuild(value); }

    public Selectable SelectedObject { get; private set; }

    private Turret _turretToBuild;
    private GameObject _turretDummy;

    private GridCell _selectedCell;

    private TextMeshProUGUI _testTxt;

    protected override void Awake()
    {
        base.Awake();
        _testTxt = GameObject.Find("TestGMSelected").GetComponent<TextMeshProUGUI>();
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


    private void Update()
    {
        _testTxt.text = $"Selected Object: {SelectedObject}";
        if (!_turretToBuild || !_turretDummy) return;

        if (!_selectedCell)
        {
            if(_turretDummy.activeSelf) _turretDummy.SetActive(false);
            return;
        }

        if(!_turretDummy.activeSelf) _turretDummy.SetActive(true);
        _turretDummy.transform.position = _selectedCell.transform.position;

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

}
