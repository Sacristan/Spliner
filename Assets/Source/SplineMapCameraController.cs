using UnityEngine;

namespace BeetrootLab.Features
{
    [RequireComponent(typeof(Camera))]
    public class SplineMapCameraController : MonoBehaviour
    {
        Vector2 oldTouchVector;
        float oldTouchDistance;

        Camera _camera;

        private Touch touch;
        private float moveSpeed = 15f;

        private Vector2 dragPosStart;
        private Vector2 dragPosCurrent;
        private Vector2 dragPosEnd;

        private float dragStartTime;
        private string debugString;

        private Vector3 movePos;
        private bool thrustInProgress =false;

        private float dampTime;

        Vector3 velocity;

        #region MonoBehaviour methods
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
                        OnDragStart();
                        break;
                    case TouchPhase.Ended:
                        OnDragEnd();
                        break;
                    case TouchPhase.Moved:
                        OnDragUpdate();
                        break;
                }

                HandleMovement();
            }
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, Screen.height - 50, 400, 50), debugString);
        }
        #endregion

        private void OnDragStart()
        {
            dragPosEnd = Vector2.zero;
            dragPosStart = touch.position;
            dragStartTime = Time.time;
            movePos = Vector3.zero;
            thrustInProgress = false;


            debugString = string.Format("OnDragStart: dragStartPos: {0}", dragPosStart);
        }

        private void OnDragUpdate()
        {
            dragPosCurrent = touch.position;

            Vector3 deltaPos = touch.deltaPosition;
            Vector3 calculatedPosDeltaLocal = deltaPos * _camera.orthographicSize / _camera.pixelHeight * -2f;
            Vector3 calculatedPosDeltaWorld = transform.TransformDirection(calculatedPosDeltaLocal);
            movePos = transform.position + calculatedPosDeltaWorld;

            dampTime = Time.deltaTime * moveSpeed;

            debugString = string.Format("OnDragUpdate: dragStartPos: {0} dragPosCurrent: {1} MovePos: {2}", dragPosStart, dragPosCurrent, movePos);

        }

        private void OnDragEnd()
        {
            dragPosEnd = touch.position;
            float diffTime = Time.time - dragStartTime;

            Vector3 momentum = dragPosEnd - dragPosStart;
            momentum /= (Time.time - dragStartTime);

            Vector3 deltaPos = (dragPosEnd - dragPosStart).normalized;

            Vector3 calculatedPosDeltaWorld = transform.TransformDirection(deltaPos + momentum);
            movePos = transform.position + calculatedPosDeltaWorld;

            dampTime = Time.deltaTime * moveSpeed * 500f;


            debugString = string.Format("OnDragEnd: dragStartPos: {0} dragPosEnd: {1} Momentum: {2} MovePos: {3}", dragPosStart, dragPosEnd, momentum, movePos);
        }

        private void HandleMovement()
        {
            if (movePos != Vector3.zero)
            {
                //transform.position = Vector3.Lerp(transform.position, movePos, lerpTime);

                transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, dampTime);
            }
        }

        //private void HandleMovement()
        //{
        //    Vector2 deltaPos = Vector2.zero;

        //    deltaPos = touch.deltaPosition;

        //    Vector3 calculatedPosDeltaLocal = deltaPos * _camera.orthographicSize / _camera.pixelHeight * -2f;
        //    Vector3 calculatedPosDeltaWorld = transform.TransformDirection(calculatedPosDeltaLocal);
        //    movePos = transform.position + calculatedPosDeltaWorld;
        //    transform.position = Vector3.Lerp(transform.position, movePos, Time.deltaTime * moveSpeed);
        //}

    }
}