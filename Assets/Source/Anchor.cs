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
        get {

            return _nextAnchor;
        }
        set {
            _nextAnchor = value;

            if (_nextAnchor != null)
            {
                BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
                splines.Add(spline);
            }
        }
    }

    void OnGUI()
    {
    }

    void OnDestroy()
    {
        foreach (BezierSpline spline in splines)
            if(spline!=null) Destroy(spline.gameObject);
    }
}