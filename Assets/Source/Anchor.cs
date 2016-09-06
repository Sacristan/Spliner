using UnityEngine;
using System.Collections.Generic;

public class Anchor : MonoBehaviour
{
    private static int width = 100;
    private static int height = 25;
    private static int xOffset = -50;
    private static int yOffset = -40;

    private static Anchor startAnchor;
    private static Anchor endAnchor;

    protected List<BezierSpline> splines = new List<BezierSpline>();

    void OnGUI()
    {
        if (!Application.isEditor) return;

        Vector2 pos = Camera.main.transform.InverseTransformPoint(transform.position);
        pos.y = Screen.height - pos.y - height;

        pos += new Vector2(xOffset, yOffset);

        Rect rect = new Rect(pos, new Vector2(width, height));

        if (startAnchor == null)
        {
            if (GUI.Button(rect, "Create Spline"))
            {
                StartSpline();
            }
        }
        else
        {
            if (startAnchor == this) return;
            if (GUI.Button(rect, "Finish Spline"))
            {
                FinishSpline ();
            }
        }
    }


    void OnDestroy()
    {
        foreach (BezierSpline spline in splines.ToArray())
            if(spline!=null) Destroy(spline.gameObject);
    }

    void StartSpline()
    {
        startAnchor = this;
    }

    void FinishSpline()
    {
        endAnchor = this;

        BezierSpline spline = SplineManager.AddSpline(startAnchor, endAnchor);

        startAnchor.splines.Add(spline);
        endAnchor.splines.Add(spline);

        startAnchor = null;
        endAnchor = null;
    }
}