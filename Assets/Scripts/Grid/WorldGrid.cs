using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
//using static GridCell;
using UnityEngine.UIElements;

public class WorldGrid : Singleton<WorldGrid> //singleton (de momento)
{
    //getters de los arrays para que sus elementos sean readonly
    public IReadOnlyList<GridCell> Path { get => _path; }
    public IReadOnlyList<GridCell> Waypoints { get => _waypoints; }
    public UnityEvent<GraveAtPath> GravesUpdated;
    public UnityEvent<GridCell> PumpkinsUpdated;

    //objetos contenedores de las celdas de cada tipo
    [SerializeField] private Transform _pathCellContainer;
    [SerializeField] private Transform _turretCellContainer;
    [SerializeField] private Transform _decorationCellContainer;
    [SerializeField] private Transform _pumpkinCellContainer;

    //arrays del camino y los waypoints (esquinas del camino + inicio + fin, en orden)
    private GridCell[] _path;
    private GridCell[] _waypoints;
    private List<GraveAtPath> _graves = new(); //array con las tumbas en orden
    private LayerMask _gridCells;
    private List<PumpkinDistance> _pumpkins;
    private Collider[] _overlaps = new Collider[4];

    protected override void Awake()
    {
        base.Awake();
        InitPath();

        _gridCells = LayerMask.GetMask("Cell");
    }

    private void Start()
    {
        InitPumpkins();
    }


    
    public GridCell GetCellAt(Vector2 gridPos) //devuelve la celda en la posicion 2d gridPos si la hay
    {
        if (Physics.OverlapSphereNonAlloc(new Vector3(gridPos.x, 0f, gridPos.y), .1f, _overlaps, _gridCells) == 0)
            return null;

        if (_overlaps[0].TryGetComponent<GridCell>(out var cell)) return cell;
        else return null;
    }



    #region Path
    private void InitPath() //guarda el camino y los waypoints en los arrays
    {
        var waypointsList = new List<GridCell>();
        _path = new GridCell[_pathCellContainer.childCount];

        for (int i = 0; i < _path.Length; i++)
        {
            _path[i] = _pathCellContainer.GetChild(i).GetComponent<GridCell>();
            if (_path[i].IsWaypoint) waypointsList.Add(_path[i]);
        }
        _waypoints = waypointsList.ToArray();
    }


    public int GetIndexOfPathCell(GridCell cell) //devuelve el indice en el path de una celda (si esta en el path)
    {
        if (cell == null || cell.Type != GridCell.CellType.Path) return -1;

        return Array.IndexOf(_path, cell);
    }

    #endregion




    #region Graves

    //clase con una tumba y su indice en el path
    public class GraveAtPath
    {
        public Transform Grave; public int PathIndex;
        internal Transform position;

        public GraveAtPath(Transform grave, int index)
        {
            Grave = grave;
            PathIndex = index;
        }
    }


    //instancia una tumba y la añade a la lista de tumbas en orden de menor a mayor indice en el path
    public void BuildGrave(GameObject gravePrefab, GridCell cell, Vector3 position, Quaternion rotation) 
    {
        if (cell.Type != GridCell.CellType.Path) return; //puede morir en pumpkin???

        var g = 
            new GraveAtPath(Instantiate(gravePrefab, position, rotation, transform).transform, GetIndexOfPathCell(cell));

        for (int i = 0; i < _graves.Count; i++)
        {
            if (g.PathIndex >= _graves[i].PathIndex) continue;

            _graves.Insert(i, g);
            return;
        }
        _graves.Add(g);
    }


    public GraveAtPath GetNearestGrave() => _graves.Count > 0 ? _graves[0] : null;
    

    //destruye una tumba, la quita de la lista y llama al evento para actualizar a los fantasmas
    public void DestroyGrave(GraveAtPath grave) 
    {
        if(grave == null) return;

        _graves.Remove(grave);
        Destroy(grave.Grave.gameObject);
        grave.Grave = null;

        GravesUpdated.Invoke(_graves.Count > 0 ? _graves[0] : null);
    }

    #endregion




    #region Pumpkins

    private class PumpkinDistance
    {
        public GridCell Cell { get; private set; }
        public float Distance { get; private set; }

        public PumpkinDistance(GridCell cell)
        {
            Cell = cell;
            Distance = Vector2.Distance(cell.XY, Instance._waypoints[^1].XY);
        }
    }

    private void InitPumpkins()
    {
        _pumpkins = new();
        for (int i = 0; i < _pumpkinCellContainer.childCount; i++)
        {
            var cell = _pumpkinCellContainer.GetChild(i).GetComponent<GridCell>();
            if (!cell.ElementOnTop || cell.ElementOnTop.TryGetComponent<PumpkinSprout>(out _)) continue;

            AddPumpkin(cell);
        }
    }

    //funciones llamadas por las GridCell, no llamarlas directamente
    //si se quiere crear/destruir una calabaza, se hace desde la gridcell correspondiente o con un command
    public bool AddPumpkin(GridCell cell)
    {
        if(cell.Type != GridCell.CellType.Pumpkin) return false;

        var pumpkin = new PumpkinDistance(cell);

        for (int i = 0; i < _pumpkins.Count; i++)
        {
            if (_pumpkins[i].Distance < pumpkin.Distance) continue;
            if (_pumpkins[i].Cell == cell) return false;

            _pumpkins.Insert(i, pumpkin);
            PumpkinsUpdated.Invoke(_pumpkins[0].Cell);
            return true;
        }

        _pumpkins.Add(pumpkin);
        PumpkinsUpdated.Invoke(_pumpkins[0].Cell);
        return true;
    }

    public GridCell GetNearestPumpkinCell() => _pumpkins[0].Cell;

    public bool RemovePumpkin(GridCell cell)
    {
        if (cell.Type != GridCell.CellType.Pumpkin) return false;

        for (int i = 0; i < _pumpkins.Count; i++)
        {
            if(cell == _pumpkins[i].Cell)   
            {
                _pumpkins.RemoveAt(i);
                PumpkinsUpdated.Invoke(_pumpkins[0].Cell);
                return true;
            }
        }

        return false;
    }
    #endregion
}
