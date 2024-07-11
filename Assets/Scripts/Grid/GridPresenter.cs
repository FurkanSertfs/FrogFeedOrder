using UnityEngine;

public class GridPresenter : MonoSingleton<GridPresenter>
{
    private GridModel model;

    [SerializeField]
    private GridView view;

    void OnEnable()
    {
        GameManager.OnNewLevelCreated += Initialize;
    }

    void OnDisable()
    {
        GameManager.OnNewLevelCreated -= Initialize;
    }

    public void Initialize()
    {
        model = new GridModel();
        model.InitializeGrid(view.GetGridWidth(), view.GetGridHeight(), transform);
    }

    public CellPresenter GetNextCell(CellPresenter currentCell, Enums.Direction direction)
    {
        return model.GetNextCell(currentCell, direction);
    }

    public bool IsLastCell(CellPresenter cell, Enums.Direction direction)
    {
        return model.IsLastCell(cell, direction);
    }
}
