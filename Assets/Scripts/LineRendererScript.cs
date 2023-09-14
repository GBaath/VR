using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScript : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLineRenderer();
    }

    void Update()
    {
        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null && pointA != null && pointB != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, pointA.position);
            lineRenderer.SetPosition(1, pointB.position);
        }
    }
}
