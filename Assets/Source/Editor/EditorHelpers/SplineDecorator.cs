using UnityEngine;
using System.Collections;

public class SplineDecorator
{

    public static void Decorate(BezierSpline bezierSpline)
    {
        if (bezierSpline == null || bezierSpline.Spline == null) return;
        Cleanup(bezierSpline);
        GenerateKnobs(bezierSpline);
        FetchKnobsForSpline(bezierSpline.Spline);
    }

    private static void Cleanup(BezierSpline bezierSpline)
    {
        foreach (Knob knob in bezierSpline.Spline.Knobs)
            Object.DestroyImmediate(knob.gameObject);
    }

    private static void GenerateKnobs(BezierSpline bezierSpline)
    {
        float stepSize = bezierSpline.Length / DistancePerKnob;
        int pointsRequiredOnSpline = Mathf.FloorToInt(stepSize);

        float stepSizeF = 1f / stepSize;

        for (int i = 1 ; i < pointsRequiredOnSpline-1; i++)
        {
            float stepNormalized = (i * stepSizeF);

            GameObject spawnedKnob = new GameObject("Knob", typeof(Knob));
            Knob knob = spawnedKnob.GetComponent<Knob>();

            Vector3 position = bezierSpline.GetPoint(stepNormalized);

            spawnedKnob.transform.localPosition = position;
            spawnedKnob.transform.SetParent(DecoratorContainer(bezierSpline.Spline));
            spawnedKnob.name = "Knob" + i;
            knob.Spline = bezierSpline.Spline;
        }
    }

    private static Transform DecoratorContainer(Spline spline)
    {
        Transform splineDecorators = spline.transform.Find("SplineDecorators");

        if(splineDecorators == null)
        {
            splineDecorators = new GameObject("SplineDecorators").transform;
            splineDecorators.SetParent(spline.transform);
        }

        return splineDecorators;
    }

    private static float DistancePerKnob
    {
        get { return 0.75f; }
    }

    private static void FetchKnobsForSpline(Spline spline)
    {
        Knob[] knobs = DecoratorContainer(spline).GetComponentsInChildren<Knob>();
        spline.Knobs = knobs;
    }

}