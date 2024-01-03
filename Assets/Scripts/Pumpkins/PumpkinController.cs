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
        Cell = WorldGrid.Instance.GetCellAt(new(transform.position.x, transform.position.z));
        WorldGrid.Instance.AddPumpkin(Cell);
    }

    public void DestroyPumpkin()
    {
        WorldGrid.Instance.RemovePumpkin(Cell);
        Destroy(gameObject);
    }
}
