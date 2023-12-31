using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

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
    //public GameObject ElementOnTop { get => _elementOnTop && _elementOnTop.activeSelf ? _elementOnTop : null; } 
    public GameObject ElementOnTop { get => _elementOnTop; } 

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

    public bool BuildWare(IWare ware, GameObject prefab = null)
    {
        if (_elementOnTop || Type != ware.CellType) return false;

        prefab = prefab == null ? ware.Prefab : prefab;

        _elementOnTop = Instantiate(prefab, transform.position, prefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    public bool DestroyWare()
    {
        if (!ElementOnTop) return false;

        Destroy(_elementOnTop);
        _elementOnTop = null;
        return true;
    }


    #region Pumpkins


    //la llama el brote para cosntruir uan calabaza y destruir el brote
    public bool BuildPumpkin(Pumpkin pumpkin)
    {
        if(Type != CellType.Pumpkin || !ElementOnTop) return false;
        if(!ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) return false;

        Destroy(_elementOnTop); //se destruye el brote
        _elementOnTop = Instantiate(pumpkin.PumpkinPrefab, transform.position, pumpkin.PumpkinPrefab.transform.rotation);
        _elementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    //esta se usa para cuando los enemigos destruyen la calabaza, para venderla se usa el command SellPumpkin
    //por tanto solo se debe llamar en modo defensa y no debe interferir con los commands nunca
    public bool DestroyPumpkin()
    {
        if (Type != CellType.Pumpkin || !ElementOnTop || ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) return false;

        Destroy(_elementOnTop);
        return true;
    }

    #endregion
}
