using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADefenseMode : AGameState
{
    public ADefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        _gameManager.HUD.StartWaveButton.SetActive(false);
    }

    protected virtual void OnWaveFinished()
    {
        _gameManager.GameState = new BuildMode(_gameManager);
    }
}
