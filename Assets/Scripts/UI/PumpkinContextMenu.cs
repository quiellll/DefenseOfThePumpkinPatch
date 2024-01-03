using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PumpkinContextMenu : MonoBehaviour
{
    public Pumpkin Pumpkin { get; private set; }
    public bool Active => gameObject.activeSelf;
    public RectTransform RectTransform { get; private set; }


    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _sellPrice;

    private GridCell _pumpkinCell;


    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
    
    public bool Display(PumpkinController pumpkinController)
    {
        if (Active && Pumpkin == pumpkinController.Pumpkin) return false;

        if (!Active) gameObject.SetActive(true);

        Pumpkin = pumpkinController.Pumpkin;
        _pumpkinCell = pumpkinController.Cell;

        _icon.sprite = Pumpkin.PumpkinIcon;
        _sellPrice.text = Pumpkin.SellPrice.ToString();

        return true;
    }

    public bool Hide()
    {
        if (!Active) return false;

        gameObject.SetActive(false);
        return true;
    }

    //asignar al boton de vender del context menu
    public void Sell()
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SellWare(Pumpkin, _pumpkinCell));
    }
}
