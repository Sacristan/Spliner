using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{
    Anchor targetAnchor;

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

                foreach (BezierSpline spline in anchor.OutgoingSplines.ToArray())
                {
                    if (spline == null) continue;

                    spline.SetControlPoints();
                    DrawHandlesAndBezierSpline(spline);
                }

                foreach (Anchor outgoingAnchor in anchor.OutgoingAnchors)
                {
                    if (outgoingAnchor != null) Debug.DrawLine(anchor.transform.position, outgoingAnchor.transform.position, Color.cyan);
                }

                anchor.RepopulateKnobs();

            }
        }
        else
        {
            editorCalled = true;
        }

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            //Debug.Log("GUI changed");
            targetAnchor.SyncAnchors();
        }
    }

    private void DrawHandlesAndBezierSpline(BezierSpline spline)
    {
        if (spline == null) return;
        Vector3 p0 = ShowPoint(0, spline);

        //Debug.Log(spline.ControlPointCount);

        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i, spline);
            Vector3 p2 = ShowPoint(i + 1, spline);
            Vector3 p3 = ShowPoint(i + 2, spline);

            Color prevColor = Handles.color;

            Handles.color = Color.blue;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.color = prevColor;

            Handles.DrawBezier(p0, p3, p1, p2, Color.green, null, 2f);

            p0 = p3;
        }
        spline.Decorate();
    }

    private Vector3 ShowPoint(int index, BezierSpline spline)
    {
        Transform handleTransform = spline.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0)
        {
            size *= 2f;
        }

        EditorGUI.BeginChangeCheck();
        point = Handles.FreeMoveHandle(point, handleRotation, 10f, Vector3.zero, Handles.RectangleCap);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
        }
        return point;
    }



}