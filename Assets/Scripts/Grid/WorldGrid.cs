using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

    public GridCell GetCellAt(Vector2 gridPos) //devuelve la celda en la posicion 2d gridPos si la hay
    {
        var cellArray = Physics.OverlapSphere(new Vector3(gridPos.x, 0f, gridPos.y), .1f, _gridCells);
        if (cellArray.Length == 0) return null;

        if (!cellArray[0].TryGetComponent<GridCell>(out var cell)) return null;

        return cell;
    }

    public int GetIndexOfPathCell(GridCell cell) //devuelve el indice en el path de una celda (si esta en el path)
    {
        if (cell == null || cell.Type != GridCell.CellType.Path) return -1;

        return Array.IndexOf(_path, cell);
    }

    #region Graves

    //clase con una tumba y su indice en el path
    public class GraveAtPath
    {
        public Transform Grave; public int PathIndex;
        public GraveAtPath(Transform grave, int index)
        {
            Grave = grave;
            PathIndex = index;
        }
    }


    //añade una tumba a la lista de tumbas en orden de menor a maytor indice en el path
    public void AddGrave(Transform grave, GridCell cell) 
    {
        var g = new GraveAtPath(grave, GetIndexOfPathCell(cell));

        for (int i = 0; i < _graves.Count; i++)
        {
            if (g.PathIndex >= _graves[i].PathIndex) continue;

            _graves.Insert(i, g);
            return;

        }

        _graves.Add(g);
    }

    public GraveAtPath GetNearestGrave() => _graves.Count > 0 ? _graves[0] : null;
    

    //quita una tumba de la lista y llama al evento para actualizar a los fantasmas
    public void RemoveGrave(GraveAtPath grave) 
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
            Distance = Vector2.Distance(cell.XY,
                Instance._waypoints[WorldGrid.Instance._waypoints.Length - 1].XY);
        }
 
    }

    private void InitPumpkins()
    {
        _pumpkins = new();
        for (int i = 0; i < _pumpkinCellContainer.childCount; i++)
        {
            var cell = _pumpkinCellContainer.GetChild(i).GetComponent<GridCell>();
            if (!cell.ElementOnTop) continue;

            //_pumpkins.Add(new PumpkinDistance(cell));
            AddPumpkin(cell);
        }

        //_pumpkins = _pumpkins.OrderBy(p => p.Distance).ToList();

    }

    public bool AddPumpkin(GridCell cell)
    {
        if(cell.Type != GridCell.CellType.Pumpkin || !cell.ElementOnTop) return false;

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
        if (cell.Type != GridCell.CellType.Pumpkin || cell.ElementOnTop) return false;

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
