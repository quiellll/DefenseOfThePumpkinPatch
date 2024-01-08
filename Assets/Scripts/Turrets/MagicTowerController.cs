using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerController : ATurretController
{
    private MagicTower _magicTower;
    private LineRenderer _beam;
    private bool _hasAttackColor = false;

    protected override void Awake()
    {
        base.Awake();
        _magicTower = _turret as MagicTower;
        _beam  = GetComponentInChildren<LineRenderer>();
        _beam.material = _magicTower.BeamMaterial;
    }

    protected override void Update()
    {
        base.Update();

        if (!_currentTarget)
        {
            if(_hasAttackColor) ResetBeamColor();
            if (_beam.enabled) _beam.enabled = false;
            return;
        }

        if(!_beam.enabled) _beam.enabled = true;

        _beam.SetPosition(0, _projectileSpawnPoint.position);
        _beam.SetPosition(1, _currentTarget.transform.position);
    }

    protected override void Fire()
    {
        _currentTarget.TakeDamage(_turret.Damage);

        _hasAttackColor = true;
        _beam.material = _magicTower.AttackMaterial;
        Invoke(nameof(ResetBeamColor), _magicTower.AttackDuration);
    }

    private void ResetBeamColor()
    {
        _beam.material = _magicTower.BeamMaterial;
        _hasAttackColor = false;
    }

}
