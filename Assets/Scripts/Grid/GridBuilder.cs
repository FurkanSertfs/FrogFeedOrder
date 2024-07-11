using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField]
    GameObject gridPrefab;

    [SerializeField]
    int gridWidth;

    [SerializeField]
    int gridHeight;

    [SerializeField]
    Vector3 gridOrigin;

    [ContextMenu("Intialize Grid")]
    void IntializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 gridPosition = new Vector3(x, y, 0) + gridOrigin;
                print(gridPosition);
                GameObject grid = Instantiate(gridPrefab, gridPosition, Quaternion.identity);
                grid.name = "Cell [ " + x + " " + y + " ]";
                grid.transform.parent = transform;
            }
        }
    }
}
