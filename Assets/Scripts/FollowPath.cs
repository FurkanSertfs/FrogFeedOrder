using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowPath : MonoBehaviour
{
    public bool IsCollected { get; set; }

    [SerializeField]
    GrapeController grapeController;

    public void StartFollowPath(List<Vector3> path, float duration)
    {
        StartCoroutine(FollowPathRoutine(path, duration));
        if (grapeController != null)
        {
            grapeController.Collet();
        }
    }

    IEnumerator FollowPathRoutine(List<Vector3> path, float duration)
    {
        foreach (var point in path)
        {
            transform.DOMove(point, duration).SetEase(Ease.Linear);

            if (point == path[path.Count - 1])
            {
                yield return new WaitForSeconds(duration / 2);
                transform
                    .DOScale(Vector3.zero, duration)
                    .OnComplete(() =>
                    {
                        IsCollected = true;
                        SoundManager.Instance.PlaySound(SoundManager.Sound.CollectibleCollected);
                    });
            }
            yield return new WaitForSeconds(duration);
        }
    }
}
