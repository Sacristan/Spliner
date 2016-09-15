using UnityEngine;
using System.Collections;
using BeetrootLab.Features;

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

    private Touch touch;

    [SerializeField]
    private float sensitivity = 3.5f;

    [SerializeField]
    private bool invertAxis = false;

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

    Bounds bounds;

    //private Camera _camera;
    private Vector3 velocity = Vector3.zero;

    private Vector2 touchPosLastFrame;

    #region MonoBehaviour methods

    void Start()
    {
        //_camera = GetComponent<Camera>();
        CalculateBounds();
    }

    void Update()
    {
        Vector2 deltaMovement = Vector2.zero;

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
                    deltaMovement = GetSwipeMovement();
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
            deltaMovement = GetSwipeScrollMovement();
        }

        HandleSmoothMovement(deltaMovement);
        touchPosLastFrame = touch.position;
    }

    #endregion

    #region Movement methods

    /// <summary>
    /// Handles Smooth movement towards desired movement destination
    /// </summary>
    private void HandleSmoothMovement(Vector2 deltaMovement)
    {
        if (deltaMovement != Vector2.zero)
        {
            Vector3 desiredMovementDestination = transform.position + (Vector3) deltaMovement;

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = desiredMovementDestination;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, desiredMovementDestination, ref velocity, dampTime);

                if (handleBounds)
                {
                    float offset = Screen.width * 0.005f;

                    Vector3 correctedDestination = new Vector3(
                        Mathf.Clamp(desiredMovementDestination.x, bounds.min.x + offset, bounds.max.x - offset),
                        desiredMovementDestination.y,
                        desiredMovementDestination.z
                    );

                    transform.position = Vector3.SmoothDamp(transform.position, correctedDestination, ref velocity, dampTime);
                }
            }
        }
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

    private Vector2 GetPercentageVector(Vector2 vector)
    {
        return new Vector2(
            vector.x / Screen.width,
            vector.y / Screen.height
        );
    }

    private Vector2 TouchDeltaPercentage
    {
        get
        {
            return TouchPercentage - PrevTouchPercentage; 
        }
    }

    /// <summary>
    /// Handles basic movement
    /// </summary>
    private Vector2 GetSwipeMovement()
    {
        scrollDirection = touch.deltaPosition.normalized;
        scrollVelocity = touch.deltaPosition.magnitude / touch.deltaTime;

        if (scrollVelocity <= scrollVelocityTreshold) scrollVelocity = 0;

        return MovementDelta;
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

            Vector3 deltaPos = (Vector3) scrollDirection.normalized * (frameVelocity * inertiaFactor) * Time.deltaTime * -1f;
            Vector3 allowedScrollAxis = AllowedScrollAxis;
            Vector3 deltaPosWithAppliedAxis = new Vector3(deltaPos.x * allowedScrollAxis.x, deltaPos.y * allowedScrollAxis.y, deltaPos.z * allowedScrollAxis.z);

            deltaMovement = deltaPosWithAppliedAxis;

            if (t >= 1.0f) scrollVelocity = 0.0f;
        }


        return deltaMovement;
    }
    #endregion

    private Vector2 AllowedScrollAxis
    {
        get { return Vector2.right; }
    }

    private Vector2 MovementDelta
    {
        get
        {
            Vector3 delta = TouchDeltaPercentage;

            float positionX = PosFromDelta(delta.x);
            float positionY = PosFromDelta(delta.y);

            Vector3 allowedScrollAxis = AllowedScrollAxis;

            return new Vector3(positionX * allowedScrollAxis.x, positionY * allowedScrollAxis.y);
        }
    }

    private float PosFromDelta(float delta)
    {
        float pos = delta * Sensitivity * Time.deltaTime;
        return invertAxis ? pos : pos * -1;
    }

    private float Sensitivity
    {
        get { return sensitivity * 100; }
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

        float minX = anchorLeft.transform.position.x;
        float maxX = anchorRight.transform.position.x;

        bounds.min.x = minX;
        bounds.max.x = maxX;

        Debug.Log("Recalculated bounds: " + bounds);
    }
}