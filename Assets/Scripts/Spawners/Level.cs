using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]

public class Level : ScriptableObject
{
    public IReadOnlyList<Day> Days { get => _days; }
    public int DayCount { get => _days.Length; }
    public Day CurrentDay { get => _days[_currentDayIndex]; }
    public bool IsOnLastDay { get => _currentDayIndex == DayCount - 1; }
    public int CurrentDayIndex { get => _currentDayIndex; }

    [SerializeField] private Day[] _days;

    private int _currentDayIndex = 0;

    public Day NextDay()
    {
        _currentDayIndex++;
        if (_currentDayIndex < DayCount) return _days[_currentDayIndex];

        _currentDayIndex = DayCount - 1;
        return null;
    }

    public void SetDay(int dayIndex) => _currentDayIndex = dayIndex;

}

[Serializable]
public class Day
{
    public int Farmers { get => _farmers; }
    public int Ghosts { get => _ghosts; }
    public int SpawnDelay { get => _spawnDelay; }

    [SerializeField] private int _farmers;
    [SerializeField] private int _ghosts;
    [SerializeField] private int _spawnDelay;
}
