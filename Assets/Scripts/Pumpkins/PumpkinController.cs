using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinController : MonoBehaviour
{
    public Pumpkin Pumpkin { get => _pumpkin; }
    public GridCell Cell { get; private set; }

    [SerializeField] private Pumpkin _pumpkin;

    private void Start()
    {
        Cell = GameManager.Instance.CellManager.GetCellAt(new(transform.position.x, transform.position.z));
        GameManager.Instance.CellManager.AddPumpkin(Cell);
        GameManager.Instance.ServiceLocator.Get<IGameDataUpdater>().AddPumpkin(Cell.XY);
    }

    public void DestroyPumpkin(bool destroyOnStart = false)
    {
        GameManager.Instance.ServiceLocator.Get<IGameDataUpdater>().RemovePumpkin(Cell.XY);
        GameManager.Instance.CellManager.RemovePumpkin(Cell);
        Destroy(gameObject);
    }
}
