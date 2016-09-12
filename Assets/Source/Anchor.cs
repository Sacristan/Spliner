using UnityEngine;
using System.Collections.Generic;

public class Anchor : MonoBehaviour
{
    [SerializeField]
    private Knob[] _incomingKnobs;

    [SerializeField]
    private Knob[] _outgoingKnobs;

    [SerializeField]
    private List<Spline> _outgoingSplines = new List<Spline>();

    [SerializeField]
    private List<Spline> _incomingSplines = new List<Spline>();

    [Header("Configure Anchor relations here")]
    [SerializeField]
    private List<Anchor> _outgoingAnchors = new List<Anchor>();

    [SerializeField]
    private List<Anchor> _incomingAnchors = new List<Anchor>();

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

    public Knob[] OutgoingKnobs
    {
        get { return _outgoingKnobs; }
        set { _outgoingKnobs = value; }
    }

    public Knob[] IncomingKnobs
    {
        get { return _incomingKnobs; }
        set { _incomingKnobs = value; }
    }

    #endregion
}