using UnityEngine;
using System.Collections.Generic;
using System;

public class Anchor : MonoBehaviour
{
    private enum AnchorStatus { Locked=0, Available=1 };

    [SerializeField]
    private List<Spline> _outgoingSplines = new List<Spline>();

    [SerializeField]
    private List<Spline> _incomingSplines = new List<Spline>();

    [Header("Configure Anchor relations here")]
    [SerializeField]
    private List<Anchor> _outgoingAnchors = new List<Anchor>();

    [SerializeField]
    private List<Anchor> _incomingAnchors = new List<Anchor>();

    #region RuntimeVars
    [SerializeField]
    private AnchorStatus anchorStatus = AnchorStatus.Locked;
    MeshRenderer meshRenderer;
    #endregion

    #region Properties
    public List<Anchor> OutgoingAnchors
    {
        get { return _outgoingAnchors; }
        set { _outgoingAnchors = value; }
    }

    public List<Anchor> IncomingAnchors
    {
        get { return _incomingAnchors; }
        set { _incomingAnchors = value; }
    }

    public List<Spline> IncomingSplines
    {
        get { return _incomingSplines; }
        set { _incomingSplines = value; }
    }

    public List<Spline> OutgoingSplines
    {
        get { return _outgoingSplines; }
        set { _outgoingSplines = value; }
    }

    #endregion

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
            case AnchorStatus.Locked:
                anchorStatus = AnchorStatus.Available;
                break;
            case AnchorStatus.Available:
                anchorStatus = AnchorStatus.Locked;
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
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

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
            case AnchorStatus.Locked:
                meshRenderer.sharedMaterial = Resources.Load("Materials/Anchor/Locked") as Material;
                break;
            case AnchorStatus.Available:
                meshRenderer.sharedMaterial = Resources.Load("Materials/Anchor/Available") as Material;
                break;
        }
    }
}