using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSprout : MonoBehaviour
{
    public Vector2 XY { get => new(transform.position.x, transform.position.z); }
    public Pumpkin Pumpkin { get => _pumpkin; }
    public GridCell Cell { get; private set; }
    public int Journeys { get; set; }

    [SerializeField] private Pumpkin _pumpkin;


    private void Start()
    {
        Cell = GameManager.Instance.CellManager.GetCellAt(XY);

        GameManager.Instance.StartBuildMode.AddListener(OnStartBuild);

        GameManager.Instance.ServiceLocator.Get<IGameDataUpdater>().AddSprout(XY, Journeys);

    }

    private void OnStartBuild()
    {
        Journeys++;
        if (Journeys >= _pumpkin.SproutGrowthPeriod) GrowPumpkin();

        else GameManager.Instance.ServiceLocator.Get<IGameDataUpdater>().UpdateSprout(XY, Journeys);

    }

    private void GrowPumpkin()
    {
        GameManager.Instance.HUD.WaveStarted.RemoveListener(OnStartBuild);
        GameManager.Instance.ServiceLocator.Get<IGameDataUpdater>().RemoveSprout(XY);

        Cell.BuildPumpkin(_pumpkin);
    }
}
