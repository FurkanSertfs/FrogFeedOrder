using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField]
    int gridWidth;

    [SerializeField]
    int gridHeight;

    public int GetGridWidth() => gridWidth;

    public int GetGridHeight() => gridHeight;
}
