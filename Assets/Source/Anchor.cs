using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [Header("Do not Edit")]
    [SerializeField]
    private List<BezierSpline> _outgoingSplines = new List<BezierSpline>();

    [SerializeField]
    private List<BezierSpline> _incomingSplines = new List<BezierSpline>();

    [Header("Configure Anchor relations here")]

    [SerializeField]
    private List<Anchor> _outgoingAnchors = new List<Anchor>();

    [SerializeField]
    private List<Anchor> _incomingAnchors = new List<Anchor>();

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
        //Debug.Log("Syncing Anchors...");

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
            this._incomingAnchors.RemoveAll(item => item == anchor);
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
            this._outgoingAnchors.RemoveAll(item => item == anchor);
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
        //Debug.Log("SyncAndCleanupAnchors");
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        foreach (Anchor anchor in anchors)
        {
            if (anchor == this) continue;

            if (anchor._incomingAnchors.Contains(this) && !this._outgoingAnchors.Contains(anchor))
            {
                //anchor._incomingAnchors.RemoveAll(item => item == this);

                foreach (Anchor incomingAnchor in anchor._incomingAnchors.ToArray())
                {
                    if (incomingAnchor == this)
                    {
                        anchor.CleanupIncomingSplinesWithAnchor(this);
                        anchor._incomingAnchors.RemoveAll(item => item == this);
                    }
                }
            }

            if (anchor._outgoingAnchors.Contains(this) && !this._incomingAnchors.Contains(anchor))
            {
                foreach (Anchor outgoingAnchor in anchor._outgoingAnchors.ToArray())
                {
                    if (outgoingAnchor == this)
                    {
                        anchor.CleanupOutgoingSplinesWithAnchor(this);
                        anchor._outgoingAnchors.RemoveAll(item => item == this);
                    }
                }
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
        RemoveRenundantSplinesFromArrays();
    }

    private void AddOutgoingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(this, anchor);
        _outgoingSplines.Add(spline);
        anchor._incomingSplines.Add(spline);
        RemoveRenundantSplinesFromArrays();
    }

    public void CleanupIncomingSplinesWithAnchor(Anchor anchor)
    {
        //Debug.Log("CleanupIncomingSplinesWithAnchor");

        foreach (BezierSpline spline in _incomingSplines.ToArray())
        {
            if (spline.StartAnchor == anchor)
            {
                _incomingSplines.RemoveAll(item => item == spline);
                anchor._outgoingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        RemoveRenundantSplinesFromArrays();
    }

    public void CleanupOutgoingSplinesWithAnchor(Anchor anchor)
    {
        //Debug.Log("CleanupOutgoingSplinesWithAnchor");

        foreach (BezierSpline spline in _outgoingSplines.ToArray())
        {
            if (spline.EndAnchor == anchor)
            {
                _outgoingSplines.RemoveAll(item => item == spline);
                anchor._incomingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        RemoveRenundantSplinesFromArrays();
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

    /// <summary>
    /// Removes null entries in Spline Lists
    /// </summary>
    private void RemoveRenundantSplinesFromArrays()
    {
        _outgoingSplines.RemoveAll(item => item == null);
        _incomingSplines.RemoveAll(item => item == null);
    }

    #endregion
}