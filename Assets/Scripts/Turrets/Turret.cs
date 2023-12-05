using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Turret")]

public class Turret : ScriptableObject
{
    public GameObject Prefab { get => _turretPrefab; }
    public GameObject Dummy { get => _dummyPrefab; }

    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _dummyPrefab;
}
