using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{

    [SerializeField]
    private List<Anchor> _outgoingAnchors;

    [SerializeField]
    private List<Anchor> _incomingAnchors;

    #region Properties
    public Anchor[] OutgoingAnchors
    {
        get
        {
            return _outgoingAnchors.ToArray();
        }

        set
        {
            _outgoingAnchors = new List<Anchor>(value);
        }
    }

    public Anchor[] IncomingAnchors
    {
        get
        {
            return _incomingAnchors.ToArray();
        }

        set
        {
            _incomingAnchors = new List<Anchor>(value);
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
        //foreach (Anchor anchor in _incomingAnchors)
        //{
        //    Debug.DrawLine(this.transform.position, anchor.transform.position, Color.red);
        //}

        foreach (Anchor anchor in _outgoingAnchors)
        {
            if(anchor!=null) Debug.DrawLine(this.transform.position, anchor.transform.position, Color.cyan);
        }
    }


    #endregion

    #region Anchor Sync
    public void SyncAnchors()
    {
        Debug.Log("Syncing Anchors...");

        foreach (Anchor anchor in _incomingAnchors)
        {
            if (anchor != null) anchor.SyncOutgoingAnchor(this);
        }

        foreach (Anchor anchor in _outgoingAnchors)
        {
            if (anchor != null) anchor.SyncIncomingAnchor(this);
        }

        SyncAndCleanupAnchors();
    }

    public void SyncIncomingAnchor(Anchor anchor)
    {
        if (!this._incomingAnchors.Contains(anchor) && this != anchor)
        {
            this._incomingAnchors.Add(anchor);
        }
    }

    public void SyncOutgoingAnchor(Anchor anchor)
    {
        if (!this._outgoingAnchors.Contains(anchor) && this != anchor)
        {
            this._outgoingAnchors.Add(anchor);
        }
    }

    private void SyncAndCleanupAnchors()
    {
        Debug.Log("SyncAndCleanupAnchors");
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        foreach(Anchor anchor in anchors)
        {
            if (anchor == this) continue;

            anchor._incomingAnchors.RemoveAll(item => !item._outgoingAnchors.Contains(anchor));
            anchor._outgoingAnchors.RemoveAll(item => !item._incomingAnchors.Contains(anchor));

            //anchor._incomingAnchors.RemoveAll(item => item == null);
            //anchor._outgoingAnchors.RemoveAll(item => item == null);
        }

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