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
    private Outline _outline;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void Start() //configuracion del outline
    {
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.white;
        _outline.OutlineWidth = 8f;
        _outline.enabled = false;
    }

    //cuando el raton este sobre el objeto establecerlo como el seleccionado
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.SetSelectedObject(this);
        _outline.enabled = true;
        Selected.Invoke();
    }

    //cuando salga el raton del objeto quitarlo como seleccionado
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.RemoveSelectedObject(this);
        _outline.enabled = false;
        Deselected.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }


}
