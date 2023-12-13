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
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public bool BuildTurret(Turret turret)
    {
        if (_elementOnTop || Type != CellType.Turret) return false;

        _elementOnTop = Instantiate(turret.Prefab, transform.position, turret.Prefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    public void BuildGrave(GameObject grave, Vector3 position, Quaternion rotation)
    {
        if(Type != CellType.Path) return;

        //Vector3 randomPos = transform.position;
        //randomPos.x = Random.Range(transform.position.x - _collider.size.x / 2f,
        //    transform.position.x + _collider.size.x / 2f);
        //randomPos.z = Random.Range(transform.position.z - _collider.size.z / 2f,
        //    transform.position.z + _collider.size.z / 2f);

        var gt = Instantiate(grave, position, rotation).transform;
        WorldGrid.Instance.AddGrave(gt, this);
    }
}
