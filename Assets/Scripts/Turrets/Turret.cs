using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//flyweight de la torreta con los stats comunes
[CreateAssetMenu(menuName = "ScriptableObjects/Turret")]
public class Turret : ScriptableObject
{
    public GameObject Prefab { get => _turretPrefab; }
    public GameObject Dummy { get => _dummyPrefab; }
    public ATurretProjectile ProjectilePrefab { get => _projectilePrefab; }

    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _dummyPrefab;

    [SerializeField] private ATurretProjectile _projectilePrefab;
}
