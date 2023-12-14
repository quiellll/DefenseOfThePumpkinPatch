using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDefenseMode : ADefenseMode //defensa de dia (granjeros)
{
    public DayDefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);

        _gameManager.FarmerWaveSpawner.WaveFinished.AddListener(OnWaveFinished);
        _gameManager.FarmerWaveSpawner.SpawnWave();
    }

    public override void Exit(IState nextState)
    {
        base.Exit(nextState);
        _gameManager.FarmerWaveSpawner.WaveFinished?.RemoveListener(OnWaveFinished);
    }
}
