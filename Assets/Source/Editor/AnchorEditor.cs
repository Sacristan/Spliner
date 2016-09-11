using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{
    private Anchor targetAnchor;

    private bool editorCalled = false;

    void OnEnable()
    {
        targetAnchor = (Anchor)target;
    }

    private void OnSceneGUI()
    {
        if (editorCalled)
        {
            Anchor[] anchors = FindObjectsOfType(typeof(Anchor)) as Anchor[];

            for (int i = 0; i < anchors.Length; i++)
            {
                Anchor anchor = anchors[i];

                foreach (Spline spline in anchor.OutgoingSplines.ToArray())
                {
                    if (spline == null) continue;
                    DrawHandlesAndBezierSpline(spline);
                }

                foreach (Anchor outgoingAnchor in anchor.OutgoingAnchors)
                {
                    if (outgoingAnchor != null) Debug.DrawLine(anchor.transform.position, outgoingAnchor.transform.position, Color.cyan);
                }

            }
        }
        else
        {
            editorCalled = true;
        }

    }

    public override void OnInspectorGUI()
    {
        if (!editorCalled) return;
        DrawDefaultInspector();

        if (GUI.changed)
        {
            AnchorSyncer.Sync(targetAnchor);
        }
    }

    private void DrawHandlesAndBezierSpline(Spline spline)
    {
        if (spline == null) return;

        BezierSpline bezierSpline = new BezierSpline(spline);

        Vector3 p0 = ShowPoint(0, bezierSpline);



        for (int i = 1; i < bezierSpline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i, bezierSpline);
            Vector3 p2 = ShowPoint(i + 1, bezierSpline);
            Vector3 p3 = ShowPoint(i + 2, bezierSpline);

            Color prevColor = Handles.color;

            Handles.color = Color.blue;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.color = prevColor;

            Handles.DrawBezier(p0, p3, p1, p2, Color.green, null, 2f);

            Debug.Log(string.Format("Points: {0} {1} {2} {3} / Spline: {4} {5} {6} {7}", p0, p1, p2, p3, spline.P0, spline.P1, spline.P2, spline.P3));

            //p0 = p3;
        }

        SplineDecorator.Decorate(bezierSpline);
    }

    private Vector3 ShowPoint(int index, BezierSpline spline)
    {
        Transform handleTransform = spline.Spline.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);

        if (index == 0)
            size *= 2f;

        EditorGUI.BeginChangeCheck();

        point = Handles.FreeMoveHandle(point, handleRotation, 10f, Vector3.zero, Handles.RectangleCap);

        if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(spline.Spline, "Move Point");
            //EditorUtility.SetDirty(spline.Spline);
            spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
        }
        return point;
    }

}