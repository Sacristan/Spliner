using UnityEngine;
using System.Collections;

public class SplineMapCameraController : MonoBehaviour
{
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;

    Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // swipe begins
        {
            startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))    // swipe ends
        {
            endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (startPosition != endPosition && startPosition != Vector3.zero && endPosition != Vector3.zero)
        {
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;

            Vector2 delta = new Vector2(deltaX, deltaY);

            //Debug.Log(delta);

            transform.Translate(delta);

            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
        }
    }
}