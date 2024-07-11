using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void AnimateScale(Transform transform, float scaleDuration, float targetScale)
    {
        transform.DOScale(Vector3.one * targetScale, scaleDuration).OnComplete(() => transform.DOScale(Vector3.one, scaleDuration));
    }

    public void ProcessCollectibleGrape(CellObject cellObject, float scaleDuration)
    {
        if (cellObject == null)
            return;
        cellObject.Hit(scaleDuration);
        SoundManager.Instance.PlaySound(SoundManager.Sound.HitGrape);
    }

    public void AnimateNonCollectibleCellObject(CellObject cellObject, float scaleDuration)
    {
        if (cellObject == null)
            return;
        cellObject.WrongHit(scaleDuration);
        SoundManager.Instance.PlaySound(SoundManager.Sound.WrongHit);
    }
}
