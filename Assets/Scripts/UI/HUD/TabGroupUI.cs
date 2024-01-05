using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroupUI : MonoBehaviour
{

    public Sprite TabIdle {get => _tabIdle; }
    public Sprite TabHover {get => _tabHover; }
    public Sprite TabActive {get => _tabActive; }

    public TabButtonUI SelectedTab{get; set;}


    // Estados de las pesta�as
    [SerializeField] private Sprite _tabIdle;
    [SerializeField] private Sprite _tabHover;
    [SerializeField] private Sprite _tabActive; 

    private List<TabButtonUI> _tabButtons;
    // Referencia a la pesta�a seleccionada
    // public TabButtonUI selectedTab;

    // Lista de las pantallas del men�
    // public List<GameObject> tabsToSwap;


    private void Awake()
    {
        _tabButtons = new List<TabButtonUI>();
    }

    public void AddTabButton(TabButtonUI button)
    {
        _tabButtons.Add(button);
    }


    // Para suscribirse a los m�todos de tabButtons

    // public void OnTabEnter(TabButtonUI button)
    // {
    //     ResetTabs();

    //     // Cambia el sprite
    //     //if (selectedTab == null || button != selectedTab) 
    //         // button.background.sprite = tabHover;
        
    // }

    // public void OnTabExit(TabButtonUI button)
    // {
    //     ResetTabs();   
    // }

    // public void OnTabSelected(TabButtonUI button)
    // {
    //     // Desmarca la pesta�a que estuviese seleccionada
    //     // if(selectedTab != null)
    //     // {
    //     //     selectedTab.Deselect();
    //     // }

    //     // Marca la pesta�a seleccionada
    //     selectedTab = button;
    //     //selectedTab.Select();

    //     //ResetTabs();
    //     button.background.sprite = tabActive;

    //     // IMPORTANTE: Vincula las pesta�as y los botones del men� seg�n
    //     // el orden en el que aparecen como hijos del grupo en el editor.
    //     int index = button.transform.GetSiblingIndex();
    //     for(int i = 0; i <tabsToSwap.Count; i++)
    //     {
    //         if (i == index)
    //         {
    //             tabsToSwap[i].SetActive(true);
    //         }
    //         else
    //         {
    //             tabsToSwap[i].SetActive(false);
    //         }
    //     }
    // }

    // public void ResetTabs()
    // {
    //     foreach(TabButtonUI button in tabButtons) 
    //     {
    //         // if (selectedTab != null && button == selectedTab) continue;
    //         // button.background.sprite = tabIdle;

    //         if(button != SelectedTab) button.ResetTab();
    //     }
    // }
}
