using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    // Posiciones
    // Radio (Rango)
    [SerializeField] private float _radius;
    // Capa enemigo
    [SerializeField] private LayerMask _enemyMask;
    // Velocidad rotacion
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _partToRotate;

    private AEnemyController _currentTarget;
    private Collider[] _enemyCollidersInRange;


    private void Awake()
    {
        // CAMBIAR A NUMERO OPTIMO DE ENEMIGOS !!!!!!!!
        _enemyCollidersInRange = new Collider[30];
    }


    private void OnDrawGizmos() => Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.1f, transform.position.z), _radius);


    private void Update()
    {
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
        float newAngle = Quaternion.LookRotation(_currentTarget.transform.position - _partToRotate.position, Vector3.up).eulerAngles.y;
        _partToRotate.rotation = Quaternion.Lerp(_partToRotate.rotation, Quaternion.Euler(new Vector3(0, newAngle, 0)), _rotationSpeed * Time.deltaTime);
        Debug.DrawLine(_partToRotate.position, _currentTarget.transform.position);

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
            _currentTarget = _enemyCollidersInRange[0].GetComponentInParent<AEnemyController>();
            return true;
        }

        _currentTarget = FindFirstEnemyInGroup(targetCount);

        return true;
    }

    private struct EnemyAtPath
    {
        public AEnemyController Enemy;
        public int PathIndex;
        public EnemyAtPath(AEnemyController enemy, int pathIndex)
        {
            Enemy = enemy;
            PathIndex = pathIndex;
        }
    }

    private AEnemyController FindFirstEnemyInGroup(int numberOfEnemies)
    {
        EnemyAtPath[] enemiesAtPath = new EnemyAtPath[numberOfEnemies];
        int maxIndex = -1;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            var enemy = _enemyCollidersInRange[i].GetComponentInParent<AEnemyController>();
            enemiesAtPath[i] = new EnemyAtPath(enemy, WorldGrid.Instance.GetIndexOfPathCell(enemy.CurrentCell));

            if (enemiesAtPath[i].PathIndex > maxIndex) maxIndex = enemiesAtPath[i].PathIndex;
            
        }

        List<EnemyAtPath> candidates = new();

        foreach(var e in enemiesAtPath) if (e.PathIndex == maxIndex) candidates.Add(e);


        if (candidates.Count == 1) return candidates[0].Enemy;
        

        int nextCellIndex = Mathf.Clamp(candidates[0].PathIndex + 1, 0, WorldGrid.Instance.Path.Count - 1);

        Vector2 nextCellPos = WorldGrid.Instance.Path[nextCellIndex].XY;

        float minDistanceToNextCell = float.MaxValue;
        AEnemyController best = null;

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
}
