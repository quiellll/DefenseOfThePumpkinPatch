using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public abstract class ASetWareToBuy : ICommand
{
    public bool Undoable { get => false; }
    private IWare _ware;
    public ASetWareToBuy(IWare Ware)
    {
        _ware = Ware;
    }
    public bool Execute()
    {
        if (GameManager.Instance.Gold - _ware.BuyPrice < 0) return false;
        GameManager.Instance.SetWareToBuild(_ware);
        return true;
    }
    public void Undo() { }
}


public abstract class ARemoveWareToBuy : ICommand
{
    public bool Undoable { get => false; }

    public bool Execute()
    {
        GameManager.Instance.RemoveWareToBuild(); return true;
    }
    public void Undo() { }
}



public abstract class ABuildWare : ICommand
{
    public bool Undoable { get => true; }
    protected IWare _ware;
    protected GridCell _cell;

    public ABuildWare(IWare Ware, GridCell cell)
    {
        _ware = Ware;
        _cell = cell;
    }

    public bool Execute()
    {
        if (!GameManager.Instance.CanBuildWare(_ware)) return false;

        if(Build())
        {
            GameManager.Instance.RemoveWareToBuild();
            GameManager.Instance.Gold -= _ware.BuyPrice;
            return true;
        }

        return false;
    }
    public void Undo()
    {
        _cell.DestroyWare();
        GameManager.Instance.Gold += _ware.BuyPrice;
    }

    protected abstract bool Build();
}


public abstract class ASellWare : ICommand
{
    public bool Undoable { get => true; }
    protected IWare _ware;
    protected GridCell _cell;

    public ASellWare(IWare Ware, GridCell cell)
    {
        _ware = Ware;
        _cell = cell;
    }

    public virtual bool Execute()
    {
        if (_cell.DestroyWare())
        {
            GameManager.Instance.Gold += _ware.SellPrice;
            return true;
        }
        return false;
    }
    public void Undo()
    {
        Build();
        GameManager.Instance.Gold -= _ware.SellPrice;
    }

    protected abstract void Build();
}




public class SetTurretToBuy : ASetWareToBuy { public SetTurretToBuy(Turret turret) : base(turret) { } }

public class RemoveTurretToBuy : ARemoveWareToBuy { public RemoveTurretToBuy() : base() { } }

public class BuildTurret : ABuildWare
{
    public BuildTurret(Turret turret, GridCell cell) : base(turret, cell) { }
    protected override bool Build() => _cell.BuildWare(_ware);
}

public class SellTurret : ASellWare
{
    public SellTurret(Turret turret, GridCell cell) : base(turret, cell) { }
    protected override void Build() => _cell.BuildWare(_ware);
}


public class SetPumpkinSproutToBuy : ASetWareToBuy { public SetPumpkinSproutToBuy(Pumpkin pumpkin) : base(pumpkin) { } }

public class RemovePumpkinSproutToBuy : ARemoveWareToBuy { public RemovePumpkinSproutToBuy() : base() { } }

public class BuildPumpkinSprout : ABuildWare
{
    public BuildPumpkinSprout(Pumpkin pumpkin, GridCell cell) : base(pumpkin, cell) { }
    protected override bool Build() => _cell.BuildWare(_ware);
}

public class SellPumpkin : ASellWare
{
    public SellPumpkin(Pumpkin pumpkin, GridCell cell) : base(pumpkin, cell) { }
    protected override void Build() => _cell.BuildWare(_ware, (_ware as Pumpkin).PumpkinPrefab);

    public override bool Execute()
    {
        if (_cell.Type != GridCell.CellType.Pumpkin || !_cell.ElementOnTop ||
            _cell.ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) return false;

        if(!base.Execute()) return false;

        //WorldGrid.Instance.RemovePumpkin(_cell);
        return true;
    }
}
