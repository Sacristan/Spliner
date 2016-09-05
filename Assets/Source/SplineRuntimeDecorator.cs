using UnityEngine;

public class SplineRuntimeDecorator : MonoBehaviour
{
    private BezierSpline spline;
    public Transform itemToSpawn;

    private const float DISTANCE_PER_KNOB = 75f;

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

    private void Awake()
    {
        Generate();
    }

    void Update()
    {
        Debug.Log("Spline length: "+ Spline.Length);
    }

    private void Generate()
    {
        float stepSize = 1f / StepSize;

        for(int i =0; i < PointsRequiredOnSpline; i++)
        {
            Transform itemSpawned = Instantiate(itemToSpawn) as Transform;


            float normalStep = (i * stepSize);

            Debug.Log(normalStep);

            Vector3 position = spline.GetPoint(normalStep);

            itemSpawned.transform.localPosition = position;
            itemSpawned.transform.SetParent(transform);
        }

    }

}