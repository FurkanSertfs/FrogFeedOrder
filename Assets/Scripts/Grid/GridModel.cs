using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModel
{
    CellPresenter[,] grid;

    public void InitializeGrid(int width, int height, Transform transform)
    {
        grid = new CellPresenter[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var childIndex = x * width + y;

                if (childIndex < transform.childCount)
                {
                    CellPresenter cell = transform.GetChild(childIndex).GetComponent<CellPresenter>();

                    if (cell != null)
                    {
                        grid[x, y] = cell;

                        cell.InitializeCell(x, y);
                    }
                }
            }
        }
    }

    public CellPresenter GetCell(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            return grid[x, y];
        }
        return null;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
    }

    public CellPresenter GetNextCell(CellPresenter cellPresenter, Enums.Direction direction)
    {
        int x = cellPresenter.GetX();
        int y = cellPresenter.GetY();

        switch (direction)
        {
            case Enums.Direction.Up:
                y += 1;
                break;
            case Enums.Direction.Down:
                y -= 1;
                break;
            case Enums.Direction.Left:
                x -= 1;
                break;
            case Enums.Direction.Right:
                x += 1;
                break;
        }

        return GetCell(x, y);
    }

    public bool IsLastCell(CellPresenter cellPresenter, Enums.Direction direction)
    {
        int x = cellPresenter.GetX();
        int y = cellPresenter.GetY();

        return direction switch
        {
            Enums.Direction.Up => y == grid.GetLength(1) - 1,
            Enums.Direction.Down => y == 0,
            Enums.Direction.Left => x == 0,
            Enums.Direction.Right => x == grid.GetLength(0) - 1,
            _ => false,
        };
    }
}
