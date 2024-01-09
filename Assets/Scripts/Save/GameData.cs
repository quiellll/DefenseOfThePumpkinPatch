using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Gold = 0;
    public int DayIndex = 0;
    public bool NextDefenseIsDay = true;
    public bool FirstNightGiftReceived = false;
    public List<string/*parsed vector2 pos*/> Pumpkins = new();
    public Dictionary<string/*parsed vector2 pos*/, int/*journeys*/> Sprouts = new();
    public Dictionary<string/*parsed vector2 pos*/, string/*turret name*/> Turrets = new();

    public List<Tuple<string/*parsed vector2 pos*/, float/*y angle*/>> Graves = new();

    public string ParseFromVector2(Vector2 v) => $"{v.x} {v.y}";

    public bool ParseToVector2(string s, out Vector2 v)
    {
        var split = s.Split(" ");

        if (split == null || split.Length != 2)
        {
            v = new();
            return false;
        }

        v = new(float.Parse(split[0]), float.Parse(split[1]));
        return true;
    }

    public void Print()
    {
        Debug.Log("data saved:");
        Debug.Log($"gold: {Gold}");
        Debug.Log($"day: {DayIndex}");
        Debug.Log($"next day?: {NextDefenseIsDay}");
        Debug.Log($"gift?: {FirstNightGiftReceived}");
        Debug.Log("turrets");
        foreach (var kv in Turrets) Debug.Log(kv.Key + " -> " + kv.Value);
        Debug.Log("sprouts");
        foreach (var kv in Sprouts) Debug.Log(kv.Key + " -> " + kv.Value);
        Debug.Log("pumpkins:");
        foreach (var p in Pumpkins) Debug.Log(p);
        Debug.Log("graves:");
        foreach(var g in Graves) Debug.Log(g);
    }
}
