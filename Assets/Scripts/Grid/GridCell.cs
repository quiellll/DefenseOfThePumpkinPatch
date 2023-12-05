using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridCell : MonoBehaviour
{
    //getters de posicion 2d en el plano xz
    public int X { get => (int)transform.position.x; }
    public int Y { get => (int)transform.position.z; }
    public Vector2 XY { get => new (X,Y); }

    public enum CellType { Path, Turret, Decoration }
    public CellType Type { get => _type; }
    public bool IsWaypoint {  get => _isWaypoint; } //solo tiene sentido si es de tipo Path
    public GameObject ElementOnTop { get => _elementOnTop; }

    [SerializeField] private CellType _type;
    [SerializeField] private bool _isWaypoint;

    private GameObject _elementOnTop;

    public bool BuildTurret(Turret turret)
    {
        if (_elementOnTop) return false;

        _elementOnTop = Instantiate(turret.Prefab, transform.position, turret.Prefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }
}
