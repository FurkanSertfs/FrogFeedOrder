using UnityEngine;
using DG.Tweening;

public class GrapeController : CellObject, IClickable
{
    bool isCollected;

    private void OnEnable()
    {
        isCollected = false;
    }

    public void Collet()
    {
        isCollected = true;
    }

    public void OnClick()
    {
        if (isCollected)
            return;
        transform.DOScale(Vector3.one * 1.5f, 0.125f).OnComplete(() => transform.DOScale(Vector3.one, 0.125f));
        SoundManager.Instance.PlaySound(SoundManager.Sound.HitGrape);
    }

    public override void InitializeCellObject(CellProperties cellProperties)
    {
        if (isCollected)
            return;

        meshRenderer.material.mainTexture = cellProperties.colorsSO.grapeTexture;
        transform.DOScale(Vector3.one, 0.25f);
    }

    public override void WrongHit(float scaleDuration)
    {
        if (isCollected)
            return;

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
        if (isCollected)
            return;

        transform.DOLocalMoveY(0.75f, scaleDuration).OnComplete(() => transform.DOLocalMoveY(0.25f, scaleDuration));
    }
}
