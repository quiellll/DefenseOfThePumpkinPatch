using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretAttributesMenu : MonoBehaviour
{
    public Turret TurretType;

    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _buyPrice;

    // Start is called before the first frame update
    void Start()
    {
        _damage.text = "Attack damage: " + TurretType.Damage.ToString();
        _attackSpeed.text = "Attack Speed: " + TurretType.FireRate.ToString();



        _buyPrice.text = "Buy Price: " + TurretType.BuyPrice.ToString();
    }
}
