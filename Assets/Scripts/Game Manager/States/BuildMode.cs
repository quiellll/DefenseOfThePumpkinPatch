using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : AGameState //estado de construccion
{
    private bool _isDay; //para saber a que estado de defensa cambiar, dia o noche
    private bool _nextIsDayInitial;
    public BuildMode(GameManager g, bool nextIsDayInitial = true): base(g) => _nextIsDayInitial = nextIsDayInitial;

    public override void Enter(IState previousState)
    {
        //si este es el estado inicial, o el estado anterior era noche, cambiar a dia el siguiente
        if (previousState != null) _isDay = previousState is NightDefenseMode;
        else _isDay = _nextIsDayInitial;

        if(_isDay) _gameManager.CellManager.ClearGraves();

        _gameManager.ServiceLocator.Get<IGameDataUpdater>().UpdateNextDefenseIsDay(_isDay);

        _gameManager.CommandManager.ClearCommands(); //se limpia el historial


        _gameManager.HUD.WaveStarted.AddListener(OnStartWave); //boton iniciar oleada

        _gameManager.StartBuildMode.Invoke();



    }

    private void OnStartWave() //cuando se pulsa el boton de empezar oleada, se cambia al estado de defensa
    {
        _gameManager.HUD.WaveStarted?.RemoveListener(OnStartWave);
        _gameManager.GameState = 
            _isDay ? new DayDefenseMode(_gameManager) : new NightDefenseMode(_gameManager);
    }
}
