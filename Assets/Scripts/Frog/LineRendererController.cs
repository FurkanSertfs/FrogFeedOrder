using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform mouthTransform;
    private FollowPath pathFollowerPrefab;

    public void Initialize(LineRenderer lineRenderer, Transform mouthTransform, FollowPath pathFollowerPrefab)
    {
        this.lineRenderer = lineRenderer;
        this.mouthTransform = mouthTransform;
        this.pathFollowerPrefab = pathFollowerPrefab;
    }

    public void ExtendTongue()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, mouthTransform.position);
    }

    public IEnumerator ExtendTongueRoutine(Vector3 lineEndPoint, float tongueSpeed)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.GetPosition(lineRenderer.positionCount - 2));
        while (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineEndPoint) > 0.1f)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, Vector3.MoveTowards(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineEndPoint, tongueSpeed * Time.deltaTime));
            yield return null;
        }

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineEndPoint);
    }

    public IEnumerator RetractTongue(float TongueSpeed)
    {
        while (lineRenderer.positionCount > 1)
        {
            while (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineRenderer.GetPosition(lineRenderer.positionCount - 2)) > 0.1f)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, Vector3.MoveTowards(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineRenderer.GetPosition(lineRenderer.positionCount - 2), TongueSpeed * Time.deltaTime));
                yield return null;
            }
            lineRenderer.positionCount--;
        }
    }

    public IEnumerator TongueFollowGrapes(List<GameObject> grapes)
    {
        lineRenderer.positionCount = grapes.Count + 1;
        lineRenderer.SetPosition(0, mouthTransform.position);
        grapes.Insert(grapes.Count, mouthTransform.gameObject);
        var pathFollower = grapes[0].GetComponent<FollowPath>();

        while (!pathFollower.IsCollected)
        {
            lineRenderer.SetPositions(grapes.ConvertAll(grape => grape.transform.position).ToArray());
            yield return null;
        }
    }

    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }
}
