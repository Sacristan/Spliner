using UnityEngine;
using System.Collections;

public class SplineMapCameraMovement : MonoBehaviour
{
    private enum PinnedAxis { X, Y, Z, XY, XZ, YZ, XYZ} 

    Camera _camera;
    Touch touch;

    [SerializeField]
    private float sensitivity = 0.25f;

    [SerializeField]
    private bool invertAxis = false;

    [SerializeField]
    private float inertiaDurationInSeconds = 1f;

    [SerializeField]
    private PinnedAxis pinnedAxis = PinnedAxis.X;

    private readonly float scrollVelocityTreshold = 100f;

    private float timeTouchPhaseEnded;
    private float scrollVelocity;
    private Vector3 scrollDirection;

    void Awake()
    {
        _camera = GetComponent<Camera>();
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
                    //touch = null;
                    break;
            }
        }
        else
        {
            HandleScrollMovement();
        }
    }

    private void HandleMovement()
    {
        _camera.transform.position += GetMovementVectorByDelta();
        scrollDirection = touch.deltaPosition.normalized;
        scrollVelocity = touch.deltaPosition.magnitude / touch.deltaTime;

        if (scrollVelocity <= scrollVelocityTreshold) scrollVelocity = 0;
    }

    private void HandleScrollMovement()
    {
        if(Mathf.Abs(scrollVelocity) > 0f)
        {
            //slow down over time
            float t = (Time.time - timeTouchPhaseEnded) / inertiaDurationInSeconds;
            float frameVelocity = Mathf.Lerp(scrollVelocity, 0.0f, t);
            _camera.transform.position += -(Vector3)scrollDirection.normalized * (frameVelocity * 0.05f) * Time.deltaTime;

            if (t >= 1.0f) scrollVelocity = 0.0f;
        }
    }

    private Vector3 GetMovementVectorByDelta()
    {
        Vector3 delta = touch.deltaPosition;

        float positionX = 0f;
        float positionY = 0f;
        float positionZ = 0f;

        switch (pinnedAxis)
        {
            case PinnedAxis.X:
                positionX = PosFromDelta(delta.x);
                break;
            case PinnedAxis.Y:
                positionY = PosFromDelta(delta.y);
                break;
            case PinnedAxis.Z:
                positionZ = PosFromDelta(delta.z);
                break;
            case PinnedAxis.XY:
                positionX = PosFromDelta(delta.x);
                positionY = PosFromDelta(delta.y);
                break;
            case PinnedAxis.XZ:
                positionX = PosFromDelta(delta.x);
                positionZ = PosFromDelta(delta.z);
                break;
            case PinnedAxis.YZ:
                positionY = PosFromDelta(delta.y);
                positionZ = PosFromDelta(delta.z);
                break;
            case PinnedAxis.XYZ:
                positionX = PosFromDelta(delta.x);
                positionY = PosFromDelta(delta.y);
                positionZ = PosFromDelta(delta.z);
                break;
        }

        return new Vector3(positionX, positionY, positionZ);
    }

    private float PosFromDelta(float delta)
    {
        float pos = delta * sensitivity * Time.deltaTime;
        return invertAxis ? pos : pos * -1;
    }
}