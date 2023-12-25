using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridCell : MonoBehaviour //clase de cada celda del mapa
{
    //getters de posicion 2d en el plano xz
    public int X { get => (int)transform.position.x; }
    public int Y { get => (int)transform.position.z; }
    public Vector2 XY { get => new (X,Y); }

    public enum CellType { Path, Turret, Decoration, Pumpkin} //tipo de celda
    public CellType Type { get => _type; }
    public bool IsWaypoint {  get => _isWaypoint; } //solo tiene sentido si es de tipo Path
    public GameObject ElementOnTop { get => _elementOnTop; } //objeto sobre la celda, de momento solo torreta

    [SerializeField] private CellType _type;
    [SerializeField] private bool _isWaypoint;

    private GameObject _elementOnTop;
    private BoxCollider _collider;



    public bool BuildTurret(Turret turret) //instancia una torreta sobre la celda
    {
        if (_elementOnTop || Type != CellType.Turret) return false;

        _elementOnTop = Instantiate(turret.Prefab, transform.position, turret.Prefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    public void BuildGrave(GameObject grave, Vector3 position, Quaternion rotation) //instancia una tumba sobre la celda
    {
        if(Type != CellType.Path) return;

        var gt = Instantiate(grave, position, rotation).transform;
        WorldGrid.Instance.AddGrave(gt, this);
    }
}
