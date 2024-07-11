using UnityEngine;
using DG.Tweening;

public class ArrowController : CellObject
{
    const float YSpawnPosition = 0.25f;

    public override void InitializeCellObject(CellProperties cellProperties)
    {
        meshRenderer.material.mainTexture = cellProperties.colorsSO.arrowTexture;
        transform.SetLocalPositionAndRotation(new Vector3(0, YSpawnPosition, 0), Quaternion.Euler(0, (int)cellProperties.direction * 90, 0));
        transform.DOScale(Vector3.one, 0.125f);
    }

    public override void WrongHit(float scaleDuration)
    {
        transform
            .DOScale(Vector3.one * 1.5f, scaleDuration)
            .OnComplete(() =>
            {
                meshRenderer.material
                    .DOColor(Color.red, scaleDuration)
                    .OnComplete(() =>
                    {
                        meshRenderer.material.DOColor(Color.white, scaleDuration);
                        transform.DOScale(Vector3.one, scaleDuration);
                    });
            });
    }

    public override void Hit(float scaleDuration)
    {
        transform.DOLocalMoveY(0.75f, scaleDuration).OnComplete(() => transform.DOLocalMoveY(0.25f, scaleDuration));
    }
}
