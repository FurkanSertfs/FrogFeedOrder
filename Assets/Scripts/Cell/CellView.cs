using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CellView : MonoBehaviour
{
    private List<Cell> cells = new();
    public List<CellProperties> cellProperties;

    public static Action OnFrogSpawned;

    public List<Cell> GetCells() => cells;

    public void SetCellProperties(List<CellProperties> properties)
    {
        cellProperties = properties;
    }

    public void CreateChildCells(Cell prefab, float spacing)
    {
        cells.Clear();

        if (cellProperties.Count == 0)
            return;
        foreach (var property in cellProperties)
        {
            var cell = PoolManager.Instance.Pull("Cell", transform.position, transform.rotation, transform).GetComponent<Cell>();
            float y = (cells.Count + 1) * spacing;
            cell.transform.localPosition = new Vector3(0, y, 0);
            cell.InitializeCell(property);
            cells.Add(cell);

            if (property.cellType == Enums.CellType.Frog)
            {
                OnFrogSpawned?.Invoke();
            }
        }
        cells.Last().ActivateCell();
    }

    public void ActivateLastCell()
    {
        if (cells.Count > 0)
        {
            cells.Last().ActivateCell();
        }
    }

    public CellObject GetCellObject()
    {
        if (cells.Count == 0)
            return null;
        return cells.Last().GetCellObject();
    }

    public void Collect()
    {
        if (cells.Count > 0)
        {
            var activeCell = cells.Last();
            activeCell.transform
                .DOScale(new Vector3(0, activeCell.transform.localScale.y, 0), 0.75f)
                .OnComplete(() =>
                {
                    cells.Remove(activeCell);
                    if (cells.Count > 0)
                    {
                        cells.Last().ActivateCell();
                    }
                });
        }
    }
}
