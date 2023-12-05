using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMode : AGameState
{

    public DefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        _gameManager.HUD.StartWaveButton.SetActive(false);
        _gameManager.WaveSpawner.WaveFinished.AddListener(OnWaveFinished);
        _gameManager.WaveSpawner.SpawnWave();
    }

    public override void Exit(IState nextState)
    {
        _gameManager.WaveSpawner.WaveFinished?.RemoveListener(OnWaveFinished);
    }

    private void OnWaveFinished()
    {
        _gameManager.GameState = new BuildMode(_gameManager);
    }
}
