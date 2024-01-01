using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSprout : MonoBehaviour
{
    public Vector2 XY { get => new(transform.position.x, transform.position.z); }

    [SerializeField] private Pumpkin _pumpkin;

    private GridCell _cell;
    private int _journeys = 0;

    private void Start()
    {
        _cell = WorldGrid.Instance.GetCellAt(XY);

        GameManager.Instance.HUD.WaveStarted.AddListener(OnStartWave);
    }

    private void OnStartWave()
    {
        _journeys++;
        if (_journeys >= _pumpkin.SproutGrowthPeriod) GrowPumpkin();
    }

    private void GrowPumpkin()
    {
        GameManager.Instance.HUD.WaveStarted.RemoveListener(OnStartWave);

        _cell.BuildPumpkin(_pumpkin);
    }
}
