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
    public GameObject ElementOnTop { get; private set; } 

    [SerializeField] private CellType _type;
    [SerializeField] private bool _isWaypoint;

    private void Awake()
    {
        if(Type == CellType.Pumpkin)
        {
            if(transform.childCount == 0)
            {
                ElementOnTop = null;
                return;
            }

            //le quitamos la calabaza de hija para que pueda ser seleccionada
            ElementOnTop = transform.GetChild(0).gameObject;
            ElementOnTop.transform.SetParent(null);

        }
    }

    //construye un brote o torreta, o calabaza (solo para deshacer venderla)
    public bool BuildWare(IWare ware, GameObject prefab = null)
    {
        if (ElementOnTop || Type != ware.CellType) return false;

        prefab = prefab == null ? ware.Prefab : prefab;

        ElementOnTop = Instantiate(prefab, transform.position, prefab.transform.rotation);
        ElementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    //destruye torreta o calabaza
    public bool DestroyWare()
    {
        if (!ElementOnTop) return false;

        if (ElementOnTop.TryGetComponent<PumpkinController>(out var pc)) pc.DestroyPumpkin();
        else Destroy(ElementOnTop);

        ElementOnTop = null;
        return true;
    }


    #region Pumpkins


    //la llama el brote para construir una calabaza y destruir el brote
    public bool BuildPumpkin(Pumpkin pumpkin)
    {
        if(Type != CellType.Pumpkin || !ElementOnTop) return false;
        if(!ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) return false;

        Destroy(ElementOnTop); //se destruye el brote
        ElementOnTop = Instantiate(pumpkin.PumpkinPrefab, transform.position, pumpkin.PumpkinPrefab.transform.rotation);
        ElementOnTop.transform.Translate(0f, .1f, 0f);
        return true;
    }

    //esta se usa para cuando los enemigos destruyen la calabaza, para venderla se usa el command SellPumpkin
    //por tanto solo se debe llamar en modo defensa y no debe interferir con los commands nunca
    public bool DestroyPumpkin()
    {
        if (Type != CellType.Pumpkin || !ElementOnTop || !ElementOnTop.TryGetComponent<PumpkinController>(out var pc))
            return false;

        pc.DestroyPumpkin();
        ElementOnTop = null;

        return true;
    }

    #endregion
}
