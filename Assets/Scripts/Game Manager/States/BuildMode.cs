using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : AGameState
{
    AGameState _previousState;

    public BuildMode(GameManager g): base(g) { }

    public override void Enter(IState previousState)
    {
        _previousState = previousState as AGameState;
        _gameManager.HUD.StartWave.AddListener(OnStartWave);
        _gameManager.HUD.StartWaveButton.SetActive(true);
    }

    private void OnStartWave()
    {
        _gameManager.HUD.StartWave?.RemoveListener(OnStartWave);
        _gameManager.GameState = new DefenseMode(_gameManager);
    }
}
