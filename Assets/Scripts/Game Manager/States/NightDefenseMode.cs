using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightDefenseMode : ADefenseMode
{
    public NightDefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);

        _gameManager.GhostWaveSpawner.WaveFinished.AddListener(OnWaveFinished);
        _gameManager.GhostWaveSpawner.SpawnWave();
    }

    public override void Exit(IState nextState)
    {
        _gameManager.GhostWaveSpawner.WaveFinished?.RemoveListener(OnWaveFinished);
    }
}
