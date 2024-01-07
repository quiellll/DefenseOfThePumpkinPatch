using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataUpdater : IGameDataUpdater
{
    private bool _dirty = false;
    private GameData _data;


    public void Initialize()
    {
        _data = new();
    }

    public void UpdateGold(int gold)
    {
        _data.Gold = gold;

        _dirty = true;
    }

    public void UpdateDay(int dayIndex)
    {
        _data.DayIndex = dayIndex;

        _dirty = true;
    }

    public void UpdateNextDefenseIsDay(bool isDay)
    {
        _data.NextDefenseIsDay = isDay;

        _dirty = true;
    }

    public void AddTurret(Turret turret, Vector2 cellPos)
    {
        string s = _data.ParseFromVector2(cellPos);
        if (_data.Turrets.ContainsKey(s)) return;

        _data.Turrets.Add(s, turret.Name);

        _dirty = true;
    }

    public void RemoveTurret(Turret turret, Vector2 cellPos)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (!_data.Turrets.ContainsKey(s) || _data.Turrets.GetValueOrDefault(s, null) != turret.Name) return;

        _data.Turrets.Remove(s);

        _dirty = true;
    }

    public void AddPumpkin(Vector2 cellPos)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (_data.Pumpkins.Contains(s)) return;
        _data.Pumpkins.Add(s);

        _dirty = true;
    }

    public void RemovePumpkin(Vector2 cellPos)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (!_data.Pumpkins.Contains(s)) return;
        _data.Pumpkins.Remove(s);

        _dirty = true;
    }

    public void AddSprout(Vector2 cellPos, int journeys)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (_data.Sprouts.ContainsKey(s)) return;
        _data.Sprouts.Add(s, journeys);

        _dirty = true;
    }

    public void UpdateSprout(Vector2 cellPos, int journeys)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (!_data.Sprouts.ContainsKey(s)) return;
        _data.Sprouts[s] = journeys;

        _dirty = true;
    }

    public void RemoveSprout(Vector2 cellPos)
    {
        string s = _data.ParseFromVector2(cellPos);

        if (!_data.Sprouts.ContainsKey(s)) return;
        _data.Sprouts.Remove(s);

        _dirty = true;
    }

    public void AddGrave(Vector2 cellPos, float yRotation)
    {
        string s = _data.ParseFromVector2(cellPos);
        _data.Graves.Add(new(s, yRotation));

        _dirty = true;
    }

    public void RemoveGrave(Vector2 cellPos, float yRotation)
    {
        string s = _data.ParseFromVector2(cellPos);
        _data.Graves.Remove(new(s, yRotation));

        _dirty = true;
    }


    public T GetDataToSave<T>()
    {
        if (typeof(T) != typeof(GameData)) return default;
        
        return (T)(object) _data;
    }

    public bool IsDirty() => _dirty;

    public void ClearDirty() => _dirty = false;
}

