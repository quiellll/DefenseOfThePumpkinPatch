using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Turret")]

public class Turret : ScriptableObject
{
    // Prefab de la torreta
    public GameObject Prefab { get => _turretPrefab; }
    // Prefab de placeholder
    public GameObject Dummy { get => _dummyPrefab; }
    // Prefab del proyectil a disparar
    public ATurretProjectile ProjectilePrefab { get => _projectilePrefab; }

    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _dummyPrefab;
    [SerializeField] private ATurretProjectile _projectilePrefab;
}
