using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetTurretToBuy : ICommand
{
    public bool Undoable { get => false; }
    private Turret _turret;
    public SetTurretToBuy(Turret turret)
    {
        _turret = turret;
    }
    public void Execute()
    {
        GameManager.Instance.SetTurretToBuild(_turret);
    }
    public void Undo() { }
}


public class RemoveTurretToBuy : ICommand
{
    public bool Undoable { get => false; }

    public void Execute()
    {
        GameManager.Instance.RemoveTurretToBuild();
    }
    public void Undo() { }
}



public class BuildTurret : ICommand
{
    public bool Undoable { get => true; }
    private Turret _turret;
    private GridCell _cell;

    public BuildTurret(Turret turret, GridCell cell)
    {
        _turret = turret;
        _cell = cell;
    }

    public void Execute()
    {
        _cell.BuildTurret(_turret);
    }
    public void Undo()
    {
        _cell.RemoveTurret();
    }
}


public class SellTurret : ICommand
{
    public bool Undoable { get => true; }
    private Turret _turret;
    private GridCell _cell;

    public SellTurret(Turret turret, GridCell cell)
    {
        _turret = turret;
        _cell = cell;
    }

    public void Execute()
    {
        _cell.RemoveTurret();
    }
    public void Undo()
    {
        _cell.BuildTurret(_turret);
    }
}
 
