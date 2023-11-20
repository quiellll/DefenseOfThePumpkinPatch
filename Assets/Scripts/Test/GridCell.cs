using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int X { get { return (int) transform.position.x; } }
    public int Y { get { return (int) transform.position.z; } }

    public CellType Type { get { return _type;} }

    [SerializeField] private CellType _type;

    public enum CellType
    {
        Path, Turret, Decoration
    }

}
