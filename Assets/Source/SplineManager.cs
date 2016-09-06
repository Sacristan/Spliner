using UnityEngine;
using System.Collections.Generic;

public class SplineManager : MonoBehaviour
{
    private static SplineManager singletone;

    [SerializeField]
    private List<BezierSpline> splines = new List<BezierSpline>();
    public BezierSpline splineTemplate;

    public List<BezierSpline> Splines { get { return splines; } }

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

    void Awake()
    {
        if (singletone == null) singletone = this;
        else Destroy(this);
    }

    public static BezierSpline AddSpline(Anchor startAnchor, Anchor endAnchor)
    {
        GameObject splineGO = Instantiate(singletone.splineTemplate.gameObject) as GameObject;
        BezierSpline spline = splineGO.GetComponent<BezierSpline>();
        singletone.splines.Add(spline);

        spline.Init(startAnchor, endAnchor);
        spline.transform.SetParent(singletone.SplineContainer.transform);
        spline.gameObject.name = "Spline_" + System.Guid.NewGuid();
        return spline;
    }

}