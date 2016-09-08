using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor
{

    private void OnSceneGUI()
    {
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }


}