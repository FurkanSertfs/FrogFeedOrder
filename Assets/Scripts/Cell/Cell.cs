using UnityEngine;
using DG.Tweening;
using System;

public class Cell : MonoBehaviour
{
    public CellProperties GetCellProperties() => cellProperties;

    public CellObject GetCellObject() => cellObject;

    [SerializeField]
    ArrowController arrowPrefab;

    [SerializeField]
    FrogController frogPrefab;

    [SerializeField]
    GrapeController grapePrefab;

    CellProperties cellProperties;
    CellObject cellObject;

    public void InitializeCell(CellProperties properties)
    {
        cellProperties = properties;
        GetComponent<MeshRenderer>().material.mainTexture = properties.colorsSO.cellTexture;
        transform.localScale = Vector3.one;
    }

    public void ActivateCell()
    {
        cellObject = CreateCellObject().GetComponent<CellObject>();
        cellObject.InitializeCellObject(cellProperties);
    }

    private GameObject CreateCellObject()
    {
        switch (cellProperties.cellType)
        {
            case Enums.CellType.Frog:
                return PoolManager.Instance.Pull("Frog", transform.position, transform.rotation, transform);
            case Enums.CellType.Berry:
                return PoolManager.Instance.Pull("Grape", transform.position, transform.rotation, transform.parent);
            case Enums.CellType.Arrow:
                return PoolManager.Instance.Pull("Arrow", transform.position, transform.rotation, transform);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
