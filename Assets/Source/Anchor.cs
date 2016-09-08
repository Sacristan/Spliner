using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [SerializeField]
    private List<Anchor> _outgoingAnchors;

    [SerializeField]
    private List<Anchor> _incomingAnchors;

    [SerializeField]
    private List<BezierSpline> _outgoingSplines;

    [SerializeField]
    private List<BezierSpline> _incomingSplines;


    #region Properties
    public List<Anchor> OutgoingAnchors
    {
        get
        {
            return _outgoingAnchors;
        }

        set
        {
            _outgoingAnchors = value;
        }
    }

    public List<Anchor> IncomingAnchors
    {
        get
        {
            return _incomingAnchors;
        }

        set
        {
            _incomingAnchors = value;
        }
    }

    //TODO: Take from incoming anchors
    public List<BezierSpline> IncomingSplines
    {
        get
        {
            return _incomingSplines;
        }
    }

    public List<BezierSpline> OutgoingSplines
    {
        get
        {
            return _outgoingSplines;
        }
    }

    //TODO: Take from incoming splines
    public Knob[] OutgoingKnobs
    {
        get
        {
            return new Knob[0];
        }
    }

    //TODO: Take from outgoing splines
    public Knob[] IncomingKnobs
    {
        get
        {
            return new Knob[0];
        }
    }

    #endregion

    #region MonoBehaviour methods

    void OnDestroy()
    {
        //CleanupSplines(true);
    }

    void OnDisable()
    {
        //CleanupSplines(true);
    }

    void Awake()
    {
        //DecorateOutgoingSplines();
    }

    void OnEnable()
    {
        //TODO: This currently is not Generating Previous splines
        //if (PrevAnchor != null)
        //{
        //    PrevAnchor.AddSplinesIfRequired();
        //    PrevAnchor.DecorateOutgoingSplines();
        //}

        //CleanupAndAddSplinesIfRequired();
    }


    void Update()
    {
        foreach (Anchor anchor in _outgoingAnchors)
        {
            if(anchor!=null) Debug.DrawLine(this.transform.position, anchor.transform.position, Color.cyan);
        }
    }


    #endregion

    #region Anchor Sync
    public void SyncAnchors()
    {
        Debug.Log("Syncing Anchors...");

        foreach (Anchor anchor in _incomingAnchors)
        {
            if (anchor != null)
            {
                anchor.SyncOutgoingAnchor(this);
            }
        }

        foreach (Anchor anchor in _outgoingAnchors)
        {
            if (anchor != null)
            {
                anchor.SyncIncomingAnchor(this);
            }
        }

        SyncAndCleanupAnchors();
        //CleanupAndAddSplinesIfRequired();
    }

    public void SyncIncomingAnchor(Anchor anchor)
    {
        if (this == anchor)
        {
            this._incomingAnchors.Remove(anchor);
            AddOutgoingSpline(anchor);
        }
        else
        {
            if (!this._incomingAnchors.Contains(anchor))
            {
                this._incomingAnchors.Add(anchor);
                AddIncomingSpline(anchor);
            }
        }
    }

    public void SyncOutgoingAnchor(Anchor anchor)
    {
        if (this == anchor)
        {
            this._outgoingAnchors.Remove(anchor);
        }
        else
        {
            if (!this._outgoingAnchors.Contains(anchor))
            {
                this._outgoingAnchors.Add(anchor);
            }
        }
    }

    private void SyncAndCleanupAnchors()
    {
        Debug.Log("SyncAndCleanupAnchors");
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        foreach (Anchor anchor in anchors)
        {
            if (anchor == this) continue;

            if (anchor._incomingAnchors.Contains(this) && !this._outgoingAnchors.Contains(anchor))
            {
                anchor._incomingAnchors.RemoveAll(item => item == this);
            }

            if (anchor._outgoingAnchors.Contains(this) && !this._incomingAnchors.Contains(anchor))
            {
                anchor._outgoingAnchors.RemoveAll(item => item == this);
            }
        }

    }
    #endregion

    #region Spline methods

    private void AddIncomingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(anchor, this);
        _incomingSplines.Add(spline);
        anchor._outgoingSplines.Add(spline);
    }

    private void AddOutgoingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(this, anchor);
        _outgoingSplines.Add(spline);
        anchor._incomingSplines.Add(spline);
    }

    /// <summary>
    /// Decorates Outgoing Splines with Knobs
    /// </summary>
    public void DecorateOutgoingSplines()
    {
        foreach (BezierSpline spline in IncomingSplines)
        {
            if (spline == null) continue;
            spline.Decorate();
        }
    }

    #endregion
}