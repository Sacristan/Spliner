using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [SerializeField]
    BezierSpline[] splines;

    [SerializeField]
    private Anchor _nextAnchor;

    public Anchor NextAnchor
    {
        get {

            return _nextAnchor;
        }
        set {
            _nextAnchor = value;

            if (_nextAnchor != null)
            {
                SplineManager.AddSpline(this, _nextAnchor);
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