using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : AWaveSpawner
{

    [SerializeField] private Level _level;
    protected override void InitSpawnerParameters()
    {
        _poolSize = _level.Days[^1].Ghosts;
        _enemiesPerWave = _level.CurrentDay.Ghosts;
        _spawnDelay = _level.CurrentDay.GhostSpawnDelay;
    }
}
