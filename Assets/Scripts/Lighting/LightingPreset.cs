using UnityEngine;

//https://gist.github.com/Glynn-Taylor/08da28896147faa6ba8f9654057d38e6
[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Lighting Preset")]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
}