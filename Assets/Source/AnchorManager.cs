using UnityEngine;
using System.Collections.Generic;

public class AnchorManager : MonoBehaviour
{
    [SerializeField]
    private List<Anchor> anchors = new List<Anchor>();

    public GameObject anchorTemplate;
    public List<Anchor> Anchors { get { return anchors; } }

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
}