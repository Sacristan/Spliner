﻿using UnityEngine;
using System.Collections;

public class Knob : MonoBehaviour
{
    public BezierSpline Spline
    {
        get
        {
            return null;
        }
    }

    public Anchor OutgoingAnchor
    {
        get
        {
            if (Spline != null) return Spline.OutgoingAnchor;
            return null;
        }
    }


    public Anchor IncomingAnchor
    {
        get
        {
            if (Spline != null) return Spline.IncomingAnchor;
            return null;
        }
    }

}