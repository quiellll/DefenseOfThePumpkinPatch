using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyController : MonoBehaviour, IEnemyStateContext
{
    public GameObject GameObject { get => gameObject; }
    //getter/setter para los estados
    public IState State { get => _currentState; set => ChangeState(value as AEnemyState); }

    //esto es temporal luego se hace un scriptableobject flyweight
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private Transform _body; //modelo hijo del obj de este script (para que la rotacion + traslacion no se rompa)

    private AEnemyState _currentState;


    private void Awake()
    {
        _body = GetComponentInChildren<MeshRenderer>().transform;
    }

    private void Start()
    {
        State = new MoveForward(this);
    }

    private void Update()
    {
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void Move(Vector3 direction)
    {
        //rotacion (del modelo hijo para no afectar el movimiento del padre)
        if(Vector3.SignedAngle(_body.forward , direction, Vector3.up) != 0)
        {
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _body.localRotation = 
                Quaternion.RotateTowards(_body.localRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        //movimiento
        transform.Translate(direction * _moveSpeed * Time.deltaTime);
    }

    private void ChangeState(AEnemyState newState)
    {
        _currentState?.Exit(newState);

        var lastState = _currentState;

        _currentState = newState;

        _currentState?.Enter(lastState);
    }
}
