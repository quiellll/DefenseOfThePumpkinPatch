using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldGrid : Singleton<WorldGrid> //singleton (de momento)
{
    //getters de los arrays para que sus elementos sean readonly
    public IReadOnlyList<GridCell> Path { get => _path; }
    public IReadOnlyList<GridCell> Waypoints { get => _waypoints; }
    public UnityEvent<GraveAtPathIndex> GravesUpdated;

    [SerializeField] private Transform _pathCellContainer;
    [SerializeField] private Transform _turretCellContainer;
    [SerializeField] private Transform _decorationCellContainer;

    //arrays del camino y los waypoints (esquinas del camino + inicio + fin, en orden)
    private GridCell[] _path;
    private GridCell[] _waypoints;
    private List<GraveAtPathIndex> _graves = new();
    private LayerMask _gridCells;

    protected override void Awake()
    {
        base.Awake();
        InitPath();
        _gridCells = LayerMask.GetMask("Cell");
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

    public GridCell GetCellAt(Vector2 gridPos)
    {
        var cellArray = Physics.OverlapSphere(new Vector3(gridPos.x, 0f, gridPos.y), .1f, _gridCells);
        if (cellArray.Length == 0) return null;

        if (!cellArray[0].TryGetComponent<GridCell>(out var cell)) return null;

        return cell;
    }

    public int GetIndexOfPathCell(GridCell cell)
    {
        if (cell == null || cell.Type != GridCell.CellType.Path) return -1;

        return Array.IndexOf(_path, cell);
    }

    #region Graves

    //[Serializable]
    public class GraveAtPathIndex
    {
        public Transform Grave; public int PathIndex;
        public GraveAtPathIndex(Transform grave, int index)
        {
            Grave = grave;
            PathIndex = index;
        }
    }

    public void AddGrave(Transform grave, GridCell cell)
    {
        var g = new GraveAtPathIndex(grave, GetIndexOfPathCell(cell));

        for (int i = 0; i < _graves.Count; i++)
        {
            if (g.PathIndex >= _graves[i].PathIndex) continue;

            _graves.Insert(i, g);
            return;

        }

        _graves.Add(g);
    }

    public GraveAtPathIndex GetNearestGrave()
    {
        return _graves[0];
    }

    public void RemoveGrave(GraveAtPathIndex grave)
    {
        if(grave == null) return;

        Debug.Log("quitada tumba del indice " + grave.PathIndex);

        _graves.Remove(grave);
        Destroy(grave.Grave.gameObject);
        grave.Grave = null;

        GravesUpdated.Invoke(_graves.Count > 0 ? _graves[0] : null);


    }

    #endregion

}
