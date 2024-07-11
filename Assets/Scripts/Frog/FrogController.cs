using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrogController : CellObject, IClickable
{
    [SerializeField]
    FrogAnimator frogAnimator;

    [SerializeField]
    FrogTongueController frogTongueController;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

    const float ScaleDuration = 0.125f;
    const float TongueSpeed = 5f;
    const float YSpawnPosition = 0.25f;
    const float TongueStepDelay = 0.2f;

    public void OnClick()
    {
        frogTongueController.ExtendTongue();
    }

    public override void InitializeCellObject(CellProperties cellProperties)
    {
        skinnedMeshRenderer.material.mainTexture = cellProperties.colorsSO.frogTexture;
        transform.localRotation = Quaternion.Euler(0, (int)cellProperties.direction * 90, 0);
        transform.DOScale(Vector3.one, 0.25f);
        transform.localPosition = new Vector3(0, YSpawnPosition, 0);
        frogTongueController.InitializeTongue(ScaleDuration, TongueSpeed, YSpawnPosition, TongueStepDelay, cellProperties);
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
