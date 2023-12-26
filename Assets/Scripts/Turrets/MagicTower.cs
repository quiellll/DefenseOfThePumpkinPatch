using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MagicTower")]

public class MagicTower : Turret
{
    public Color BeamColor { get => _beamColor; }
    public Color AttackColor { get => _attackColor; }
    public float AttackColorDuration { get => _attackColorDuration; }

    [SerializeField] private Color _beamColor;
    [SerializeField] private Color _attackColor;
    [SerializeField] private float _attackColorDuration;
}
