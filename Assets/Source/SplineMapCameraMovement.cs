using UnityEngine;
using System.Collections;
using BeetrootLab.Features;
using System;

public class SplineMapCameraMovement : MonoBehaviour
{

    private struct Bounds
    {
        private float minX;
        private float maxX;

        public Bounds(float min, float max)
        {
            minX = min;
            maxX = max;
        }

        //Screen.width* 0.005f
        public override string ToString()
        {
            return string.Format("Min X: {0} Max X: {1}", minX, maxX);
        }

        private float Offset
        {
            get { return Screen.width * 0.005f; }
        }

        public float MinXBoundaryInternal
        {
            get { return minX + Offset; }
        }

        public float MinXBoundaryExternal
        {
            get { return minX - Offset; }
        }

        public float MaxXBoundaryInternal
        {
            get { return maxX - Offset; }
        }

        public float MaxXBoundaryExternal
        {
            get { return maxX + Offset; }
        }

    }

    private Touch touch;

    [SerializeField]
    private float scrollSensitivity = 5f;

    [SerializeField]
    public float dampTime = 0.15f;

    [SerializeField]
    private float inertiaDurationInSeconds = 1f;

    [SerializeField]
    private float inertiaFactor = 0.05f;

    [SerializeField]
    private bool handleBounds = false;

    private readonly float scrollVelocityTreshold = 100f;

    private float timeTouchPhaseEnded;
    private float scrollVelocity;
    private Vector3 scrollDirection;

    private Camera _camera;
    private Vector3 velocity = Vector3.zero;

    private Vector2 touchPosLastFrame;

    Bounds bounds;

    #region Properties

    private float DampTime
    {
        get { return dampTime; }
    }

    private Vector2 AllowedScrollAxis
    {
        get { return Vector2.right; }
    }

    private Vector2 TouchPercentage
    {
        get
        {
            return GetPercentageVector(touch.position);
        }
    }

    private Vector2 PrevTouchPercentage
    {
        get
        {
            return GetPercentageVector(touchPosLastFrame);
        }
    }

    private Vector2 TouchDeltaPercentage
    {
        get
        {
            return TouchPercentage - PrevTouchPercentage;
        }
    }
    #endregion

    #region MonoBehaviour methods

    void Start()
    {
        _camera = GetComponent<Camera>();
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
                    scrollVelocity = 0.0f;
                    touchPosLastFrame = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    HandleMovement();
                    break;
                case TouchPhase.Ended:
                    timeTouchPhaseEnded = Time.time;
                    //Debug.Break();
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
            Vector2 swipeScrollMovement = GetSwipeScrollMovement();
            HandleMovementSmoothing(swipeScrollMovement);

            HandleBounds();
        }

        if (Input.touchCount > 0) touchPosLastFrame = touch.position;
    }

    #endregion

    #region Movement methods

    private void HandleMovement()
    {
        Vector3 calculatedPos = (Vector3)((touchPosLastFrame - touch.position) * _camera.orthographicSize / _camera.pixelHeight * 2f);
        Vector3 calculatedPosOnAxis = new Vector3(calculatedPos.x, 0, 0);

        Vector3 desiredPosition = transform.position + transform.TransformDirection(calculatedPosOnAxis);
        transform.position = ClampWithinExternalBounds(desiredPosition);

        CalculateScrollVelocity();
    }

    private void CalculateScrollVelocity()
    {
        Vector2 correctedDeltaPos = touch.deltaPosition * _camera.orthographicSize / _camera.pixelHeight * scrollVelocityTreshold * scrollSensitivity;

        scrollDirection = correctedDeltaPos.normalized;
        scrollVelocity = correctedDeltaPos.magnitude / touch.deltaTime;

        if (scrollVelocity <= scrollVelocityTreshold) scrollVelocity = 0;

        //Debug.Log("CalculateScrollVelocity: "+scrollVelocity);
    }

    private void HandleBounds()
    {
        if (!handleBounds) return;

        if (transform.position.x < bounds.MinXBoundaryInternal || transform.position.x > bounds.MaxXBoundaryInternal) scrollVelocity = 0f;

        Vector3 correctedDestination = new Vector3(
            Mathf.Clamp(transform.position.x, bounds.MinXBoundaryInternal, bounds.MaxXBoundaryInternal),
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(transform.position, correctedDestination, ref velocity, DampTime);
    }

    /// <summary>
    /// Handles Smooth movement towards desired movement destination
    /// </summary>
    /// 
    private void HandleMovementSmoothing(Vector2 deltaMovement)
    {
        if (deltaMovement != Vector2.zero)
        {
            Vector3 desiredMovementDestination = ClampWithinExternalBounds( transform.position + (Vector3)deltaMovement );

            if (Input.touchCount < 1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, desiredMovementDestination, ref velocity, DampTime);
            }
        }
    }

    /// <summary>
    /// Handles smoothed movement after swipe gesture ended
    /// </summary>
    private Vector2 GetSwipeScrollMovement()
    {
        Vector2 deltaMovement = Vector2.zero;

        //Debug.Log("GetSwipeScrollMovement scrollVelocity:" + scrollVelocity);

        if (Mathf.Abs(scrollVelocity) > 0f)
        {
            float t = (Time.time - timeTouchPhaseEnded) / inertiaDurationInSeconds;
            float frameVelocity = Mathf.Lerp(scrollVelocity, 0.0f, t);

            Vector3 deltaPos = (Vector3)scrollDirection.normalized * (frameVelocity * inertiaFactor) * Time.deltaTime * -1f;
            Vector3 allowedScrollAxis = AllowedScrollAxis;
            Vector3 deltaPosWithAppliedAxis = new Vector3(deltaPos.x * allowedScrollAxis.x, deltaPos.y * allowedScrollAxis.y, deltaPos.z * allowedScrollAxis.z);

            deltaMovement = deltaPosWithAppliedAxis;

            if (t >= 1.0f) scrollVelocity = 0.0f;
        }

        return deltaMovement;
    }
    #endregion

    private Vector3 ClampWithinExternalBounds(Vector3 desiredPosition)
    {
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, bounds.MinXBoundaryExternal, bounds.MaxXBoundaryExternal);
        return desiredPosition;
    }

    private Vector2 GetPercentageVector(Vector2 vector)
    {
        return new Vector2(
            vector.x / Screen.width,
            vector.y / Screen.height
        );
    }

    private void CalculateBounds()
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        Anchor anchorLeft = null;
        Anchor anchorRight = null;

        foreach (Anchor anchor in anchors)
        {
            if (anchorLeft == null || anchorRight == null)
            {
                anchorLeft = anchor;
                anchorRight = anchor;
                continue;
            }

            float posXAnchor = anchor.transform.position.x;

            float posXL = anchorLeft.transform.position.x;
            float posXR = anchorRight.transform.position.x;

            if (posXAnchor > posXR) anchorRight = anchor;
            if (posXAnchor < posXL) anchorLeft = anchor;
        }

        bounds = new Bounds(anchorLeft.transform.position.x, anchorRight.transform.position.x);

        Debug.Log("Calculated bounds: " + bounds);
    }
}