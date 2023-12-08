using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : AGameState
{
    private bool _nextStateIsDay;

    public BuildMode(GameManager g): base(g) { }

    public override void Enter(IState previousState)
    {
        _nextStateIsDay = previousState == null || previousState as NightDefenseMode != null;
        _gameManager.HUD.StartWave.AddListener(OnStartWave);
        _gameManager.HUD.StartWaveButton.SetActive(true);
    }

    private void OnStartWave()
    {
        _gameManager.HUD.StartWave?.RemoveListener(OnStartWave);
        _gameManager.GameState = 
            _nextStateIsDay ? new DayDefenseMode(_gameManager) : new NightDefenseMode(_gameManager);
    }
}
