using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    int moveCount = 5;

    [SerializeField]
    int level = 15;

    [SerializeField]
    List<CellPresenter> cellPresenters;

    public static Action<int> OnSetMove;
    public static Action<int> OnLevelStart;

    public void InvokeEvents()
    {
        OnSetMove?.Invoke(moveCount);
        OnLevelStart?.Invoke(level);
    }

    public List<List<CellProperties>> GetCellProperties()
    {
        List<List<CellProperties>> cellProperties = new List<List<CellProperties>>();

        foreach (var cellPresenter in cellPresenters)
        {
            cellProperties.Add(cellPresenter.GetCellProperties());
        }

        return cellProperties;
    }
}
