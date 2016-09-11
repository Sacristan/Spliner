using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Spline))]
public class SplineDecorator : MonoBehaviour
{
    private Spline spline;
    public Transform knobTemplate;

    [SerializeField]
    private float distancePerKnob = 75f;

    private Spline Spline
    {
        get
        {
            if (spline == null) spline = GetComponent<Spline>();
            return spline;
        }
    }

    private float StepSize
    {
        get
        {
            return 0f;
            //return Spline.Length / distancePerKnob;
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

    //TODO: FIX ME
    public void GenerateKnobs()
    {
        //Cleanup();

        //float stepSize = 1f / StepSize;

        //for (int i = 1; i < PointsRequiredOnSpline - 1; i++)
        //{
        //    float stepNormalized = (i * stepSize);

        //    Transform itemSpawned = Instantiate(knobTemplate) as Transform;

        //    Vector3 position = spline.Spline.GetPoint(stepNormalized);

        //    itemSpawned.transform.localPosition = position;
        //    itemSpawned.transform.SetParent(spline.DecoratorContainer);
        //    itemSpawned.gameObject.name = "Knob" + i;
        //}
    }

    private void Cleanup()
    {
        foreach ( Knob knob in Spline.DecoratorContainer.GetComponentsInChildren<Knob>())
            DestroyImmediate(knob.gameObject);
    }
}