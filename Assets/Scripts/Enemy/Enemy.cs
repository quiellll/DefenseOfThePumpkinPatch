using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Flyweight del enemigo, stats comunes
[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    public int Health { get => _health; }
    public float MoveSpeed { get => _moveSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }
    public int Loot { get => _loot; }
    public int LootWithPumpkin { get => _loot + 10; }
    public AEnemyController Prefab { get => _enemyPrefab; }
    public GameObject GravePrefab { get => _gravePrefab; }
    public AudioClip InteractSound { get => _interactSound; }
    public AudioClip DieSound { get => _dieSound; }
    public ParticleSystem SpawnZombieParticles { get => _spawnZombieParticles; }

    [SerializeField] private int _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _loot;
    [SerializeField] private AEnemyController _enemyPrefab;
    [SerializeField] private GameObject _gravePrefab;
    [SerializeField] private AudioClip _interactSound;
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private ParticleSystem _spawnZombieParticles;
}
