using UnityEngine;
using UnityEditor;

namespace BeetrootLab.Features
{
    public class SplineDecorator
    {
        /// <summary>
        /// Cleans up knobs, generates and assigns them to spline. Accepts only bezier splines
        /// </summary>
        /// <param name="bezierSpline"></param>
        public static void Decorate(BezierSpline bezierSpline)
        {
            if (bezierSpline == null || bezierSpline.Spline == null) return;
            Cleanup(bezierSpline);
            GenerateKnobs(bezierSpline);
            FetchKnobsForSpline(bezierSpline.Spline);
        }

        /// <summary>
        /// Destroys all Spline knobs
        /// </summary>
        /// <param name="bezierSpline"></param>
        private static void Cleanup(BezierSpline bezierSpline)
        {
            foreach (Knob knob in bezierSpline.Spline.Knobs)
                Object.DestroyImmediate(knob.gameObject);
        }

        /// <summary>
        /// Creates knobs for spline
        /// </summary>
        /// <param name="bezierSpline"></param>
        private static void GenerateKnobs(BezierSpline bezierSpline)
        {
            float stepSize = bezierSpline.Length / DistancePerKnob;
            int pointsRequiredOnSpline = Mathf.FloorToInt(stepSize);

            float stepSizeF = 1f / stepSize;

            for (int i = 1; i < pointsRequiredOnSpline - 1; i++)
            {
                float stepNormalized = (i * stepSizeF);

                GameObject spawnedKnob = new GameObject("Knob", typeof(Knob));
                Knob knob = spawnedKnob.GetComponent<Knob>();

                Vector3 position = bezierSpline.GetPoint(stepNormalized);

                spawnedKnob.transform.localPosition = position;
                spawnedKnob.transform.SetParent(DecoratorContainer(bezierSpline.Spline));
                spawnedKnob.name = "Knob" + i;
                knob.Spline = bezierSpline.Spline;

                GameObject spawnedVisualisation = PrefabUtility.InstantiatePrefab(SplineMap.KnobVisualisation) as GameObject;
                spawnedVisualisation.transform.SetParent(spawnedKnob.transform);
                spawnedVisualisation.transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Gets (and creates if required) decorator container object for spline
        /// </summary>
        /// <param name="spline"></param>
        /// <returns></returns>
        private static Transform DecoratorContainer(Spline spline)
        {
            Transform splineDecorators = spline.transform.Find("SplineDecorators");

            if (splineDecorators == null)
            {
                splineDecorators = new GameObject("SplineDecorators").transform;
                splineDecorators.SetParent(spline.transform);
            }

            return splineDecorators;
        }

        private static SplineMap SplineMap
        {
            get { return GameObject.FindObjectOfType<SplineMap>(); }
        }

        private static void FetchKnobsForSpline(Spline spline)
        {
            Knob[] knobs = DecoratorContainer(spline).GetComponentsInChildren<Knob>();
            spline.Knobs = knobs;
        }

        private static float DistancePerKnob
        {
            get { return 0.75f; }
        }

    }
}