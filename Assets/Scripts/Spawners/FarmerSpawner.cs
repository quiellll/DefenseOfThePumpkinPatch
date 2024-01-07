using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerSpawner : AWaveSpawner
{
    [SerializeField] private Level _level;

    protected override void InitSpawnerParameters()
    {
        _poolSize = _level.Days[^1].Farmers;
        _enemiesPerWave = _level.CurrentDay.Farmers;
        _spawnDelay = _level.CurrentDay.SpawnDelay;
    }
}
