using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{
    Anchor targetAnchor;
    SerializedObject SerializedTargetAnchor;
    SerializedProperty NextAnchorProperty;

    void OnEnable()
    {
        targetAnchor = (Anchor)target;
        SerializedTargetAnchor = new SerializedObject(targetAnchor);
        NextAnchorProperty = SerializedTargetAnchor.FindProperty("_nextAnchor");
    }

    private void OnSceneGUI()
    {
        Anchor[] anchors = FindObjectsOfType(typeof(Anchor)) as Anchor[];

        for (int i = 0; i < anchors.Length; i++)
        {
            Anchor anchor = anchors[i];

            if (anchor.NextAnchor != null)
            {
                Handles.DrawLine(anchor.transform.position, anchor.NextAnchor.transform.position);
            }

            anchor.AddSplinesIfRequired();
            anchor.DecorateOutgoingSplines();
        }

    }

    public override void OnInspectorGUI()
    {
        targetAnchor.DecorateOutgoingSplines();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(NextAnchorProperty);

        if (EditorGUI.EndChangeCheck())
        {
            SerializedTargetAnchor.ApplyModifiedProperties();
            targetAnchor.CleanupAndAddSplinesIfRequired();
        }

    }
}