using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADefenseMode : AGameState //clase comun para los estados de defensa de dia y noche
{
    public ADefenseMode(GameManager g) : base(g) { }

    public override void Enter(IState previousState)
    {
        _gameManager.HUD.StartWaveButton.SetActive(false); //se desactiva el boton de empezar oleada (ya empezo)

        _gameManager.CommandManager.ClearCommands(); //se limpia el historial
        _gameManager.HUD.UpdateUndoButton();
        _gameManager.HUD.TimeScaleButton.SetActive(true);

        _gameManager.StartDefenseMode.Invoke();
    }

    protected virtual void OnWaveFinished()
    {
        _gameManager.GameState = new BuildMode(_gameManager); //al terminar la oleada se cambia a construccion
    }
}
