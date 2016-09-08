using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{

    [SerializeField]
    private Anchor[] _outgoingAnchors;

    [SerializeField]
    private Anchor[] _incomingAnchors;

    #region Properties
    public Anchor[] OutgoingAnchors
    {
        get
        {
            return _outgoingAnchors;
        }
    }

    public Anchor[] IncomingAnchors
    {
        get
        {
            return _incomingAnchors;
        }
    }

    //TODO: Take from incoming anchors
    public BezierSpline[] IncomingSplines
    {
        get
        {
            return new BezierSpline[0];
        }
    }

    //TODO: Take from outgoing anchors
    public BezierSpline[] OutgoingSplines
    {
        get
        {
            return new BezierSpline[0];
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

    void OnValidate()
    {
        AddSplinesIfRequired();
    }

    void OnDestroy()
    {
        CleanupSplines(true);
    }

    void OnDisable()
    {
        CleanupSplines(true);
    }

    void Awake()
    {
        DecorateOutgoingSplines();
    }

    void OnEnable()
    {
        //TODO: This currently is not Generating Previous splines
        //if (PrevAnchor != null)
        //{
        //    PrevAnchor.AddSplinesIfRequired();
        //    PrevAnchor.DecorateOutgoingSplines();
        //}

        CleanupAndAddSplinesIfRequired();
    }

    void OnGUI()
    {
        //if (Application.isPlaying) return;
        //int height = 25;
        //int width = 100;

        //if (_nextAnchor == null)
        //{
        //    Vector2 pos = Camera.main.transform.InverseTransformPoint(transform.position);
        //    pos.x += width * -0.5f;
        //    pos.y = Screen.height - pos.y - height * 2.2f;
        //    Rect rect = new Rect(pos, new Vector2(width, height));
        //    GUI.Label(rect, "No End Anchor!");
        //}
    }

    #endregion

    #region Spline methods

    /// <summary>
    /// Addds incoming splines
    /// </summary>
    /// <param name="spline"></param>
    public void AddIncomingSpline(BezierSpline spline)
    {
        //incomingSplines.Add(spline);
    }

    /// <summary>
    /// Cleans up splines and adds new ones (if required) on request
    /// </summary>
    public void CleanupAndAddSplinesIfRequired()
    {
        CleanupSplines();
        AddSplinesIfRequired();
    }


    /// <summary>
    /// Checks if next anchor present and if outgoint splines already arent over limit. If ok then generates a new outgoing spline
    /// </summary>
    public void AddSplinesIfRequired()
    {
        // TODO: FIX

        //RemoveRenundantSplinesFromArrays();
        //if (_nextAnchor != null)
        //{
        //    if (outgoingSplines.Count < 1)
        //    {
        //        //_nextAnchor.PrevAnchor = this;

        //        BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
        //        outgoingSplines.Add(spline);
        //        _nextAnchor.AddIncomingSpline(spline);
        //    }
        //}
    }

    /// <summary>
    /// Cleanup outgoing and incoming(optional) splines
    /// </summary>
    /// <param name="cleanIncoming"> Clears also incoming splines </param>
    public void CleanupSplines(bool cleanIncoming = false)
    {
        //Debug.Log("CleanupSplines called");

        //if (Application.isPlaying) return;

        //RemoveRenundantSplinesFromArrays();

        //SplineManager.CleanupOutgoingSplinesForAnchor(this);
        //foreach (BezierSpline spline in OutgoingSplines)
        //    outgoingSplines.Remove(spline);

        //if (cleanIncoming)
        //{
        //    SplineManager.CleanupIncomingSplinesForAnchor(this);
        //    foreach (BezierSpline spline in IncomingSplines)
        //        incomingSplines.Remove(spline);
        //}

        //RemoveRenundantSplinesFromArrays();
    }

    /// <summary>
    /// Decorates Outgoing Splines with Knobs
    /// </summary>
    public void DecorateOutgoingSplines()
    {
        foreach (BezierSpline spline in this.OutgoingSplines)
        {
            if (spline == null) continue;
            spline.SplineDecorator.GenerateKnobs();
        }
    }

    /// <summary>
    /// Removes null entries in Spline Lists
    /// </summary>
    private void RemoveRenundantSplinesFromArrays()
    {
        //outgoingSplines.RemoveAll(item => item == null);
        //incomingSplines.RemoveAll(item => item == null);
    }

    #endregion

}