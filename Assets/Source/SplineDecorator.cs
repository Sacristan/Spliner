using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierSpline))]
[ExecuteInEditMode]
public class SplineDecorator : MonoBehaviour
{
    private BezierSpline spline;
    public Transform itemToSpawn;

    private const float DISTANCE_PER_KNOB = 75f;
    private const float OFFET_FROM_BORDERS = 50f;

    float lastLength = 0f;

    bool inChange = false;

    float handleLengthChangeCallRateInSeconds = 0.5f;
    float handleLengthChangedLastCalled = 0f;

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
        get { return Spline.DecoratorContainer; }
    }

    void Awake()
    {
        GenerateKnobs();
    }


    //void HandleLengthChangeIfRequired()
    //{
    //    Debug.Log("HandleLengthChangeIfRequired");
    //    bool lengthChanged = lastLength != Spline.Length;

    //    if (lengthChanged)
    //    {
    //        inChange = true;
    //        lastLength = Spline.Length;
    //    }
    //    else
    //    {
    //        if (inChange)
    //            Generate();

    //        inChange = false;
    //    }
    //}

    public void GenerateKnobs()
    {
        if (Application.isPlaying) return;
        Cleanup();

        float stepSize = 1f / StepSize;
        //float offSetNormalized = 1f / OFFET_FROM_BORDERS;

        for (int i = 0; i < PointsRequiredOnSpline; i++)
        {
            float stepNormalized = (i * stepSize);
            //if (stepNormalized < offSetNormalized || stepNormalized >= (1 - offSetNormalized)) continue;

            Transform itemSpawned = Instantiate(itemToSpawn) as Transform;

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