using UnityEngine;
using System.Collections;

public class Spline : MonoBehaviour
{

    [SerializeField]
    private Knob[] _knobs;

    [SerializeField]
    private Anchor _startAnchor;
    [SerializeField]
    private Anchor _endAnchor;

    public Anchor StartAnchor
    {
        get { return _startAnchor; }
        set { _startAnchor = value; }
    }

    public Anchor EndAnchor
    {
        get { return _endAnchor; }
        set { _endAnchor = value;  }
    }

    public Knob[] Knobs
    {
        get { return _knobs; }
        set { _knobs = value;  }
    }
}
