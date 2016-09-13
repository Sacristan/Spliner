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
            }
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, Screen.height - 50, 400, 50), debugString);
        }
        #endregion

        private void OnDragStart()
        {
            dragPosStart = touch.position;
            dragStartTime = Time.time;
            debugString = string.Format("OnDragStart: dragStartPos: {0}", dragPosStart);
        }

        private void OnDragUpdate()
        {
            dragPosCurrent = touch.position;
            debugString = string.Format("OnDragUpdate: dragStartPos: {0} dragPosCurrent: {1}", dragPosStart, dragPosCurrent);
        }

        private void OnDragEnd()
        {
            dragPosEnd = touch.position;
            float diffTime = Time.time - dragStartTime;

            Vector2 momentum = dragPosEnd - dragPosStart;
            momentum /= (Time.time - dragStartTime);

            debugString = string.Format("OnDragEnd: dragStartPos: {0} dragPosEnd: {1} Momentum: {2}", dragPosStart, dragPosEnd, momentum);

            Reset();
        }

        private void Reset()
        {
            dragPosStart = Vector2.zero;
            dragPosEnd = Vector2.zero;
            dragPosCurrent = Vector2.zero;
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