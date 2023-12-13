using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class AEnemyController : MonoBehaviour, IStateContext, IPoolObject
{
    //getter/setter para los estados
    public IState State { get => _currentState; set => ChangeState(value as AEnemyState); }
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public bool IsAlive { get; private set; }
    public GridCell CurrentCell { get; private set; }
    public Vector2 XY { get => new (transform.position.x, transform.position.z); }

    [SerializeField] protected Enemy _stats;
    //modelo hijo del obj de este script (para que la rotacion + traslacion no se rompa)
    [SerializeField] protected Transform _body; 

    private AEnemyState _currentState;
    private int _currentHealth;
    private Vector2 _gridPos;
    private WaveSpawner _spawner;


    protected virtual void Awake()
    {
        _currentHealth = _stats.Health;
        IsAlive = true;
    }

    protected virtual void Start()
    {
        UpdateCurrentCell();
    }

    protected virtual void Update()
    {
        _currentState?.Update();
    }

    protected virtual void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public virtual void InitEnemy(Vector3 pos, Quaternion rot, WaveSpawner spawner)
    {
        _spawner = spawner;
        transform.SetPositionAndRotation(pos, rot);
        IsAlive = true;
        UpdateCurrentCell();
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

    public void Despawn() => _spawner.DespawnEnemy(this);

    protected void UpdateCurrentCell()
    {
        if (!WorldGrid.Instance) return;

        Vector2 roundedPos = new (Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
        if (_gridPos == roundedPos) return;
        _gridPos = roundedPos;
        CurrentCell = WorldGrid.Instance.GetCellAt(_gridPos);
    }

    protected virtual void ChangeState(AEnemyState newState, bool isInitial = false)
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

    protected void SetInitialState(AEnemyState state) => ChangeState(state, true);

    void OnMouseDown() => TakeDamage(_stats.Health); //debug

    public virtual void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);

        if(_currentHealth == 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        IsAlive = false;
        Despawn();
    }



    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this, parent) : Instantiate(this);
        
        clone.Active = active;
        return clone;
    }

    public virtual void Reset()
    {
        _currentHealth = _stats.Health;
        transform.position = Vector3.one * 300f;
    }

}
