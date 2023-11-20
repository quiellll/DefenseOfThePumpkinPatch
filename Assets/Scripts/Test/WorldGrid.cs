using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : Singleton<WorldGrid>
{
    public GridCell[] Path { get { return _path; } }

    [SerializeField] private Transform _pathCellContainer;
    [SerializeField] private Transform _turretCellContainer;
    [SerializeField] private Transform _decorationCellContainer;

    private GridCell[] _path;


    protected override void Awake()
    {
        base.Awake();
        StorePath();
    }

    private void StorePath()
    {
        _path = new GridCell[_pathCellContainer.childCount];

        for (int i = 0; i < _path.Length; i++)
        {
            _path[i] = _pathCellContainer.GetChild(i).GetComponent<GridCell>();
        }
    }


}
