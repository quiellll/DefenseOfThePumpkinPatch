using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;



public class TurretContextMenu : MonoBehaviour
{
    public Turret Turret {  get; private set; }
    public bool Active => gameObject.activeSelf;
    public RectTransform RectTransform {  get; private set; }

    [SerializeField] private Image _balistaIcon;
    [SerializeField] private Image _catapultIcon;
    [SerializeField] private Image _magicIcon;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
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
        //_name.text = Turret.Description;
        _name.text = Turret.name;
        _damage.text = "Attack damage: " + Turret.Damage.ToString();
        _attackSpeed.text = "Attack Speed: " + Turret.FireRate.ToString();
        _sellPrice.text = "Sell Price: " + Turret.SellPrice.ToString();

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
        GameManager.Instance.HUD.UpdateUndoButton();
    }



}
