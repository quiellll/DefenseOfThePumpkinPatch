using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    public int Health { get => _health; }
    public float MoveSpeed { get => _moveSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }
    public AEnemyController Prefab { get => _enemyPrefab; }
    public GameObject GravePrefab { get => _gravePrefab; }

    [SerializeField] private AEnemyController _enemyPrefab;
    [SerializeField] private int _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _gravePrefab;
}
