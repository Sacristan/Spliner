using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [Header("Do not Edit")]

    [SerializeField]
    private Knob[] _incomingKnobs;

    [SerializeField]
    private Knob[] _outgoingKnobs;

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

    public List<BezierSpline> IncomingSplines
    {
        get
        {
            return _incomingSplines;
        }

        set
        {
            _incomingSplines = value;
        }
    }

    public List<BezierSpline> OutgoingSplines
    {
        get
        {
            return _outgoingSplines;
        }

        set
        {
            _outgoingSplines = value;
        }
    }

    public Knob[] OutgoingKnobs
    {
        get
        {
            return _outgoingKnobs;
        }

        set
        {
            _outgoingKnobs = value;
        }
    }

    public Knob[] IncomingKnobs
    {
        get
        {
            return _incomingKnobs;
        }
        set
        {
            _incomingKnobs = value;
        }
    }

    #endregion

    #region Anchor Sync
    public void SyncAnchors()
    {
        //Debug.Log("Syncing Anchors...");

        foreach (Anchor anchor in IncomingAnchors)
        {
            if (anchor != null) anchor.SyncOutgoingAnchor(this);
        }

        foreach (Anchor anchor in OutgoingAnchors)
        {
            if (anchor != null) anchor.SyncIncomingAnchor(this);
        }

        SyncAndCleanupAnchors();
    }

    public void SyncIncomingAnchor(Anchor anchor)
    {
        if (this == anchor)
        {
            this.IncomingAnchors.RemoveAll(item => item == anchor);
            this.AddOutgoingSpline(anchor);
        }
        else
        {
            if (!this.IncomingAnchors.Contains(anchor))
            {
                this.IncomingAnchors.Add(anchor);
                this.AddIncomingSpline(anchor);
            }
        }
    }

    public void SyncOutgoingAnchor(Anchor anchor)
    {
        if (this == anchor)
        {
            this.OutgoingAnchors.RemoveAll(item => item == anchor);
        }
        else
        {
            if (!this.OutgoingAnchors.Contains(anchor))
            {
                this.OutgoingAnchors.Add(anchor);
                this.AddOutgoingSpline(anchor);
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

            if (anchor.IncomingAnchors.Contains(this) && !this.OutgoingAnchors.Contains(anchor))
            {
                //anchor.IncomingAnchors.RemoveAll(item => item == this);

                foreach (Anchor incomingAnchor in anchor.IncomingAnchors.ToArray())
                {
                    if (incomingAnchor == this)
                    {
                        anchor.CleanupIncomingSplinesWithAnchor(this);
                        anchor.IncomingAnchors.RemoveAll(item => item == this);
                    }
                }
            }

            if (anchor.OutgoingAnchors.Contains(this) && !this.IncomingAnchors.Contains(anchor))
            {
                foreach (Anchor outgoingAnchor in anchor.OutgoingAnchors.ToArray())
                {
                    if (outgoingAnchor == this)
                    {
                        anchor.CleanupOutgoingSplinesWithAnchor(this);
                        anchor.OutgoingAnchors.RemoveAll(item => item == this);
                    }
                }
            }
        }

        this.CleanupSplines();
    }
    #endregion

    #region Spline methods

    private void AddIncomingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(anchor, this);
        this.IncomingSplines.Add(spline);
        anchor.OutgoingSplines.Add(spline);
        this.CleanupSplines();
    }

    private void AddOutgoingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(this, anchor);
        this.OutgoingSplines.Add(spline);
        anchor.IncomingSplines.Add(spline);
        this.CleanupSplines();
    }

    public void CleanupIncomingSplinesWithAnchor(Anchor anchor)
    {
        Debug.Log("CleanupIncomingSplinesWithAnchor");

        foreach (BezierSpline spline in IncomingSplines.ToArray())
        {
            if (spline == null) continue;
            if (spline.StartAnchor == anchor)
            {
                this.IncomingSplines.RemoveAll(item => item == spline);
                anchor.OutgoingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        this.CleanupSplines();
    }

    public void CleanupOutgoingSplinesWithAnchor(Anchor anchor)
    {
        Debug.Log("CleanupOutgoingSplinesWithAnchor");

        foreach (BezierSpline spline in OutgoingSplines.ToArray())
        {
            if (spline == null) continue;
            if (spline.EndAnchor == anchor)
            {
                this.OutgoingSplines.RemoveAll(item => item == spline);
                anchor.IncomingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        this.CleanupSplines();
    }

    public void DecorateOutgoingSplines()
    {
        foreach (BezierSpline spline in this.IncomingSplines)
        {
            if (spline == null) continue;
            spline.Decorate();
        }
    }

    private void CleanupSplines()
    {
        this.RemoveRenundantSplinesFor(OutgoingSplines);
        this.RemoveRenundantSplinesFor(IncomingSplines);
    }

    private void RemoveRenundantSplinesFor(List<BezierSpline> list)
    {
        List<BezierSpline> splinesMap = new List<BezierSpline>();

        foreach (BezierSpline spline in list.ToArray())
        {
            if (spline == null || splinesMap.Contains(spline))
                list.Remove(spline);
            else
                splinesMap.Add(spline);
        }
    }

    #endregion


    #region Knob Sync

    public void RepopulateKnobs()
    {
        this.OutgoingKnobs = FetchKnobs(OutgoingSplines);

        foreach(Anchor anchor in OutgoingAnchors)
        {
            if(anchor != null) anchor.IncomingKnobs = this.OutgoingKnobs;
        }
    }

    private Knob[] FetchKnobs(List<BezierSpline> splines)
    {
        List<Knob> knobsList = new List<Knob>();

        foreach (BezierSpline spline in splines)
        {
            if (spline == null) continue;
            foreach (Knob knob in spline.SplineDecorator.Knobs)
            {
                if (knob != null && !knobsList.Contains(knob))
                    knobsList.Add(knob);
            }
        }

        knobsList.RemoveAll(item => item == null);
        return knobsList.ToArray();
    }

    #endregion

}