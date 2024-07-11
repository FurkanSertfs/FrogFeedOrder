using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FrogTongueController : MonoBehaviour
{
    public static Func<bool> OnFrogCompleted;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform mouthTransform;

    [SerializeField]
    private FollowPath pathFollowerPrefab;

    private FrogAnimator frogAnimator;
    private Coroutine tongueRoutine;

    private float tongueSpeed = 5f;
    private float ySpawnPosition = -0.25f;
    private float scaleDuration = 0.125f;
    private float tongueStepDelay = 0.2f;

    private CellPresenter cellPresenter;
    private CellProperties cellProperties;
    private CellProperties defaultCellProperties;

    private List<Vector3> controllerPositions = new();
    private LineRendererController lineRendererController;
    private List<GameObject> grapes = new List<GameObject>();

    private void Start()
    {
        lineRendererController = GetComponent<LineRendererController>();
        lineRendererController.Initialize(lineRenderer, mouthTransform, pathFollowerPrefab);
        frogAnimator = GetComponent<FrogAnimator>();
    }

    public void InitializeTongue(float scaleDuration, float tongueSpeed, float ySpawnPosition, float tongueStepDelay, CellProperties cellProperties)
    {
        this.scaleDuration = scaleDuration;
        this.tongueSpeed = tongueSpeed;
        this.ySpawnPosition = ySpawnPosition;
        this.cellProperties = cellProperties;
        this.tongueStepDelay = tongueStepDelay;
        defaultCellProperties = cellProperties;
        cellPresenter = GetComponentInParent<CellPresenter>();
    }

    public void ExtendTongue()
    {
        if (tongueRoutine == null)
        {
            if (!MovePresenter.Instance.UseMove())
                return;
            tongueRoutine = StartCoroutine(ExtendTongueRoutine());
        }
    }

    private IEnumerator ExtendTongueRoutine()
    {
        frogAnimator.AnimateScale(transform, scaleDuration, 1.3f);
        frogAnimator.SetTrigger("ExtendTongue");
        yield return new WaitForSeconds(0.125f);
        lineRendererController.ExtendTongue();

        cellProperties = defaultCellProperties;
        controllerPositions.Clear();
        controllerPositions.Add(mouthTransform.position);
        grapes.Clear();

        var cellPresenters = cellPresenter.GetCells();

        foreach (var presenter in cellPresenters)
        {
            Vector3 cellSpawnPosition = presenter.transform.TransformPoint(new Vector3(0, ySpawnPosition, 0));
            Vector3 tongueEnd = cellSpawnPosition;

            controllerPositions.Add(tongueEnd);

            yield return lineRendererController.ExtendTongueRoutine(tongueEnd, tongueSpeed);

            HandleTargetCell(presenter);
        }
    }

    void HandleTargetCell(CellPresenter presenter)
    {
        presenter.OnHit(cellProperties, out bool sameColor, out bool isLastCell, out bool isArrow, out CellObject cellObjebts);

        if (sameColor && !isArrow)
        {
            if (cellObjebts != null)
            {
                frogAnimator.ProcessCollectibleGrape(cellObjebts, scaleDuration);

                grapes.Add(cellObjebts.gameObject);
            }

            if (isLastCell && tongueRoutine != null)
            {
                StopCoroutine(tongueRoutine);

                tongueRoutine = StartCoroutine(RetractTongue());
            }
        }
        else if (isArrow)
        {
            var pathFollower = Instantiate(pathFollowerPrefab, presenter.transform.position, Quaternion.identity, presenter.transform);
            pathFollower.transform.localPosition = new Vector3(0, ySpawnPosition / 2, 0);
            grapes.Add(pathFollower.gameObject);
            cellProperties = presenter.GetActiveCellProperties();
            return;
        }
        else
        {
            frogAnimator.AnimateNonCollectibleCellObject(cellObjebts, scaleDuration);

            if (tongueRoutine != null)
            {
                StopCoroutine(tongueRoutine);
                tongueRoutine = StartCoroutine(RetractTongue(false));
            }
        }
    }

    private IEnumerator RetractTongue(bool collectGrapes = true)
    {
        yield return new WaitForSeconds(0.5f);

        if (collectGrapes)
        {
            StartCoroutine(CollectGrapes());
            yield break;
        }

        yield return lineRendererController.RetractTongue(tongueSpeed);
        frogAnimator.SetTrigger("RetractTongue");
        tongueRoutine = null;
        MovePresenter.Instance.ControlMove();
    }

    IEnumerator CollectGrapes()
    {
        controllerPositions.Reverse();
        grapes.Reverse();
        controllerPositions.RemoveAt(0);
        StartCoroutine(lineRendererController.TongueFollowGrapes(grapes));
        var cellPresenters = cellPresenter.GetCells();
        cellPresenters.Reverse();

        for (int i = 0; i < grapes.Count - 1; i++)
        {
            cellPresenters[i].Collect();
            var positionsForGrapes = new List<Vector3>(controllerPositions);
            for (int j = 0; j < i; j++)
                positionsForGrapes.RemoveAt(0);
            grapes[i].GetComponent<FollowPath>().StartFollowPath(positionsForGrapes, tongueStepDelay);
            yield return new WaitForSeconds(tongueStepDelay - 0.08f);

            if (grapes[i].GetComponent<GrapeController>() == null)
            {
                yield return new WaitForSeconds(tongueStepDelay - 0.08f);
            }
        }
        var pathFollower = grapes[0].GetComponent<FollowPath>();
        yield return new WaitUntil(() => pathFollower.IsCollected);
        frogAnimator.SetTrigger("RetractTongue");
        lineRendererController.ClearLineRenderer();
        yield return new WaitForSeconds(0.25f);

        transform
            .DOScale(Vector3.zero, 0.25f)
            .OnComplete(() =>
            {
                tongueRoutine = null;
                cellPresenter.Collect();
            });

        yield return new WaitForSeconds(0.35f);
        if (OnFrogCompleted?.Invoke() != true)
        {
            MovePresenter.Instance.ControlMove();
        }
    }
}
