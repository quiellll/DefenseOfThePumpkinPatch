using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightDefenseMode : ADefenseMode //defensa de noche (fantasmas)
{
    public NightDefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        base.Enter(previousState);
        _gameManager.ZombieSpawner.WaveFinished.AddListener(OnWaveFinished);
        _gameManager.GhostSpawner.SpawnWave();
    }

    public override void Exit(IState nextState)
    {
        _gameManager.ZombieSpawner.WaveFinished?.RemoveListener(OnWaveFinished);
        Debug.Log("noche fuera");
        GameManager.Instance.AdvanceDay();
    }
}
