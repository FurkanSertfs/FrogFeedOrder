using DG.Tweening;
using UnityEngine;

public abstract class CellObject : MonoBehaviour
{
    public Renderer meshRenderer;
    public abstract void WrongHit(float scaleDuration);
    public abstract void Hit(float scaleDuration);
    public abstract void InitializeCellObject(CellProperties cellProperties);
}
