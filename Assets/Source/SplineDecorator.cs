using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierSpline))]
public class SplineDecorator : MonoBehaviour
{
    private BezierSpline spline;
    public Transform knobTemplate;

    [SerializeField]
    private float distancePerKnob = 75f;

    private BezierSpline Spline
    {
        get
        {
            if (spline == null) spline = GetComponent<BezierSpline>();
            return spline;
        }
    }

    private float StepSize
    {
        get
        {
            return Spline.Length / distancePerKnob;
        }
    }

    private int PointsRequiredOnSpline
    {
        get
        {
            return Mathf.FloorToInt(StepSize);
        }
    }

    public Knob[] Knobs
    {
        get
        {
            return Spline.Knobs;
        }
    }

    public void GenerateKnobs()
    {
        if (Application.isPlaying) return;
        Cleanup();

        float stepSize = 1f / StepSize;

        for (int i = 1; i < PointsRequiredOnSpline-1; i++)
        {
            float stepNormalized = (i * stepSize);

            Transform itemSpawned = Instantiate(knobTemplate) as Transform;

            Vector3 position = spline.GetPoint(stepNormalized);

            itemSpawned.transform.localPosition = position;
            itemSpawned.transform.SetParent(spline.DecoratorContainer);
            itemSpawned.gameObject.name = "Knob"+i;
        }

    }

    private void Cleanup()
    {
        foreach ( Knob knob in Spline.DecoratorContainer.GetComponentsInChildren<Knob>())
            DestroyImmediate(knob.gameObject);
    }
}