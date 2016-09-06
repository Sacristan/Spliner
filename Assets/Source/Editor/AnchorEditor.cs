using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{
    Anchor targetAnchor;

    void OnEnable()
    {
        targetAnchor = (Anchor)target;
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

            anchor.HandleSplines();
        }

    }

    public override void OnInspectorGUI()
    {
        targetAnchor.HandleSplines();

        EditorGUI.BeginChangeCheck();

        Anchor nextAnchor = (Anchor)EditorGUILayout.ObjectField("Next Anchor: ", targetAnchor.NextAnchor, typeof(Anchor), true);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(targetAnchor);
            targetAnchor.NextAnchor = nextAnchor;
        }
    }
}