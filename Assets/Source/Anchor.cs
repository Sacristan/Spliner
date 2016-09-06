using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [SerializeField]
    List<BezierSpline> incomingSplines = new List<BezierSpline>();

    [SerializeField]
    List<BezierSpline> outgoingSplines = new List<BezierSpline>();


    [SerializeField]
    private Anchor _nextAnchor;


    public BezierSpline[] IncomingSplines
    {
        get
        {
            return incomingSplines.ToArray();
        }
    }

    public BezierSpline[] OutgoingSplines
    {
        get
        {
            return outgoingSplines.ToArray();
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
            CleanupSplines();

            if (value == this) return;
            _nextAnchor = value;

            if (_nextAnchor != null)
            {
                BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
                outgoingSplines.Add(spline);
                _nextAnchor.AddIncomingSpline(spline);
            }
        }
    }

    void OnDestroy()
    {
        CleanupSplines(true);
    }

    void OnGUI()
    {
        if (Application.isPlaying) return;
        int height = 25;
        int width = 100;

        if (NextAnchor == null)
        {
            Vector2 pos = Camera.main.transform.InverseTransformPoint(transform.position);
            pos.x += width * -0.5f;
            pos.y = Screen.height - pos.y - height * 2.2f;
            Rect rect = new Rect(pos, new Vector2(width, height));
            GUI.Label(rect, "No End Anchor!");
        }
    }

    public void AddIncomingSpline(BezierSpline spline)
    {
        incomingSplines.Add(spline);
    }

    public void CleanupSplines(bool cleanIncoming=false)
    {
        Debug.Log("CleanupSplines called");

        if (Application.isPlaying) return;

        RemoveRenundantSplinesFromArrays();

        foreach (BezierSpline spline in OutgoingSplines)
            DestroyImmediate(spline.gameObject);

        if (cleanIncoming)
        {
            foreach (BezierSpline spline in IncomingSplines)
                DestroyImmediate(spline.gameObject);
        }

        RemoveRenundantSplinesFromArrays();
    }

    public void HandleSplines()
    {
        foreach (BezierSpline spline in this.OutgoingSplines)
        {
            if (spline == null) continue;
            spline.SplineDecorator.GenerateKnobs();
        }
    }

    private void RemoveRenundantSplinesFromArrays()
    {
        outgoingSplines.RemoveAll(item => item == null);
        incomingSplines.RemoveAll(item => item == null);
    }

}