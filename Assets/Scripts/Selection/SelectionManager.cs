using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionManager
{
    public Selectable SelectedObject { get; private set; } //obeto seleccionable seleccionado 
    public GridCell SelectedCell { get; private set; }

    private TextMeshProUGUI _testTxt; //texto de debug para saber que objerto esta seleccionado actualmente

    public SelectionManager()
    {
        _testTxt = GameObject.Find("TestGMSelected").GetComponent<TextMeshProUGUI>();
    }


    //cuando se pasa el raton sobre un objeto seleccionable, se establece como el objeto seleccionado
    public void SetSelectedObject(Selectable selectable)
    {
        SelectedObject = selectable;
        if (SelectedObject.TryGetComponent<GridCell>(out var cell)) SelectedCell = cell;
        else SelectedCell = null;

        _testTxt.text = $"Selected Object: {SelectedObject}"; //debug
    }

    //cuando se hace quita el raton de un seleccionable, se quita
    public bool RemoveSelectedObject(Selectable selectable)
    {
        if (SelectedObject != selectable) return false;

        SelectedObject = null;
        SelectedCell = null;

        _testTxt.text = $"Selected Object: None"; //debug

        return true;
    }

    public bool ClickSelectedObject(Selectable selectable)
    {
        if(SelectedObject != selectable) return false;

        if (SelectedCell) return ClickSelectedCell();

        //aqui poner codigo de clickar las torretas, calabazas y otros objetos seleccionables

        return true;
    }

    private bool ClickSelectedCell()
    {
        ICommand command = null;
        switch(SelectedCell.Type)
        {
            case GridCell.CellType.Turret:
                command = new BuildTurret(GameManager.Instance.WareToBuild as Turret, SelectedCell);
                break;

            case GridCell.CellType.Pumpkin:
                command = new BuildPumpkinSprout(GameManager.Instance.WareToBuild as Pumpkin, SelectedCell);
                break;     
        }
        return GameManager.Instance.CommandManager.ExecuteCommand(command);
    }
}
