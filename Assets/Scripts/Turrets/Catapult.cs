
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Catapult")]
public class Catapult : Turret
{
    public float MaxProjectileHeight { get => _maxProjectileHeight; }

    [SerializeField] private float _maxProjectileHeight;
}
