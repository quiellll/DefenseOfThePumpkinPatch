using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//controlador del enemigo, funcionalidades comunes de todos los enemigos que se agrupan en la clase abstracta
public abstract class AEnemyController : MonoBehaviour, IPoolObject
{
    //getter/setter para los estados
    public IState State { get => _currentState; set => ChangeState(value as AEnemyState); }
    //para activar-desactivar objeto usando la interfaz IPoolObject
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public bool IsAlive { get; private set; } //si esta vivo
    public GridCell CurrentCell { get; private set; } //la celda actual del camino donde esta
    //su posicion en el plano xz
    public Vector2 XY { get => new (transform.position.x, transform.position.z); }
    public Vector3 Direction { get; private set; }

    protected float _rotationSpeed;

    [SerializeField] protected Enemy _stats; //scriptableobject flyweight con los parametros comunes
    //modelo hijo del obj de este script (para que la rotacion + traslacion no se rompa)
    [SerializeField] protected Transform _body; 

    private AEnemyState _currentState;
    protected int _currentHealth;
    private Vector2 _gridPos; //posicion redondead para saber en que celda esta
    private IEnemySpawner _spawner;


    protected Animator _animator;
    private ParticleSystem _damageParticles;

    protected int _deadAnim, _damagedAnim, _pickUpAnim;


    protected virtual void Awake()
    {
        _currentHealth = _stats.Health;
        IsAlive = true;
        _animator = GetComponentInChildren<Animator>();
        _damageParticles = GetComponentInChildren<ParticleSystem>();

        _deadAnim = Animator.StringToHash("isDead");
        _damagedAnim = Animator.StringToHash("isDamaged");
        _pickUpAnim = Animator.StringToHash("isPickingUp");

        _rotationSpeed = _stats.RotationSpeed;
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

    public virtual void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        _spawner = spawner;
        transform.SetPositionAndRotation(pos, rot);
        IsAlive = true;
        UpdateCurrentCell();
    }

    //funcion de movimiento llamada por los estados de movimiento
    public void Move(Vector3 direction, bool rotate = true)
    {
        Direction = direction;

        //rotacion (del modelo hijo para no afectar el movimiento del padre)
        if(rotate && Vector3.SignedAngle(_body.forward , direction, Vector3.up) != 0)
        {
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _body.localRotation = 
                Quaternion.RotateTowards(_body.localRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }


        //movimiento
        transform.Translate(direction * _stats.MoveSpeed * Time.deltaTime);

        UpdateCurrentCell();
    }

    public void ChangeRotationSpeed(float factor) => _rotationSpeed *= factor;
    public void ResetRotationSpeed() => _rotationSpeed = _stats.RotationSpeed;

    public void Despawn() => _spawner.DespawnEnemy(this);

    //actualiza la celda actual si ha cambiado
    protected void UpdateCurrentCell()
    {
        if (!GameManager.Instance.CellManager) return;

        Vector2 roundedPos = new (Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
        if (_gridPos == roundedPos) return;

        var lastCell = CurrentCell;

        _gridPos = roundedPos;
        CurrentCell = GameManager.Instance.CellManager.GetCellAt(_gridPos);

        if(lastCell == null) return;

        if (lastCell.Type == GridCell.CellType.Path && CurrentCell.Type != GridCell.CellType.Path)
            transform.Translate(0f, 0.1f, 0f);

        else if (lastCell.Type != GridCell.CellType.Path && CurrentCell.Type == GridCell.CellType.Path)
            transform.Translate(0f, -0.1f, 0f);
    }

    //cambia de estado (funcion llamada solo por el setter de State)
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

    //funcion publica para recibir daño
    public virtual void TakeDamage(int damage)
    {
        if (_currentHealth <= 0) return;
        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        _damageParticles.Play();

        if (_currentHealth == 0)
        {
            StartCoroutine(StartDeath());
        }
        //else SetAnimation(_damagedAnim);
    }

    protected IEnumerator StartDeath()
    {
        GameManager.Instance.ServiceLocator.Get<IAudioManager>().PlaySoundEffect(_stats.DieSound);

        GameManager.Instance.Gold += State is MoveBackwards ? _stats.LootWithPumpkin : _stats.Loot;
        State = null;
        SetAnimation(_deadAnim);
        IsAlive = false;
        yield return new WaitForSeconds(0.6f);

        Die();
    }

    protected virtual void Die() //morir
    {
        Despawn();
    }

    //instanciar un objeto a paertir de este (para el Object Pool)

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        IPoolObject clone = parent ? Instantiate(this, parent) : Instantiate(this);
        
        clone.Active = active;
        return clone;
    }

    public virtual void Reset() //funcion de IPoolObject para "limpiar" el objeto cuando vuelve a la pool
    {
        _currentHealth = _stats.Health;
    }

    public virtual void InteractWithPumpkin(GridCell pumpkinCell)
    {
        GameManager.Instance.ServiceLocator.Get<IAudioManager>().PlaySoundEffect(_stats.InteractSound);
    }


    protected void SetAnimation(int anim, bool reset = true)
    {
        _animator.SetBool(anim, true);
        if(reset) StartCoroutine(ResetAnimationParam(anim));
    }

    private IEnumerator ResetAnimationParam(int anim)
    {
        yield return new WaitForSeconds(.01f);
        _animator.SetBool(anim, false);
    }

}
