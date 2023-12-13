using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyState : IState
{
    protected AEnemyController _enemy; //referencia al contexto para poder acceder a sus metodos (moverse, etc)

    public AEnemyState(AEnemyController enemy)
    {
        _enemy = enemy;
    }

    //metodos obligatiorios a implementar
    public abstract void Enter(IState previousState);
    public abstract void Update();

    //metodos opcionales a implementar (para evitar implementarlos y dejarlos vacios)
    public virtual void FixedUpdate() { }
    public virtual void Exit(IState nextState) { }
}
