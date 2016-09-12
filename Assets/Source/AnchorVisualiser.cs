using UnityEngine;
using System.Collections;

public class AnchorVisualiser : MonoBehaviour
{
    private enum AnchorVisualiserStatus { Locked = 0, Available = 1 };

    [SerializeField]
    private AnchorVisualiserStatus anchorStatus = AnchorVisualiserStatus.Locked;

    private MeshRenderer _meshRenderer;
    private SplineMap _splineMap;
    private Camera _camera;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _splineMap = transform.root.GetComponent<SplineMap>();
        _camera = _splineMap.transform.GetComponentInChildren<Camera>();
        AssignMaterial();
    }

    void Update()
    {
        if (CanAssignMaterial())
        {
            ChangeState();
            AssignMaterial();
        }
    }

    private void ChangeState()
    {
        switch (anchorStatus)
        {
            case AnchorVisualiserStatus.Locked:
                anchorStatus = AnchorVisualiserStatus.Available;
                break;
            case AnchorVisualiserStatus.Available:
                anchorStatus = AnchorVisualiserStatus.Locked;
                break;
        }
    }

    private bool CanAssignMaterial()
    {
        bool result = false;

        Vector2 touchPos = Vector2.zero;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
                touchPos = Input.GetTouch(0).position;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                touchPos = Input.mousePosition;
        }

        if (touchPos != Vector2.zero)
        {
            Ray ray = _camera.ScreenPointToRay(touchPos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                result = hit.collider.gameObject == gameObject;
        }

        return result;
    }

    private void AssignMaterial()
    {
        switch (anchorStatus)
        {
            case AnchorVisualiserStatus.Locked:
                _meshRenderer.sharedMaterial = _splineMap.AnchorLockedMaterial;
                break;
            case AnchorVisualiserStatus.Available:
                _meshRenderer.sharedMaterial = _splineMap.AnchorAvailableMaterial;
                break;
        }
    }
}
