using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int Health { get => _health; }
    public float MoveSpeed { get => _moveSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }
    public GameObject GravePrefab { get => _gravePrefab; }

    [SerializeField] private int _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _gravePrefab;
}
