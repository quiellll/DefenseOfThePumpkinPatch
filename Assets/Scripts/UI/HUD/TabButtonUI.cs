using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    [SerializeField] private GameObject _shopTab;
    [SerializeField] private bool _defaultSelected;

    // Sistema de eventos (por si es necesario)
    public UnityEvent TabSelected;
    public UnityEvent TabDeselected;

    private TabGroupUI _tabGroup;
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _tabGroup = GetComponentInParent<TabGroupUI>();
        _tabGroup.AddTabButton(this);

        if (_defaultSelected) SelectButton();
    }

    // Eventos de puntero
    public void OnPointerClick(PointerEventData eventData) => SelectButton();

    private void SelectButton()
    {

        if (_tabGroup.SelectedTab == this) return;
        _shopTab.SetActive(true);
        _image.sprite = _tabGroup.TabActive;

        _tabGroup.SelectedTab?.ResetTabButton();
        _tabGroup.SelectedTab?.HideTab();
        _tabGroup.SelectedTab = this;
        TabSelected.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tabGroup.SelectedTab != this) _image.sprite = _tabGroup.TabHover;
        // _tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_tabGroup.SelectedTab != this) ResetTabButton();
        //_tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update

    public void ResetTabButton() => _image.sprite = _tabGroup.TabIdle;

    public void HideTab()
    {
        _shopTab.SetActive(false);
        TabDeselected.Invoke();
    }

    // Eventos
    // public void Select()
    // {
    //     onTabSelected?.Invoke();
    // }

    // public void Deselect()
    // {
    //     onTabDeselected?.Invoke();
    // }
}
