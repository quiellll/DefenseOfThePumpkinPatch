using UnityEngine;

public class ShowTurretContextMenu : ICommand
{
    public bool Undoable => false;
    private ATurretController _turetController;
    public ShowTurretContextMenu(ATurretController turretController)
    {
        _turetController = turretController;
    }

    public bool Execute()
    {
        return GameManager.Instance.ContextMenuManager.DisplayTurretMenu(_turetController);
    }

    public void Undo() {}
}

public class ShowPumpkinContextMenu : ICommand
{
    public bool Undoable => false;
    private PumpkinController _pumpkinController;
    public ShowPumpkinContextMenu(PumpkinController pumpkinController)
    {
        _pumpkinController = pumpkinController;
    }

    public bool Execute()
    {
        return GameManager.Instance.ContextMenuManager.DisplayPumpkinMenu(_pumpkinController);
    }

    public void Undo() { }
}

public class ShowSproutContextMenu : ICommand
{
    public bool Undoable => false;
    private PumpkinSprout _sproutController;
    public ShowSproutContextMenu(PumpkinSprout sprout)
    {
        _sproutController = sprout;
    }

    public bool Execute()
    {
        return GameManager.Instance.ContextMenuManager.DisplaySproutMenu(_sproutController);
    }

    public void Undo() { }
}

public class HideActiveContextMenu : ICommand
{
    public bool Undoable => false;
    public HideActiveContextMenu() { }

    public bool Execute()
    {
        return GameManager.Instance.ContextMenuManager.HideActiveMenu();
    }

    public void Undo() { }
}