using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnchorManager))]
public class AnchorManagerEditor : Editor
{
    AnchorManager t;
    SerializedObject GetTarget;

    void OnEnable()
    {
        t = (AnchorManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying) return;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Node"))
        {
            t.AddAnchor();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < t.Anchors.Count; i++)
        {
            Anchor anchor = t.Anchors[i];
            if (anchor == null) continue;
            GUILayout.BeginHorizontal();
            GUILayout.Label(anchor.gameObject.name);

            if (GUILayout.Button("X"))
                t.RemoveAnchor(anchor);

            GUILayout.EndHorizontal();
        }
    }
}