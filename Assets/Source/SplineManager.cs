using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SplineManager : MonoBehaviour
{
    private static SplineManager singletone;

    [SerializeField]
    private List<BezierSpline> splines = new List<BezierSpline>();
    public BezierSpline splineTemplate;

    public List<BezierSpline> Splines { get { return splines; } }

    [SerializeField]
    private Transform _splineContainer;

    private Transform SplineContainer
    {
        get
        {
            if (_splineContainer == null)
            {
                _splineContainer = new GameObject("Splines").transform;
                _splineContainer.SetParent(transform);
            }
            return _splineContainer;
        }
    }

    private static SplineManager Singletone {
        get
        {
            if(singletone == null)
            {
                singletone = FindObjectOfType<SplineManager>();
            }
            return singletone;
        }
     }

    public static BezierSpline AddSpline(Anchor startAnchor, Anchor endAnchor)
    {
        CleanupNullSplinesInList();

        GameObject splineGO = Instantiate(Singletone.splineTemplate.gameObject) as GameObject;
        BezierSpline spline = splineGO.GetComponent<BezierSpline>();

        spline.Init(startAnchor, endAnchor);

        Singletone.splines.Add(spline);
        spline.SplineManager = Singletone;
        spline.transform.SetParent(Singletone.SplineContainer.transform);
        spline.gameObject.name = "Spline_" + System.Guid.NewGuid();

        return spline;
    }


    public static void CleanupIncomingSplinesForAnchor(Anchor anchor)
    {
        CleanupNullSplinesInList();

        foreach (BezierSpline spline in Singletone.splines.ToArray())
        {
            if (spline.EndAnchor == anchor)
            {
                spline.MarkForDestruction();
                Singletone.splines.RemoveAll(item=> item == spline);
            }
        }

    }

    public static void CleanupOutgoingSplinesForAnchor(Anchor anchor)
    {
        CleanupNullSplinesInList();

        foreach (BezierSpline spline in Singletone.splines.ToArray())
        {
            if (spline.StartAnchor == anchor)
            {
                spline.MarkForDestruction();
                Singletone.splines.RemoveAll(item => item == spline);
            }
        }

    }

    private static void CleanupNullSplinesInList()
    {
        Singletone.splines.RemoveAll(item => item == null);
    }

}