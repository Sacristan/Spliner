using UnityEngine;
using UnityEditor;

namespace BeetrootLab.Features
{
    [CustomEditor(typeof(SplineMap))]
    public class SplineMapEditor : Editor
    {
        SplineMap t;
        SerializedObject GetTarget;

        void OnEnable()
        {
            t = (SplineMap)target;
            GetTarget = new SerializedObject(t);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GetTarget.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Add Anchor"))
            {
                AddAnchor();
            }
        }

        private void AddAnchor()
        {
            Anchor[] anchors = GameObject.FindObjectsOfType<Anchor>();

            GameObject anchorGo = new GameObject("Anchor" + anchors.Length, typeof(Anchor));
            Anchor anchor = anchorGo.GetComponent<Anchor>();
            AnchorSyncer.Sync(anchor);

            anchorGo.transform.SetParent(AnchorsContainer);
            anchorGo.transform.localPosition = Vector3.zero;

            GameObject spawnedVisualisation = PrefabUtility.InstantiatePrefab(t.AnchorVisualisation) as GameObject;
            spawnedVisualisation.transform.SetParent(anchorGo.transform);
            spawnedVisualisation.transform.localPosition = Vector3.zero;
        }


        private Transform AnchorsContainer
        {
            get
            {
                string containerName = "Anchors";
                Transform anchorsContainer = t.transform.Find(containerName);

                if (anchorsContainer == null)
                {
                    anchorsContainer = new GameObject(containerName).transform;
                    anchorsContainer.SetParent(t.transform);
                    anchorsContainer.transform.localPosition = Vector3.zero;
                }

                return anchorsContainer;
            }
        }
    }
}
