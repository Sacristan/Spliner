using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineManager))]
public class SplineManagerEditor : Editor
{
    SplineManager t;
    SerializedObject GetTarget;

    void OnEnable()
    {
        t = (SplineManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying) return;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add"))
        {
            t.AddSpline();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < t.Splines.Count; i++)
        {
            BezierSpline spline = t.Splines[i];

            if (spline == null) continue;
            GUILayout.BeginHorizontal();
            GUILayout.Label(spline.gameObject.name);

            if (GUILayout.Button("X"))
                t.RemoveSpline(spline);

            GUILayout.EndHorizontal();
        }
    }
}