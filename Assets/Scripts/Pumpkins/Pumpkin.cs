using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName =("ScriptableObjects/Pumpkin"))]
public class Pumpkin : ScriptableObject, IWare
{
    //IWare
    public static readonly int SellPercentage = 75;
    public int BuyPrice { get => _buyPrice; }
    public int SellPrice { get => Mathf.FloorToInt(_buyPrice * SellPercentage / 100f); }
    public GameObject Prefab { get => _sproutPrefab; }
    public GameObject Dummy { get => _sproutDummy; }

    public GridCell.CellType CellType { get => GridCell.CellType.Pumpkin; }


    public GameObject PumpkinPrefab { get => _pumpkinPrefab; }
    public int SproutGrowthPeriod { get => _sproutGrowthPeriod; }

    public Sprite PumpkinIcon { get => _pumpkinIcon; }
    public Sprite SproutIcon { get => _sproutIcon; }



    [SerializeField] private int _buyPrice;
    [SerializeField] private int _sproutGrowthPeriod;
    [SerializeField] private GameObject _pumpkinPrefab;
    [SerializeField] private GameObject _sproutPrefab;
    [SerializeField] private GameObject _sproutDummy;
    [SerializeField] private Sprite _pumpkinIcon;
    [SerializeField] private Sprite _sproutIcon;
}