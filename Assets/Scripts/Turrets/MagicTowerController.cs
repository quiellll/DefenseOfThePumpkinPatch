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
        _beam.startColor = _magicTower.BeamColor;
        _beam.endColor = _magicTower.BeamColor;
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
        _currentTarget.TakeDamage(_turret.ProjectileDamage);

        _hasAttackColor = true;
        _beam.startColor = _magicTower.AttackColor;
        _beam.endColor = _magicTower.AttackColor;
        Invoke(nameof(ResetBeamColor), _magicTower.AttackColorDuration);
    }

    private void ResetBeamColor()
    {
        _beam.startColor = _magicTower.BeamColor;
        _beam.endColor = _magicTower.BeamColor;
        _hasAttackColor = false;
    }

}
