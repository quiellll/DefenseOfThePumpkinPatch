using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    //temporales para debug
    private Renderer _renderer;
    private Color _color;

    private GameObject _elementOnTop;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _color = _renderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WorldGrid.Instance.PointerOnCell(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WorldGrid.Instance.PointerOffCell(this);
    }

    public bool BuildTurret(Turret turret)
    {
        if (_elementOnTop) return false;

        _elementOnTop = Instantiate(turret.Prefab, transform.position, turret.Prefab.transform.rotation,
            transform);
        return true;
    }

    //para debug
    //private void OnMouseDown() => Debug.Log(XY);
    //private void OnMouseEnter() => _renderer.material.color = Color.red;
    //private void OnMouseExit() => _renderer.material.color = _color;

}
