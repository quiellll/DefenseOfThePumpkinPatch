using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(TurretRange))]
public abstract class ATurretController : MonoBehaviour
{
    //su posicion en el plano xz
    public Vector2 XY { get => new(transform.position.x, transform.position.z); }

    // Referencia al ScriptableObject de la torreta
    [SerializeField] protected Turret _turret;
    // Parte del modelo que gira
    [SerializeField] private Transform _partToRotate;
    // Punto de partida del proyectil
    [SerializeField] protected Transform _projectileSpawnPoint;

    // Herramientas para controlar el disparo
    private float _fireDelay;
    private float _fireDelayTimer = 0f;

    // Gestión de enemigos
    protected AEnemyController _currentTarget;
    private Collider[] _enemyCollidersInsideOuterRadius;

    // Capa en la que se encuentran los enemigos
    private LayerMask _enemyMask;


    protected virtual void Awake()
    {
        // CAMBIAR A NUMERO OPTIMO DE ENEMIGOS !!!!!!!!
        _enemyCollidersInsideOuterRadius = new Collider[10];

        // Con esto se saca correctamente la velocidad de disparo y el retraso entre disparos
        _fireDelay = 1.0f / _turret.FireRate;
        _fireDelayTimer = _fireDelay;

        //inicializar la layermask de enemigos
        _enemyMask = LayerMask.GetMask("Enemy");

    }

    // DEBUG. Para dibujar la esfera que muestra el rango de ataque
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.1f, transform.position.z), _turret.OuterRadius);

        if (_turret.InnerRadius == 0f) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.1f, transform.position.z), _turret.InnerRadius);
    }


    protected virtual void Update()
    {
        //actualizamos el contador de tiempo entre disparos
        _fireDelayTimer += Time.deltaTime;

        // Si no tiene un objetivo fijado, comienza a buscar uno
        if (!_currentTarget)
        {
            FindTarget();
            return;
        }

        // comprobar distancia al objetivo
        float distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), _currentTarget.XY);
        // Si se sale del rango, deja de apuntarle
        if(distanceToTarget > _turret.OuterRadius || distanceToTarget < _turret.InnerRadius)
        {
            _currentTarget = null;
            return;
        }

        //Rotar hacia el target
        RotateTowardsTarget();

        //disparamos al target
        //si hay enemigo en el rango, comprobamos si se ha esperado el delay , y seteamos el timer a 0 para dispararle de nuevo
        if (_fireDelayTimer >= _fireDelay)
        {
            Fire();
            _fireDelayTimer = 0.0f;
        }
    }


    // Este método se encarga de encontrar un enemigo y marcarlo como objetivo
    private bool FindTarget()
    {

        // Con esto se crea la esfera que rodea la torre y sirve como su rango de ataque
        Vector3 _groundPos = transform.position;
        _groundPos.y = 0;
        var targetCount = Physics.OverlapSphereNonAlloc(_groundPos, _turret.OuterRadius, _enemyCollidersInsideOuterRadius, _enemyMask);

        //si no hay un inner radius
        if (_turret.InnerRadius <= 0f)
        {
            // Si no hay enemigos, dejamos el objetivo actual como null
            if (targetCount == 0)
            {
                _currentTarget = null;
                return false;
            }
            // Si solo hay un enemigo en todo el rango, se marca como objetivo
            else if (targetCount == 1) 
            {
                _currentTarget = _enemyCollidersInsideOuterRadius[0].GetComponentInParent<AEnemyController>();
                return true;
            }
            // En caso de que haya más de un enemigo en todo el rango, hay que compararlos y ver cuál es la mejor opción
            _currentTarget = FindFirstEnemyInGroup(_enemyCollidersInsideOuterRadius, targetCount);

            return true;
        }
        //si hay un inner radius
        List<Collider> enemyColsInRange = new();
        for (int i = 0; i < targetCount; i++) //metemos en la lista los que esten fuera del inner radius
        {
            var enemyXY = _enemyCollidersInsideOuterRadius[i].GetComponentInParent<AEnemyController>().XY;
            if (Vector2.Distance(XY, enemyXY) >= _turret.InnerRadius)
                enemyColsInRange.Add(_enemyCollidersInsideOuterRadius[i]);
        }

        // Si no hay enemigos, dejamos el objetivo actual como null
        if (enemyColsInRange.Count == 0)
        {
            _currentTarget = null;
            return false;
        }
        // Si solo hay un enemigo en todo el rango, se marca como objetivo
        else if (enemyColsInRange.Count == 1)
        {
            _currentTarget = enemyColsInRange[0].GetComponentInParent<AEnemyController>();
            return true;
        }
        // En caso de que haya más de un enemigo en todo el rango, hay que compararlos y ver cuál es la mejor opción
        _currentTarget = FindFirstEnemyInGroup(enemyColsInRange.ToArray(), enemyColsInRange.Count);
        return true;

    }

    // Estructura para conocer datos de un enemigo en una casilla concreta
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

    // Métodos de búsqueda de enemigos.
    // En principio siempre se apunta al primer enemigo de la oleada, si se quisiera mejorar el juego, se podrían incluir distintos métodos
    // como por ejemplo disparar al último enemigo; al de mayor vida; al de menor vida...
    // Busca al primer enemigo en un segmento del camino.
    private AEnemyController FindFirstEnemyInGroup(Collider[] enemyColliders, int numberOfEnemies)
    {
        // Seleccionamos todos los enemigos en el segmento
        EnemyAtPath[] enemiesAtPath = new EnemyAtPath[numberOfEnemies];
        int maxIndex = -1;


        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Vemos en que casilla está situado
            var enemy = enemyColliders[i].GetComponentInParent<AEnemyController>();
            enemiesAtPath[i] = new EnemyAtPath(enemy, WorldGrid.Instance.GetIndexOfPathCell(enemy.CurrentCell));

            // Nos quedamos con la casilla más alta (cercana a la zona a defender) que tenga enemigos.
            if (enemiesAtPath[i].PathIndex > maxIndex) maxIndex = enemiesAtPath[i].PathIndex;
            
        }

        // Se crea una lista de "candidatos" a ser disparado
        List<EnemyAtPath> candidates = new();

        // Todos los enemigos que se encuentren en esa casilla seleccionada se añaden a la lista
        foreach(var e in enemiesAtPath) if (e.PathIndex == maxIndex) candidates.Add(e);

        // Si sólo hay un enemigo, se devuelve directamente
        if (candidates.Count == 1) return candidates[0].Enemy;
        
        // Si hay más de un enemigo, se busca al que esté más adelantado dentro de la propia casilla de la siguiente forma
        // Se obtiene la casilla siguiente a la seleccionada.
        int nextCellIndex = Mathf.Clamp(candidates[0].PathIndex + 1, 0, WorldGrid.Instance.Path.Count - 1);

        // Se obtiene la posición de esta casilla nueva
        Vector2 nextCellPos = WorldGrid.Instance.Path[nextCellIndex].XY;

        float minDistanceToNextCell = float.MaxValue;
        AEnemyController best = null;

        // Se iteran los enemigos y se selecciona el más cercano a la casilla nueva
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

    // Este método busca rotar el arma hacia el enemigo al que esté apuntando
    private void RotateTowardsTarget()
    {
        // Se saca el ángulo al que hay que apuntar
        float newAngle = Quaternion.LookRotation(_currentTarget.transform.position - _partToRotate.position, Vector3.up).eulerAngles.y;
        // Se rota el cañón/catapulta/ballesta hacia el enemigo
        _partToRotate.rotation = Quaternion.Lerp(_partToRotate.rotation, Quaternion.Euler(new Vector3(0, newAngle, 0)), _turret.RotationSpeed * Time.deltaTime);
        // DEBUG. Dibuja la línea que muestra hacia dónde se apunta
        Debug.DrawLine(_partToRotate.position, _currentTarget.transform.position);
    }

    // Método para disparar
    protected abstract void Fire();

}
