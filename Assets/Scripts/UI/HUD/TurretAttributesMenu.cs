using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretAttributesMenu : MonoBehaviour
{
    public Turret TurretType;

    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _attackSpeed;
    [SerializeField] private TextMeshProUGUI _buyPrice;
    [SerializeField] private Image[] _targets;
    [SerializeField] private Sprite _farmerSprite;
    [SerializeField] private Sprite _ghostSprite;
    [SerializeField] private Sprite _zombieSprite;

    // Start is called before the first frame update
    void Start()
    {
        _damage.text = "Attack damage: " + TurretType.Damage.ToString();
        _attackSpeed.text = "Attack Speed: " + TurretType.FireRate.ToString();



        _buyPrice.text = "Buy Price: " + TurretType.BuyPrice.ToString();


        //set targets sprites
        int i = 0;
        if (TurretType.CanTarget(Turret.EnemyTarget.Farmer)) _targets[i++].sprite = _farmerSprite;
        if (TurretType.CanTarget(Turret.EnemyTarget.Ghost)) _targets[i++].sprite = _ghostSprite;
        if (TurretType.CanTarget(Turret.EnemyTarget.Zombie)) _targets[i++].sprite = _zombieSprite;


        foreach(var target in _targets) if(target.sprite == null) target.enabled = false;
    }
}
