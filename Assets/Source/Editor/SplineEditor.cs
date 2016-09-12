using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    private void OnSceneGUI()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

}
