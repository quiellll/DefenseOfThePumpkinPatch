using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//flyweight de la torreta con los stats comunes
[CreateAssetMenu(menuName = "ScriptableObjects/Turret")]
public class Turret : ScriptableObject, IWare
{
    //IWare
    public static readonly int SellPercentage = 75;

    public int BuyPrice { get => _buyPrice; }
    public int SellPrice { get => Mathf.FloorToInt(_buyPrice * SellPercentage / 100f); }
    // Prefab de la torreta
    public GameObject Prefab { get => _turretPrefab; }
    // Prefab de placeholder
    public GameObject Dummy { get => _dummyPrefab; }
    public GridCell.CellType CellType { get => GridCell.CellType.Turret; }
    // Prefab del proyectil a disparar
    public ATurretProjectile ProjectilePrefab { get => _projectilePrefab; }
    public float RotationSpeed { get => _rotationSpeed; } //velocidad de rotacion
    //los enemigos a menos de este radio no son targeteados
    public float InnerRadius { get => _innerRadius; } 
    //los enemigos a mas de este radio no son targeteados
    public float OuterRadius { get => _outerRadius; }
    public float FireRate { get => _fireRate; } //disparos por segundo
    public float ProjectileMoveSpeed {  get => _projectileMoveSpeed; }
    public int Damage { get => _damage; }
    public Sprite Icon { get => _icon; }
    //public string Description { get => _description; }
    public EnemyTarget ValidTargets { get => _validTargets; }



    [SerializeField] private int _buyPrice;
    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _dummyPrefab;
    [SerializeField] private ATurretProjectile _projectilePrefab;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _innerRadius;
    [SerializeField] private float _outerRadius;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _projectileMoveSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private Sprite _icon;
    //[SerializeField] private string _description
    [SerializeField] private EnemyTarget _validTargets;

    [Flags] public enum EnemyTarget
    {
        None = 0,
        Farmer = 1,
        Ghost = 2,
        Zombie = 4,
    }


    public bool CanTarget(AEnemyController enemy)
    {
        EnemyTarget target;

        if (enemy is FarmerController) target = EnemyTarget.Farmer;
        else if (enemy is GhostController) target = EnemyTarget.Ghost;
        else if(enemy is ZombieController) target = EnemyTarget.Zombie;
        else return false;

        return _validTargets.HasFlag(target);
    }
}
