using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellModel
{
    public List<Cell> Cells { get; set; } = new List<Cell>();
    public int CellX { get; private set; }
    public int CellY { get; private set; }

    public void InitializeCell(int x, int y, List<Cell> cells)
    {
        CellX = x;
        CellY = y;
        Cells = cells;
    }

    public bool IsCellEmpty() => Cells.Count == 0;

    public Vector3 GetActiveCellTransform(Vector3 defaultPosition) => Cells.Count == 0 ? defaultPosition : Cells.Last().transform.position;

    public CellProperties GetActiveCellProperties() => Cells.Count == 0 ? null : Cells.Last().GetCellProperties();
}
