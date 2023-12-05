using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameState : IState
{
    protected GameManager _gameManager;
    public AGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public abstract void Enter(IState previousState);

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Exit(IState nextState) { }

}
