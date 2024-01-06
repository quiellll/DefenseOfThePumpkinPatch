using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDefenseMode : ADefenseMode //defensa de dia (granjeros)
{
    public DayDefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);

        _gameManager.FarmerSpawner.WaveFinished.AddListener(OnWaveFinished);
        _gameManager.FarmerSpawner.SpawnWave();
    }

    public override void Exit(IState nextState)
    {
        base.Exit(nextState);
        _gameManager.FarmerSpawner.WaveFinished?.RemoveListener(OnWaveFinished);
    }
}
