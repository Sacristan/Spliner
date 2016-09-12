using UnityEngine;
using UnityEditor;

namespace BeetrootLab.Features
{

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
}
