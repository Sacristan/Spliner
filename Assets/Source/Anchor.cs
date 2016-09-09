using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
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