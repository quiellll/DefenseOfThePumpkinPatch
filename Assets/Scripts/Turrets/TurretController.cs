using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Turret _turret;
    // Posiciones
    // Radio (Rango)
    [SerializeField] private float _radius;
    // Capa enemigo
    [SerializeField] private LayerMask _enemyMask;
    // Velocidad rotacion
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private Transform _partToRotate;
    [SerializeField] private Transform _projectileSpawnPoint;
    private float _fireDelay;
    private float _fireDelayTimer = 0f;

    private EnemyController _currentTarget;
    private Collider[] _enemyCollidersInRange;

    private TurretProjectileSpawner _projectileSpawner;


    private void Awake()
    {
        // CAMBIAR A NUMERO OPTIMO DE ENEMIGOS !!!!!!!!
        _enemyCollidersInRange = new Collider[30];


        foreach(TurretProjectileSpawner spawner in FindObjectsOfType<TurretProjectileSpawner>())
        {
            // Si el spawner de balas es igual que la bala de mi torreta
            if(spawner.ProjectileType == _turret.ProjectilePrefab.GetType()) // DA ERROR
            { 
                _projectileSpawner = spawner;
                break;
            }
        }

        _fireDelay = 1.0f / _fireRate;
        _fireDelayTimer = _fireDelay;
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.1f, transform.position.z), _radius);


    private void Update()
    {
        _fireDelayTimer += Time.deltaTime; //actualizamosel contador de tiempo entre disparos

        if(!_currentTarget)
        {
            FindTarget();
            return;
        }

        //comprobar distancia al target
        float distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), _currentTarget.XY);
        if(distanceToTarget > _radius)
        {
            _currentTarget = null;
            return;
        }

        //Rotar hacia el target
        RotateTowardsTarget();

        //disparamos al target
        //si hay enemigo en el rango, comprobamos si se ha esperado el delay , y seteamos el timer a 0 para dispararle
        if (_fireDelayTimer >= _fireDelay)
        {
            Fire();
            _fireDelayTimer = 0.0f;
        }
    }



    private bool FindTarget()
    {
        Vector3 _groundPos = transform.position;
        _groundPos.y = 0;
        var targetCount = Physics.OverlapSphereNonAlloc(_groundPos, _radius, _enemyCollidersInRange, _enemyMask);

        if (targetCount == 0)
        {
            _currentTarget = null;
            return false;
        }

        else if (targetCount == 1) 
        {
            _currentTarget = _enemyCollidersInRange[0].GetComponentInParent<EnemyController>();
            return true;
        }

        _currentTarget = FindFirstEnemyInGroup(targetCount);

        return true;
    }

    private struct EnemyAtPath
    {
        public EnemyController Enemy;
        public int PathIndex;
        public EnemyAtPath(EnemyController enemy, int pathIndex)
        {
            Enemy = enemy;
            PathIndex = pathIndex;
        }
    }

    private EnemyController FindFirstEnemyInGroup(int numberOfEnemies)
    {
        EnemyAtPath[] enemiesAtPath = new EnemyAtPath[numberOfEnemies];
        int maxIndex = -1;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            var enemy = _enemyCollidersInRange[i].GetComponentInParent<EnemyController>();
            enemiesAtPath[i] = new EnemyAtPath(enemy, WorldGrid.Instance.GeIndexOfPathCell(enemy.CurrentCell));

            if (enemiesAtPath[i].PathIndex > maxIndex) maxIndex = enemiesAtPath[i].PathIndex;
            
        }

        List<EnemyAtPath> candidates = new();

        foreach(var e in enemiesAtPath) if (e.PathIndex == maxIndex) candidates.Add(e);


        if (candidates.Count == 1) return candidates[0].Enemy;
        

        int nextCellIndex = Mathf.Clamp(candidates[0].PathIndex + 1, 0, WorldGrid.Instance.Path.Count - 1);

        Vector2 nextCellPos = WorldGrid.Instance.Path[nextCellIndex].XY;

        float minDistanceToNextCell = float.MaxValue;
        EnemyController best = null;

        foreach(var c in candidates) 
        {
            float d = Vector2.Distance(c.Enemy.XY, nextCellPos);
            if (d < minDistanceToNextCell)
            {
                minDistanceToNextCell = d;
                best = c.Enemy;
            }
        }

        return best;
    }

    private void RotateTowardsTarget()
    {
        float newAngle = Quaternion.LookRotation(_currentTarget.transform.position - _partToRotate.position, Vector3.up).eulerAngles.y;
        _partToRotate.rotation = Quaternion.Lerp(_partToRotate.rotation, Quaternion.Euler(new Vector3(0, newAngle, 0)), _rotationSpeed * Time.deltaTime);
        Debug.DrawLine(_partToRotate.position, _currentTarget.transform.position);
    }

    private void Fire()
    {
        var projectile = _projectileSpawner.SpawnProjectile();
        projectile.Init(_projectileSpawner, _currentTarget, _projectileSpawnPoint.position);
    }
}
