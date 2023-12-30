using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroupUI : MonoBehaviour
{
    // Lista de los distintos botones
    public List<TabButtonUI> tabButtons;

    // Estados de las pestañas
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    // Referencia a la pestaña seleccionada
    public TabButtonUI selectedTab;

    // Lista de las pantallas del menú
    public List<GameObject> tabsToSwap;

    // Para suscribirse a los métodos de tabButtons
    public void AddListener(TabButtonUI button)
    {
        // es igual que un if == null...
        tabButtons ??= new List<TabButtonUI>();

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonUI button)
    {
        ResetTabs();

        // Cambia el sprite
        if (selectedTab == null || button != selectedTab) 
        { 
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButtonUI button)
    {
        ResetTabs();   
    }

    public void OnTabSelected(TabButtonUI button)
    {
        // Desmarca la pestaña que estuviese seleccionada
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        // Marca la pestaña seleccionada
        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActive;

        // IMPORTANTE: Vincula las pestañas y los botones del menú según
        // el orden en el que aparecen como hijos del grupo en el editor.
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i <tabsToSwap.Count; i++)
        {
            if (i == index)
            {
                tabsToSwap[i].SetActive(true);
            }
            else
            {
                tabsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButtonUI button in tabButtons) 
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.background.sprite = tabIdle;
        }
    }
}
