
using UnityEngine;

public interface IWare
{
    public int BuyPrice { get; }
    public int SellPrice { get; }
    public GameObject Prefab { get; }
    public GameObject Dummy { get; }
    public GridCell.CellType CellType { get; }
}