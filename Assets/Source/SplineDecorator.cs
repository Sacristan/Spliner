using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierSpline))]
[ExecuteInEditMode]
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

    private Transform DecoratorContainer
    {
        get { return Spline.DecoratorContainer; }
    }

    //void Awake()
    //{
    //    GenerateKnobs();
    //}

    public void GenerateKnobs()
    {
        if (Application.isPlaying) return;
        Cleanup();

        float stepSize = 1f / StepSize;

        for (int i = 0; i < PointsRequiredOnSpline; i++)
        {
            float stepNormalized = (i * stepSize);

            Transform itemSpawned = Instantiate(knobTemplate) as Transform;

            Vector3 position = spline.GetPoint(stepNormalized);

            itemSpawned.transform.localPosition = position;
            itemSpawned.transform.SetParent(spline.DecoratorContainer);
        }

    }

    private void Cleanup()
    {
        foreach (Transform child in DecoratorContainer.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

}