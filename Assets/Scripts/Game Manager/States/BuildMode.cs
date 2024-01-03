using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : AGameState //estado de construccion
{
    private bool _nextStateIsDay; //para saber a que estado de defensa cambiar, dia o noche

    public BuildMode(GameManager g): base(g) { }

    public override void Enter(IState previousState)
    {
        //si este es el estado inicial, o el estado anterior era noche, cambiar a dia el siguiente
        _nextStateIsDay = previousState == null || previousState as NightDefenseMode != null;
        _gameManager.HUD.WaveStarted.AddListener(OnStartWave);
        _gameManager.HUD.StartWaveButton.SetActive(true);
    }

    private void OnStartWave() //cuando se pulsa el boton de empezar oleada, se cambia al estado de defensa
    {
        _gameManager.HUD.WaveStarted?.RemoveListener(OnStartWave);
        _gameManager.GameState = 
            _nextStateIsDay ? new DayDefenseMode(_gameManager) : new NightDefenseMode(_gameManager);
    }
}
