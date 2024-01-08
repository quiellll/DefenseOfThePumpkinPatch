using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : AEnemyController //controlador del fantasma que hereda del comun abstracto
{
    public bool Transformed {  get; set; }

    [SerializeField] private GameObject _pumpkin;

    private ZombieSpawner _zombieSpawner;

    private Vector3 _transformPositon;
    private Quaternion _transformRotation;

    protected override void Start()
    {
        base.Start();

        SetInitialState(new GhostMoveForward(this));
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void InitEnemy(Vector3 pos, Quaternion rot, IEnemySpawner spawner)
    {
        base.InitEnemy(pos, rot, spawner);
        Transformed = false;
        _pumpkin.SetActive(false);
        _zombieSpawner = (spawner as AWaveSpawner).gameObject.GetComponent<ZombieSpawner>();
        SetInitialState(new GhostMoveForward(this));
    }

    public override void Reset()
    {
        base.Reset();
        Transformed = false;
    }

    public void TransformToZombie(CellManager.GraveAtPath grave)
    {
        Transformed = true;

        _transformPositon = transform.position;
        _transformRotation = transform.rotation;

        Despawn(); //despawneamos al fantasma
        //llamo a startcoroutine desde el spawner porque
        //unity solo deja hacer coroutines desde objetos activos
        _zombieSpawner.StartCoroutine(SpawnZombie(grave));


    }

    private IEnumerator SpawnZombie(CellManager.GraveAtPath grave)
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.CellManager.DestroyGrave(grave); //destuimos la tumba
        _zombieSpawner.SpawnZombie(_transformPositon, _transformRotation);
    }

    public override void InteractWithPumpkin(GridCell pumpkinCell)
    {
        base.InteractWithPumpkin(pumpkinCell);
        State = null;
        SetAnimation(_pickUpAnim, false);
        StartCoroutine(WaitAndAscend(pumpkinCell));
    }

    private IEnumerator WaitAndAscend(GridCell pumpkinCell)
    {
        yield return new WaitForSeconds(.4f);
        pumpkinCell.DestroyPumpkin();
        _pumpkin.SetActive(true);
        State = new GhostAscend(this);
    }
}

