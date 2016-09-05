using UnityEngine;
using System.Collections.Generic;

public class AnchorManager : MonoBehaviour
{
    [SerializeField]
    private List<Anchor> anchors = new List<Anchor>();

    public List<Anchor> Anchors { get { return anchors; } }

    public Anchor anchorTemplate;
    public BezierSpline splineTemplate;

    public void AddAnchor()
    {
        Anchor anchor = Anchor.Create(this);
        anchors.Add(anchor);
    }

    public void RemoveAnchor(Anchor anchor)
    {
        anchors.Remove(anchor);
        Destroy(anchor.gameObject);
    }

    public Anchor AnchorAtIndex(int index)
    {
        Anchor anchor = null;
        if (index >= 0 && Anchors.Count > index) anchor = Anchors[index];
        return anchor;
    }

    public int IndexForAnchor(Anchor anchor)
    {
        int result = -1;
        result = Anchors.IndexOf(anchor);
        return result;
    }

}