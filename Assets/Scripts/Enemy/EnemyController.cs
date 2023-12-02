using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyController : MonoBehaviour, IEnemyStateContext, IPoolObject
{
    public GameObject GameObject { get => gameObject; }
    //getter/setter para los estados
    public IState State { get => _currentState; set => ChangeState(value as AEnemyState); }
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public bool IsAlive { get; private set; }
    public WaveSpawner Spawner { get; set; }
    public GridCell CurrentCell { get; private set; }
    public Vector2 XY { get => new (transform.position.x, transform.position.z); }

    [SerializeField] private EnemyStats _stats;

    //modelo hijo del obj de este script (para que la rotacion + traslacion no se rompa)
    private Transform _body; 

    private AEnemyState _currentState;

    private int _currentHealth;


    private void Awake()
    {
        _body = GetComponentInChildren<MeshRenderer>().transform;
        _currentHealth = _stats.Health;
        IsAlive = true;
    }

    private void Start()
    {
        SetInitialState(new MoveForward(this));
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
                Quaternion.RotateTowards(_body.localRotation, targetRotation, _stats.RotationSpeed * Time.deltaTime);
        }
        //movimiento
        transform.Translate(direction * _stats.MoveSpeed * Time.deltaTime);

        UpdateCurrentCell();
    }

    private void UpdateCurrentCell()
    {
        Vector2 roundedPos = new (Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
        CurrentCell = WorldGrid.Instance.GetCellAt(roundedPos);
    }

    private void ChangeState(AEnemyState newState, bool isInitial = false)
    {
        if(isInitial)
        {
            _currentState = newState;
            _currentState.Enter(null);
            return;
        }

        _currentState?.Exit(newState);

        var lastState = _currentState;

        _currentState = newState;

        _currentState?.Enter(lastState);
    }

    private void SetInitialState(AEnemyState state) => ChangeState(state, true);

    void OnMouseDown() => TakeDamage(1);

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);

        if(_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsAlive = false;
        Spawner.DespawnEnemy(this);
    }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this) : Instantiate(this, parent);
        
        clone.Active = active;
        return clone;
    }

    public void Reset()
    {
        _currentHealth = _stats.Health;
        IsAlive = true;
        SetInitialState(new MoveForward(this));
    }
}
