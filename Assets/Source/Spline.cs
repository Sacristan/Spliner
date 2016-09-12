using UnityEngine;
using System.Collections;

public class Spline : MonoBehaviour
{
    [SerializeField]
    private Vector3 _p0;
    [SerializeField]
    private Vector3 _p1;
    [SerializeField]
    private Vector3 _p2;
    [SerializeField]
    private Vector3 _p3;

    [SerializeField]
    private Knob[] _knobs = new Knob[0];

    [SerializeField]
    private Anchor _startAnchor;
    [SerializeField]
    private Anchor _endAnchor;

    #region Spline point GET/SET
    public Vector3 P0
    {
        get { return _p0; }
        set { _p0 = value; }
    }

    public Vector3 P1
    {
        get { return _p1; }
        set { _p1 = value; }
    }

    public Vector3 P2
    {
        get { return _p2; }
        set { _p2 = value; }
    }

    public Vector3 P3
    {
        get { return _p3; }
        set { _p3 = value; }
    }
    #endregion

    public Anchor StartAnchor
    {
        get { return _startAnchor; }
        set { _startAnchor = value; }
    }

    public Anchor EndAnchor
    {
        get { return _endAnchor; }
        set { _endAnchor = value; }
    }

    public Knob[] Knobs
    {
        get { return _knobs; }
        set { _knobs = value; }
    }
}
