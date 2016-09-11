using System.Collections.Generic;
using UnityEngine;

public class AnchorSpliner
{
    public static void AddIncomingSpline(Anchor targetAnchor, Anchor anchorToAdd)
    {
        AddSpline(anchorToAdd, targetAnchor);
    }

    public static void AddOutgoingSpline(Anchor targetAnchor, Anchor anchorToAdd)
    {
        AddSpline(targetAnchor, anchorToAdd);
    }

    public static void CleanupIncomingSplinesWithAnchor(Anchor targetAnchor, Anchor anchorToRemove)
    {
        Debug.Log("CleanupIncomingSplinesWithAnchor");

        foreach (Spline spline in targetAnchor.IncomingSplines.ToArray())
        {
            if (spline == null) continue;
            if (spline.StartAnchor == anchorToRemove)
            {
                targetAnchor.IncomingSplines.RemoveAll(item => item == spline);
                anchorToRemove.OutgoingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        CleanupSplines(targetAnchor);
    }

    public static void CleanupOutgoingSplinesWithAnchor(Anchor targetAnchor, Anchor anchorToRemove)
    {
        Debug.Log("CleanupOutgoingSplinesWithAnchor");

        foreach (Spline spline in targetAnchor.OutgoingSplines.ToArray())
        {
            if (spline == null) continue;
            if (spline.EndAnchor == anchorToRemove)
            {
                targetAnchor.OutgoingSplines.RemoveAll(item => item == spline);
                anchorToRemove.IncomingSplines.RemoveAll(item => item == spline);
                spline.MarkForDestruction();
            }
        }

        CleanupSplines(targetAnchor);
    }

    public static void DecorateOutgoingSplines(Anchor anchor)
    {
        foreach (Spline spline in anchor.IncomingSplines)
        {
            if (spline == null) continue;
            spline.Decorate();
        }
    }

    public static void CleanupSplines(Anchor anchor)
    {
        RemoveRenundantSplinesFor(anchor.OutgoingSplines);
        RemoveRenundantSplinesFor(anchor.IncomingSplines);
    }

    #region Private Methods

    private static void AddSpline(Anchor fromAnchor, Anchor toAnchor)
    {
        Spline spline = Spline.Create(fromAnchor, toAnchor);
        toAnchor.IncomingSplines.Add(spline);
        fromAnchor.OutgoingSplines.Add(spline);

        CleanupSplines(toAnchor);
        CleanupSplines(fromAnchor);
    }


    private static void RemoveRenundantSplinesFor(List<Spline> list)
    {
        List<Spline> splinesMap = new List<Spline>();

        foreach (Spline spline in list.ToArray())
        {
            if (spline == null || splinesMap.Contains(spline))
                list.Remove(spline);
            else
                splinesMap.Add(spline);
        }
    }
    #endregion
}
