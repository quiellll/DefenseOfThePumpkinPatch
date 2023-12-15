using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : ATurretProjectile
{
    // Velocidad del proyectil
    [SerializeField] private float _projectileSpeed = 10.0f;

    // Almacena todos los objetos con los que colisiona
    private Collider[] _overlaps = new Collider[5];
    // Radio del proyectil
    private float _colliderRadius;
    // Dirección del movimiento
    private Vector3 _direction;

    private void Awake()
    {
        // Se obtiene el collider del objeto hijo y se usa su radio para el OverlapSphere
        var col = GetComponentInChildren<SphereCollider>();
        _colliderRadius = col.radius * col.transform.localScale.x;
    }

    private void Update()
    {
        // Si el enemigo al que estaba siendo disparado muere, se mueve hacia delante (porque de normal la bala sigue a los enemigos).
        if(!EnemyTarget.IsAlive || !EnemyTarget.gameObject.activeSelf) _direction = transform.forward;

        // Si el enemigo sigue vivo, la dirección será el vector hacia la posición del propio enemigo.
        else _direction = (EnemyTarget.transform.position - transform.position).normalized;
        transform.Translate(_projectileSpeed * Time.deltaTime * _direction, Space.World);

        // Comprueba las colisiones
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        // Crea la OverlapSphere para saber con qué colisiona.
        var c = Physics.OverlapSphereNonAlloc(transform.position, _colliderRadius, _overlaps);

        for (int i = 0; i < c; i++) 
        {
            // Si choca con otra bala o con otra torre, ignora la colisión
            if (_overlaps[i].GetComponentInParent<ATurretProjectile>() != null) continue;
            if (_overlaps[i].GetComponent<TurretController>() != null) continue;

            // Si choca con un enemigo, lo daña
            if (_overlaps[i].TryGetComponent<AEnemyController>(out var e)) Damage(e);

            // Si choca con cualquier cosa que no sea torre o bala, desaparece (incluso con enemigos)
            DespawnProjectile();
            break;
        }
    }
}
