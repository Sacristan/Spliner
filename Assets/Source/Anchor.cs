using UnityEngine;
using System.Collections.Generic;

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
}