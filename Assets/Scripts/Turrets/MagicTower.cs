using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MagicTower")]

public class MagicTower : Turret
{
    public Material BeamMaterial { get => _beamMaterial; }
    public Material AttackMaterial { get => _attackMaterial; }
    public float AttackDuration { get => _attackDuration; }

    [SerializeField] private Material _beamMaterial;
    [SerializeField] private Material _attackMaterial;
    [SerializeField] private float _attackDuration;
}
