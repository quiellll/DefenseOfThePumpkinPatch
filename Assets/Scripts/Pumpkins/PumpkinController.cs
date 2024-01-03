using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinController : MonoBehaviour
{
    GridCell _cell;

    private void Start()
    {
        _cell = WorldGrid.Instance.GetCellAt(new(transform.position.x, transform.position.z));
        WorldGrid.Instance.AddPumpkin(_cell);
    }

    public void DestroyPumpkin()
    {
        WorldGrid.Instance.RemovePumpkin(_cell);
        Destroy(gameObject);
    }
}