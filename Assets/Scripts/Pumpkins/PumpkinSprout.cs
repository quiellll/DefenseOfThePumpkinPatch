using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSprout : MonoBehaviour
{
    public Vector2 XY { get => new(transform.position.x, transform.position.z); }
    public Pumpkin Pumpkin { get => _pumpkin; }
    public GridCell Cell { get; private set; }

    [SerializeField] private Pumpkin _pumpkin;

    private int _journeys = 0;

    private void Start()
    {
        Cell = WorldGrid.Instance.GetCellAt(XY);

        GameManager.Instance.StartBuild.AddListener(OnStartBuild);
    }

    private void OnStartBuild()
    {
        _journeys++;
        if (_journeys >= _pumpkin.SproutGrowthPeriod) GrowPumpkin();
    }

    private void GrowPumpkin()
    {
        GameManager.Instance.HUD.WaveStarted.RemoveListener(OnStartBuild);

        Cell.BuildPumpkin(_pumpkin);
    }
}
