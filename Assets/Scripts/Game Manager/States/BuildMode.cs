using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : AGameState //estado de construccion
{
    private bool _isDay; //para saber a que estado de defensa cambiar, dia o noche

    public BuildMode(GameManager g): base(g) { }

    public override void Enter(IState previousState)
    {
        //si este es el estado inicial, o el estado anterior era noche, cambiar a dia el siguiente
        _isDay = previousState == null || previousState is NightDefenseMode;

        if(_isDay) _gameManager.CellManager.ClearGraves();

        _gameManager.CommandManager.ClearCommands(); //se limpia el historial
        _gameManager.HUD.WaveStarted.AddListener(OnStartWave);
        _gameManager.HUD.StartWaveButton.SetActive(true);
        _gameManager.HUD.UpdateUndoButton();

        if(_gameManager.TimeScale != 1) _gameManager.HUD.ToggleTimeScale();
        _gameManager.HUD.TimeScaleButton.SetActive(false);

        _gameManager.StartBuildMode.Invoke();



    }

    private void OnStartWave() //cuando se pulsa el boton de empezar oleada, se cambia al estado de defensa
    {
        _gameManager.HUD.WaveStarted?.RemoveListener(OnStartWave);
        _gameManager.GameState = 
            _isDay ? new DayDefenseMode(_gameManager) : new NightDefenseMode(_gameManager);
    }
}
