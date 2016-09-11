using System.Collections.Generic;
using UnityEngine;

public class AnchorKnobSyncer {

    public static void RepopulateKnobs(Anchor targetAnchor)
    {
        targetAnchor.OutgoingKnobs = FetchKnobs(targetAnchor.OutgoingSplines);

        foreach (Anchor anchor in targetAnchor.OutgoingAnchors)
        {
            if (anchor != null) anchor.IncomingKnobs = targetAnchor.OutgoingKnobs;
        }
    }

    private static Knob[] FetchKnobs(List<Spline> splines)
    {
        List<Knob> knobsList = new List<Knob>();

        foreach (Spline spline in splines)
        {
            if (spline == null) continue;
            foreach (Knob knob in spline.SplineDecorator.Knobs)
            {
                if (knob != null && !knobsList.Contains(knob))
                    knobsList.Add(knob);
            }
        }

        knobsList.RemoveAll(item => item == null);
        return knobsList.ToArray();
    }
}
