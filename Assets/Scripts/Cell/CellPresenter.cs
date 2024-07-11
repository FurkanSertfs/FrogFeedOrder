using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellPresenter : MonoBehaviour
{
    private CellModel model;
    private CellView view;

    [SerializeField]
    private Cell cellPrefab;

    [SerializeField]
    private float ySpacing = 0.02f;

    private void Awake()
    {
        model = new CellModel();
        view = GetComponent<CellView>();
    }

    public void InitializeCell(int x, int y)
    {
        view.CreateChildCells(cellPrefab, ySpacing);
        model.InitializeCell(x, y, view.GetCells());
    }

    public int GetX() => model.CellX;

    public int GetY() => model.CellY;

    public bool IsCellEmpty() => model.IsCellEmpty();

    public Vector3 GetActiveCellTransform() => model.GetActiveCellTransform(transform.position);

    public CellProperties GetActiveCellProperties() => model.GetActiveCellProperties();

    public void SetCellProperties(List<CellProperties> properties)
    {
        view.SetCellProperties(properties);
    }

    public List<CellProperties> GetCellProperties()
    {
        if (view == null)
            view = GetComponent<CellView>();

        return view.cellProperties;
    }

    public List<CellPresenter> GetCells()
    {
        var direction = model.GetActiveCellProperties().direction;
        var gridController = GridPresenter.Instance;
        var cellPresenters = new List<CellPresenter> { this };

        CellPresenter currentPresenter = this;
        while (true)
        {
            var nextCell = gridController.GetNextCell(currentPresenter, direction);
            if (nextCell == null || nextCell.IsCellEmpty())
                break;

            currentPresenter = nextCell;
            cellPresenters.Add(nextCell);

            if (nextCell.GetActiveCellProperties().cellType == Enums.CellType.Arrow)
            {
                direction = currentPresenter.GetActiveCellProperties().direction;
            }
        }
        cellPresenters.Remove(this);
        return cellPresenters;
    }

    public CellObject GetCellObject()
    {
        return view.GetCellObject();
    }

    public void Collect()
    {
        view.Collect();
    }

    public void OnHit(CellProperties frogCellProperties, out bool sameColor, out bool isLastCell, out bool isArrow, out CellObject cellObject)
    {
        sameColor = false;
        isLastCell = false;
        isArrow = false;
        cellObject = null;

        if (model.IsCellEmpty())
            return;

        var activeCellProperties = model.GetActiveCellProperties();
        sameColor = activeCellProperties.colorsSO == frogCellProperties.colorsSO;
        isLastCell = GridPresenter.Instance.IsLastCell(this, frogCellProperties.direction);
        isArrow = activeCellProperties.cellType == Enums.CellType.Arrow;
        cellObject = view.GetCellObject();
    }
}
