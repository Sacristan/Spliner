using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierSpline))]
public class SplineDecorator : MonoBehaviour
{
    private BezierSpline spline;
    public Transform itemToSpawn;

    private const float DISTANCE_PER_KNOB = 75f;
    private const float OFFET_FROM_BORDERS = 50f;

    float lastLength = 0f;

    bool inChange = false;

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
            return Spline.Length / DISTANCE_PER_KNOB;
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
        get { return spline.DecoratorContainer; }
    }

    private Transform[] Children
    {
        get
        {
            return DecoratorContainer.GetComponentsInChildren<Transform>();
        }
    }

    private void Awake()
    {
        InvokeRepeating("HandleLengthChangeIfRequired", 0f, 0.5f);
    }

    void HandleLengthChangeIfRequired()
    {
        bool lengthChanged = lastLength != Spline.Length;

        if (lengthChanged)
        {
            inChange = true;
            lastLength = Spline.Length;
        }
        else
        {
            if (inChange)
                HandleLengthChange();

            inChange = false;
        }
    }

    private void HandleLengthChange()
    {
        Cleanup();
        Generate();
    }

    private void Cleanup()
    {
        foreach (Transform child in Children)
        {
            if (child == DecoratorContainer) continue;
            Destroy(child.gameObject);
        }
    }

    private void Generate()
    {
        float stepSize = 1f / StepSize;
        float offSetNormalized = 1f / OFFET_FROM_BORDERS;

        for (int i = 0; i < PointsRequiredOnSpline; i++)
        {
            Transform itemSpawned = Instantiate(itemToSpawn) as Transform;

            float stepNormalized = (i * stepSize);

            if (stepNormalized < offSetNormalized || stepNormalized > (1 - offSetNormalized)) continue;

            Vector3 position = spline.GetPoint(stepNormalized);

            itemSpawned.transform.localPosition = position;
            itemSpawned.transform.SetParent(spline.DecoratorContainer);
        }

    }

}