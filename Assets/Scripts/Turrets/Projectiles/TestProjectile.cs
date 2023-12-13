using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : ATurretProjectile
{
    [SerializeField] private float _projectileSpeed = 10.0f;

    private Collider[] _overlaps = new Collider[5];
    private float _colliderRadius;
    private Vector3 _direction;

    private void Awake()
    {
        var col = GetComponentInChildren<SphereCollider>();
        _colliderRadius = col.radius * col.transform.localScale.x;
    }

    private void Update()
    {
        _direction = (EnemyTarget.transform.position - transform.position).normalized;
        transform.Translate(_projectileSpeed * Time.deltaTime * _direction, Space.World);

        CheckCollisions();
    }

    private void CheckCollisions()
    {
        var c = Physics.OverlapSphereNonAlloc(transform.position, _colliderRadius, _overlaps);

        for (int i = 0; i < c; i++) 
        {
            if (_overlaps[i].GetComponentInParent<ATurretProjectile>() != null) continue;
            if (_overlaps[i].GetComponent<TurretController>() != null) continue;


            if (_overlaps[i].TryGetComponent<EnemyController>(out var e)) Damage(e);

            Debug.Log("despawneado");
            DespawnProjectile();
            break;
        }
    }
}
