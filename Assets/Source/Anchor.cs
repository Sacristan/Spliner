using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour
{
    [SerializeField]
    List<BezierSpline> incomingSplines = new List<BezierSpline>();

    [SerializeField]
    List<BezierSpline> outgoingSplines = new List<BezierSpline>();

    [SerializeField]
    private Anchor _nextAnchor;

    #region Properties

    public BezierSpline[] IncomingSplines
    {
        get
        {
            return incomingSplines.ToArray();
        }
    }

    public BezierSpline[] OutgoingSplines
    {
        get
        {
            return outgoingSplines.ToArray();
        }
    }

    public Anchor NextAnchor
    {
        get
        {
            return _nextAnchor;
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

    void OnEnable()
    {
        CleanupAndAddSplinesIfRequired();
    }

    void OnGUI()
    {
        if (Application.isPlaying) return;
        int height = 25;
        int width = 100;

        if (NextAnchor == null)
        {
            Vector2 pos = Camera.main.transform.InverseTransformPoint(transform.position);
            pos.x += width * -0.5f;
            pos.y = Screen.height - pos.y - height * 2.2f;
            Rect rect = new Rect(pos, new Vector2(width, height));
            GUI.Label(rect, "No End Anchor!");
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
        incomingSplines.Add(spline);
    }

    public void CleanupAndAddSplinesIfRequired()
    {
        CleanupSplines();
        AddSplinesIfRequired();
    }

    public void AddSplinesIfRequired()
    {
        RemoveRenundantSplinesFromArrays();
        if (_nextAnchor != null && outgoingSplines.Count < 1)
        {
            BezierSpline spline = SplineManager.AddSpline(this, _nextAnchor);
            outgoingSplines.Add(spline);
            _nextAnchor.AddIncomingSpline(spline);
        }
    }

    /// <summary>
    /// Cleanup outgoing and incoming(optional) splines
    /// </summary>
    /// <param name="cleanIncoming"> Clears also incoming splines </param>
    public void CleanupSplines(bool cleanIncoming=false)
    {
        Debug.Log("CleanupSplines called");

        if (Application.isPlaying) return;

        RemoveRenundantSplinesFromArrays();

        SplineManager.CleanupOutgoingSplinesForAnchor(this);
        foreach (BezierSpline spline in OutgoingSplines)
            outgoingSplines.Remove(spline);

        if (cleanIncoming)
        {
            SplineManager.CleanupIncomingSplinesForAnchor(this);
            foreach (BezierSpline spline in IncomingSplines)
                incomingSplines.Remove(spline);
        }

        RemoveRenundantSplinesFromArrays();
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
        outgoingSplines.RemoveAll(item => item == null);
        incomingSplines.RemoveAll(item => item == null);
    }

    #endregion

}