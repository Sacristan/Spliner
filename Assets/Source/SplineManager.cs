using UnityEngine;
using System.Collections.Generic;

public class SplineManager : MonoBehaviour
{
    [SerializeField]
    private List<BezierSpline> splines = new List<BezierSpline>();
    public List<BezierSpline> Splines { get { return splines; } }

    public BezierSpline splineTemplate;

    public BezierSpline AddSpline()
    {
        GameObject splineGO = Instantiate(splineTemplate.gameObject) as GameObject;
        BezierSpline spline = splineGO.GetComponent<BezierSpline>();
        splines.Add(spline);

        spline.Init(this);
        spline.transform.SetParent(transform);
        spline.gameObject.name = "Spline_" + System.Guid.NewGuid();
        return spline;
    }

    public void RemoveSpline(BezierSpline spline)
    {
        splines.Remove(spline);
        Destroy(spline.gameObject);
    }

    public BezierSpline SplineAtIndex(int index)
    {
        BezierSpline spline = null;
        if (index >= 0 && Splines.Count > index) spline = Splines[index];
        return spline;
    }

    public int IndexForSpline(BezierSpline spline)
    {
        int result = -1;
        result = Splines.IndexOf(spline);
        return result;
    }

}