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
    //objeto sobre la celda, de momento torreta o calabaza
    public GameObject ElementOnTop { get => _elementOnTop && _elementOnTop.activeSelf ? _elementOnTop : null; } 

    [SerializeField] private CellType _type;
    [SerializeField] private bool _isWaypoint;

    private GameObject _elementOnTop;

    private void Awake()
    {
        if(Type == CellType.Pumpkin)
        {
            if(transform.childCount == 0)
            {
                _elementOnTop = null;
                return;
            }

            //le quitamos la calabaza de hija para que pueda ser seleccionada
            _elementOnTop = transform.GetChild(0).gameObject;
            _elementOnTop.transform.SetParent(null);

        }
    }

    public bool BuildTurret(Turret turret) //instancia una torreta sobre la celda
    {
        if (_elementOnTop || Type != CellType.Turret) return false;

        _elementOnTop = Instantiate(turret.Prefab, transform.position, turret.Prefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    public bool SellTurret()
    {
        if (!ElementOnTop || Type != CellType.Turret) return false;

        Destroy(_elementOnTop);
        _elementOnTop = null;
        return true;
    }

    public void BuildGrave(GameObject gravePrefab, Vector3 position, Quaternion rotation) //instancia una tumba sobre la celda
    {
        if(Type != CellType.Path) return; //puede morir en pumpkin???

        var gt = Instantiate(gravePrefab, position, rotation, transform).transform;
        WorldGrid.Instance.AddGrave(gt, this);
    }

    public bool BuildPumpkin(GameObject pumpkinPrefab)
    {
        if(Type != CellType.Pumpkin) return false;
        if (ElementOnTop) return false; //si el top no es nulo y esta activado

        //si no es nulo y no returneamos antes, significa que ya existe un top pero esta desactivado 
        if(_elementOnTop)
        {
            _elementOnTop.SetActive(true);
            WorldGrid.Instance.AddPumpkin(this);
            return true;
        }
        //si no existe lo instanciamos
        _elementOnTop = Instantiate(pumpkinPrefab, transform.position, pumpkinPrefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        WorldGrid.Instance.AddPumpkin(this);
        return true;
    }

    public bool DestroyPumpkin()
    {
        if (Type != CellType.Pumpkin) return false;
        if(!ElementOnTop) return false;

        _elementOnTop.SetActive(false);
        WorldGrid.Instance.RemovePumpkin(this);
        return true;
    }
}
