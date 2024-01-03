using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

//https://discussions.unity.com/t/how-to-convert-from-world-space-to-canvas-space/117981

public class TurretContextMenu : MonoBehaviour
{
    public Turret Turret {  get; private set; }
    public bool Active => gameObject.activeSelf;
    public RectTransform RectTransform {  get; private set; }

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _sellPrice;

    private GridCell _turretCell;


    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public bool Display(ATurretController turretController)
    {
        if (Active && Turret == turretController.Turret) return false;

        if (!Active) gameObject.SetActive(true);

        Turret = turretController.Turret;
        _turretCell = turretController.Cell;

        _icon.sprite = Turret.Icon;
        _description.text = Turret.Description;
        _damage.text = Turret.Damage.ToString();
        _attackSpeed.text = Turret.FireRate.ToString();
        _sellPrice.text = Turret.SellPrice.ToString();

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
        GameManager.Instance.CommandManager.ExecuteCommand(new SellWare(Turret, _turretCell));
    }


}
