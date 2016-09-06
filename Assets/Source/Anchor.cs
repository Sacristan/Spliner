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

            CleanupSplines();

            if (_nextAnchor != null)
            {
                BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
                splines.Add(spline);
            }

        }
    }

    void OnDestroy()
    {
        CleanupSplines();
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

    public void CleanupSplines()
    {
        Debug.Log("CleanupSplines called");

        if (Application.isPlaying) return;

        splines.RemoveAll(item => item == null);

        foreach (BezierSpline spline in Splines)
            DestroyImmediate(spline.gameObject);
    }

    public void HandleSplines()
    {
        foreach (BezierSpline spline in this.Splines)
        {
            if (spline == null) continue;
            spline.SplineDecorator.GenerateKnobs();
        }
    }

}