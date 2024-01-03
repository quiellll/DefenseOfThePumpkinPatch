using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//clase para los objetos seleccionables
[RequireComponent(typeof(Outline), typeof(Collider))]
public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent Selected, Deselected;

    public GridCell GridCell { get; private set; }
    public ATurretController TurretController { get; private set; }
    public PumpkinController PumpkinController { get; private set; }
    public PumpkinSprout PumpkinSprout { get; private set; }

    private SelectionManager _selectionManager;
    private Outline _outline;


    private void Start() //configuracion del outline
    {
        _selectionManager = GameManager.Instance.SelectionManager;

        FindController();

        _outline = GetComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.white;
        _outline.OutlineWidth = 8f;
        _outline.enabled = false;
    }

    private void FindController()
    {
        GridCell = GetComponent<GridCell>();
        if (GridCell != null) return;

        TurretController = GetComponent<ATurretController>();
        if (TurretController != null) return;

        PumpkinController = GetComponent<PumpkinController>();
        if (PumpkinController != null) return;

        PumpkinSprout = GetComponent<PumpkinSprout>();
    }

    //cuando el raton este sobre el objeto establecerlo como el seleccionado
    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectionManager.SetSelectedObject(this);
        _outline.enabled = true;
        Selected.Invoke();
    }

    //cuando salga el raton del objeto quitarlo como seleccionado
    public void OnPointerExit(PointerEventData eventData)
    {
        _selectionManager.RemoveSelectedObject(this);
        _outline.enabled = false;
        Deselected.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _selectionManager.ClickSelectedObject(this);
    }


}
