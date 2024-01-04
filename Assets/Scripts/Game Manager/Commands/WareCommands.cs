using UnityEngine;

public class SetWareToBuy : ICommand
{
    public bool Undoable => false;
    private IWare _ware;
    public SetWareToBuy(IWare Ware)
    {
        _ware = Ware;
    }
    public bool Execute()
    {
        if (GameManager.Instance.Gold - _ware.BuyPrice < 0) return false;
        GameManager.Instance.BuildManager.SetWareToBuild(_ware);
        return true;
    }
    public void Undo() { }
}


public class RemoveWareToBuy : ICommand
{
    public bool Undoable => false;

    public bool Execute()
    {
        GameManager.Instance.BuildManager.RemoveWareToBuild(); return true;
    }
    public void Undo() { }
}



public  class BuildWare : ICommand
{
    public bool Undoable => true;
    protected IWare _ware;
    protected GridCell _cell;

    public BuildWare(IWare Ware, GridCell cell)
    {
        _ware = Ware;
        _cell = cell;
    }

    public bool Execute()
    {
        if (!GameManager.Instance.BuildManager.CanBuildWare(_ware)) return false;

        if(Build())
        {
            GameManager.Instance.BuildManager.RemoveWareToBuild();
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

    protected virtual bool Build() => _cell.BuildWare(_ware);
}


public class SellWare : ICommand
{
    public bool Undoable => true;
    protected IWare _ware;
    protected GridCell _cell;

    public SellWare(IWare Ware, GridCell cell)
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

    protected virtual void Build() => _cell.BuildWare(_ware);
}



public class SellPumpkin : SellWare
{
    public SellPumpkin(Pumpkin pumpkin, GridCell cell) : base(pumpkin, cell) { }

    public override bool Execute()
    {
        if (_cell.Type != GridCell.CellType.Pumpkin || !_cell.ElementOnTop ||
            _cell.ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) return false;

        return base.Execute();
    }
    protected override void Build() => _cell.BuildWare(_ware, (_ware as Pumpkin).PumpkinPrefab);
}
