using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataUpdater : IService
{

    public void UpdateGold(int gold);
    public void UpdateDay(int dayIndex);
    public void UpdateNextDefenseIsDay(bool isDay);
    public void AddTurret(Turret turret, Vector2 cellPos);
    public void RemoveTurret(Turret turret, Vector2 cellPos);
    public void AddPumpkin(Vector2 cellPos);
    public void RemovePumpkin(Vector2 cellPos);
    public void AddSprout(Vector2 cellPos, int journeys);
    public void UpdateSprout(Vector2 cellPos, int journeys);
    public void RemoveSprout(Vector2 cellPos);
    public void AddGrave(Vector2 cellPos, float yRotation);
    public void RemoveGrave(Vector2 cellPos, float yRotation);

    public T GetDataToSave<T>();
    public bool IsDirty();
    public void ClearDirty();
}
