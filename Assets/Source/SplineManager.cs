using UnityEngine;
using System.Collections.Generic;

public class SplineManager : MonoBehaviour
{
    [SerializeField]
    private List<BezierSpline> splines = new List<BezierSpline>();
    public List<BezierSpline> Splines { get { return splines; } }

    public BezierSpline splineTemplate;
    public GameObject anchorTemplate;

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
        //int index = IndexForSpline(spline);

        //BezierSpline prevSpline = SplineAtIndex(index - 1);
        //BezierSpline nextSpline = SplineAtIndex(index + 1);

        //if (prevSpline != null) prevSpline.IsDirty = true;
        //if (nextSpline != null) nextSpline.IsDirty = true;

        //if(prevSpline == null)
        //{
        //    if(nextSpline!=null) nextSpline.CreateStartAnchor();
        //}
        //else
        //{
        //    prevSpline.CreateEndAnchor();
        //    if (nextSpline != null) nextSpline.TakeStartAnchorFromSplineEndAnchor(prevSpline);
        //}

        //splines.Remove(spline);
        //Destroy(spline.gameObject);

        //if (prevSpline != null) prevSpline.IsDirty = false;
        //if (nextSpline != null) nextSpline.IsDirty = false;
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