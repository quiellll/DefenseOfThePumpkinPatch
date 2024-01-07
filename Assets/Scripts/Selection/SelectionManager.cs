using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager
{
    public UnityEvent WareBuilt = new();

    public Selectable SelectedObject { get; private set; } //obeto seleccionable seleccionado 
    public GridCell SelectedCell { get; private set; }


    public SelectionManager()
    {
    }


    //cuando se pasa el raton sobre un objeto seleccionable, se establece como el objeto seleccionado
    public void SetSelectedObject(Selectable selectable)
    {
        SelectedObject = selectable;
        SelectedCell = selectable.GridCell; //null si no es una celda

    }

    //cuando se hace quita el raton de un seleccionable, se quita
    public bool RemoveSelectedObject(Selectable selectable)
    {
        if (SelectedObject != selectable) return false;

        SelectedObject = null;
        SelectedCell = null;


        return true;
    }

    public bool ClickSelectedObject(Selectable selectable)
    {
        if(SelectedObject != selectable) return false;

        if(SelectedCell != null ) return BuildWareOnCell();

        return ShowContextMenu();
    }

    private bool BuildWareOnCell()
    {
        ICommand command = null;

        if (SelectedCell.Type == GridCell.CellType.Turret || SelectedCell.Type == GridCell.CellType.Pumpkin)
        {
            command = new BuildWare(GameManager.Instance.BuildManager.WareToBuild, SelectedCell);
        }
        var built = command != null && GameManager.Instance.CommandManager.ExecuteCommand(command);

        if (built)
        {
            WareBuilt.Invoke();
        }

        return built;
    }

    private bool ShowContextMenu()
    {
        ICommand command = null;

        if(SelectedObject.TurretController != null)
            command = new ShowTurretContextMenu(SelectedObject.TurretController);

        else if (SelectedObject.PumpkinController != null)
            command = new ShowPumpkinContextMenu(SelectedObject.PumpkinController);

        else if (SelectedObject.PumpkinSprout != null)
            command = new ShowSproutContextMenu(SelectedObject.PumpkinSprout);


        return command != null && GameManager.Instance.CommandManager.ExecuteCommand(command);
    }
}
