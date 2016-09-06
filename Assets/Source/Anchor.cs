using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [SerializeField]
    List<BezierSpline> splines = new List<BezierSpline>();

    [SerializeField]
    private Anchor _nextAnchor;

    public BezierSpline[] Splines
    {
        get
        {
            return splines.ToArray();
        }
    }

    public Anchor NextAnchor
    {
        get
        {
            return _nextAnchor;
        }
        set
        {
            _nextAnchor = value;

            if (_nextAnchor == null)
            {
                CleanupSplines();
            }
            else
            {
                CleanupSplines();
                BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
                splines.Add(spline);
            }

        }
    }

    void OnDestroy()
    {
        CleanupSplines();
    }

    public void CleanupSplines()
    {
        if (Application.isPlaying) return;

        foreach (BezierSpline spline in Splines)
            if (spline != null) DestroyImmediate(spline.gameObject);
    }

    public void HandleSplines()
    {
        if (NextAnchor == null)
        {
            CleanupSplines();
        }
        else
        {
            foreach (BezierSpline spline in this.Splines)
            {
                if (spline == null) continue;
                spline.SplineDecorator.GenerateKnobs();
            }
        }

    }

}