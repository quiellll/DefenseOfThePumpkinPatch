using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    // Referencia al grupo al que pertenecen
    public TabGroupUI tabGroup;

    // Imagen de fondo
    public Image background;

    // Sistema de eventos (por si es necesario)
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    // Eventos de puntero
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.AddListener(this);
    }

    // Eventos
    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke();
    }
}
