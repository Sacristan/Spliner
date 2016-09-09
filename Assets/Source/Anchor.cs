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

    #region Spline methods

    public void AddIncomingSpline(Anchor anchor)
    {
        BezierSpline spline = BezierSpline.Create(anchor, this);
        this.IncomingSplines.Add(spline);
        anchor.OutgoingSplines.Add(spline);
        this.CleanupSplines();
    }

    public void AddOutgoingSpline(Anchor anchor)
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

    public void CleanupSplines()
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