using UnityEngine;
using System.Collections;

public class SplineMapCameraMovement : MonoBehaviour
{
    private struct Bounds
    {
       public Vector2 min;
       public Vector2 max;

       public override string ToString()
        {
            return string.Format("Min: {0} Max: {1}", min, max);
        }
    }

    private enum PinnedAxis { X, Y, Z, XY, XZ, YZ, XYZ }

    private Camera _camera;
    private Touch touch;

    [SerializeField]
    private float sensitivity = 2.5f;

    [SerializeField]
    private bool invertAxis = false;

    [SerializeField]
    public float dampTime = 0.15f;

    [SerializeField]
    private float inertiaDurationInSeconds = 1f;

    [SerializeField]
    private float inertiaFactor = 0.05f;

    [SerializeField]
    private PinnedAxis pinnedAxis = PinnedAxis.X;

    [SerializeField]
    private bool handleBounds = false;

    private readonly float scrollVelocityTreshold = 100f;

    private float timeTouchPhaseEnded;
    private float scrollVelocity;
    private Vector3 scrollDirection;

    Bounds bounds;

    private Vector3 boundTo;

    private Vector3 velocity = Vector3.zero;

    private Vector3 desiredMovementDestination;

    #region MonoBehaviour methods

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        CalculateBounds();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    desiredMovementDestination = Vector3.zero;
                    scrollVelocity = 0.0f;
                    break;
                case TouchPhase.Moved:
                    HandleMovement();
                    break;
                case TouchPhase.Ended:
                    timeTouchPhaseEnded = Time.time;
                    break;
                case TouchPhase.Canceled:
                    break;
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
            }
        }
        else
        {
            HandleScrollMovement();
        }


        HandleSmoothMovement();
    }

    #endregion

    #region Movement methods

    /// <summary>
    /// Handles Smooth movement towards desired movement destination
    /// </summary>
    private void HandleSmoothMovement()
    {

        if (desiredMovementDestination != Vector3.zero)
        {
            Vector3 correctedDestination = desiredMovementDestination;
            if (handleBounds)
            {
                correctedDestination = new Vector3(
                    Mathf.Clamp(desiredMovementDestination.x, bounds.min.x, bounds.max.x),
                    desiredMovementDestination.y,
                    desiredMovementDestination.z
                );
            }

            //Debug.Log(string.Format("Bounds: {0} movementMovementDestination: {1} correctedPos: {2}", bounds, desiredMovementDestination, correctedDestination));
            transform.position = Vector3.SmoothDamp(transform.position, correctedDestination, ref velocity, dampTime);
        }
    }

    /// <summary>
    /// Handles basic movement
    /// </summary>
    private void HandleMovement()
    {
        desiredMovementDestination = transform.position + MovementDelta;
        //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        scrollDirection = touch.deltaPosition.normalized;
        scrollVelocity = touch.deltaPosition.magnitude / touch.deltaTime;

        if (scrollVelocity <= scrollVelocityTreshold) scrollVelocity = 0;
    }

    /// <summary>
    /// Handles smoothed movement after swipe gesture ended
    /// </summary>
    private void HandleScrollMovement()
    {
        if (Mathf.Abs(scrollVelocity) > 0f)
        {
            float t = (Time.time - timeTouchPhaseEnded) / inertiaDurationInSeconds;
            float frameVelocity = Mathf.Lerp(scrollVelocity, 0.0f, t);

            Vector3 deltaPos = (Vector3)scrollDirection.normalized * (frameVelocity * inertiaFactor) * Time.deltaTime * -1f;
            Vector3 allowedScrollAxis = AllowedScrollAxis;
            Vector3 deltaPosWithAppliedAxis = new Vector3(deltaPos.x * allowedScrollAxis.x, deltaPos.y * allowedScrollAxis.y, deltaPos.z * allowedScrollAxis.z);

            //transform.Translate(deltaPosWithAppliedAxis);
            desiredMovementDestination = transform.position + deltaPosWithAppliedAxis;

            if (t >= 1.0f) scrollVelocity = 0.0f;
        }
    }
    #endregion

    private Vector3 AllowedScrollAxis
    {
        get
        {
            Vector3 result = Vector3.zero;

            switch (pinnedAxis)
            {
                case PinnedAxis.X:
                    result = Vector3.right;
                    break;
                case PinnedAxis.Y:
                    result = Vector3.up;
                    break;
                case PinnedAxis.Z:
                    result = Vector3.forward;
                    break;
                case PinnedAxis.XY:
                    result = new Vector3(1, 1, 0);
                    break;
                case PinnedAxis.XZ:
                    result = new Vector3(1, 0, 1);
                    break;
                case PinnedAxis.YZ:
                    result = new Vector3(0, 1, 1);
                    break;
                case PinnedAxis.XYZ:
                    result = Vector3.one;
                    break;
            }

            return result;
        }
    }

    private Vector3 MovementDelta
    {
        get
        {
            Vector3 delta = touch.deltaPosition;

            float positionX = PosFromDelta(delta.x);
            float positionY = PosFromDelta(delta.y);
            float positionZ = PosFromDelta(delta.z);

            Vector3 allowedScrollAxis = AllowedScrollAxis;

            return new Vector3(positionX * allowedScrollAxis.x, positionY * allowedScrollAxis.y, positionZ * allowedScrollAxis.z);
        }
    }

    private float PosFromDelta(float delta)
    {
        float pos = delta * sensitivity * Time.deltaTime;
        return invertAxis ? pos : pos * -1;
    }

    private void CalculateBounds()
    {
        //int mapX = 1600;
        //int mapY = 600;

        //float verticalExtent = _camera.orthographicSize;
        //float horizontalExtent = _camera.orthographicSize * Screen.width / Screen.height;

        //bounds.min = new Vector2(horizontalExtent - mapX / 2.0f, verticalExtent - mapY / 2.0f);
        //bounds.max = new Vector2(mapX / 2.0f - horizontalExtent, mapY / 2.0f - verticalExtent);

        bounds.min.x = 709;
        bounds.max.x = 713;

        Debug.Log("Recalculated bounds: "+bounds);
    }
}