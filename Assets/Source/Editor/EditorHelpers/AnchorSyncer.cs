﻿using UnityEngine;

namespace BeetrootLab.Features
{
    public class AnchorSyncer
    {
        /// <summary>
        /// Synchronises this anchor to incoming and outcoming anchors
        /// </summary>
        /// <param name="targetAnchor"></param>
        public static void Sync(Anchor targetAnchor)
        {
            foreach (Anchor anchor in targetAnchor.IncomingAnchors)
            {
                if (anchor != null) SyncOutgoingAnchor(anchor, targetAnchor);
            }

            foreach (Anchor anchor in targetAnchor.OutgoingAnchors)
            {
                if (anchor != null) SyncIncomingAnchor(anchor, targetAnchor);
            }

            SyncAndCleanupAnchors(targetAnchor);
        }


        #region Private Methods

        /// <summary>
        /// Adds toAnchor to fromAnchor as incoming if required
        /// </summary>
        /// <param name="toAnchor"></param>
        /// <param name="fromAnchor"></param>
        private static void SyncIncomingAnchor(Anchor toAnchor, Anchor fromAnchor)
        {
            if (toAnchor == fromAnchor)
            {
                toAnchor.IncomingAnchors.RemoveAll(item => item == fromAnchor);
            }
            else
            {
                if (!toAnchor.IncomingAnchors.Contains(fromAnchor))
                {
                    toAnchor.IncomingAnchors.Add(fromAnchor);
                    AnchorSpliner.AddIncomingSpline(toAnchor, fromAnchor);
                }
            }
        }

        /// <summary>
        /// Adds toAnchor to fromAnchor as outgoing if required
        /// </summary>
        /// <param name="toAnchor"></param>
        /// <param name="fromAnchor"></param>
        public static void SyncOutgoingAnchor(Anchor toAnchor, Anchor fromAnchor)
        {
            if (toAnchor == fromAnchor)
            {
                toAnchor.OutgoingAnchors.RemoveAll(item => item == fromAnchor);
            }
            else
            {
                if (!toAnchor.OutgoingAnchors.Contains(fromAnchor))
                {
                    toAnchor.OutgoingAnchors.Add(fromAnchor);
                    AnchorSpliner.AddOutgoingSpline(toAnchor, fromAnchor);
                }
            }
        }

        /// <summary>
        /// General syncronization and cleanup of anchors 
        /// </summary>
        /// <param name="targetAnchor"></param>
        private static void SyncAndCleanupAnchors(Anchor targetAnchor)
        {
            Anchor[] anchors = GameObject.FindObjectsOfType<Anchor>();

            foreach (Anchor anchor in anchors)
            {
                if (anchor == targetAnchor) continue;

                if (anchor.IncomingAnchors.Contains(targetAnchor) && !targetAnchor.OutgoingAnchors.Contains(anchor))
                {
                    //anchor.IncomingAnchors.RemoveAll(item => item == this);

                    foreach (Anchor incomingAnchor in anchor.IncomingAnchors.ToArray())
                    {
                        if (incomingAnchor == targetAnchor)
                        {
                            AnchorSpliner.CleanupIncomingSplinesWithAnchor(anchor, targetAnchor);
                            anchor.IncomingAnchors.RemoveAll(item => item == targetAnchor);
                        }
                    }
                }

                if (anchor.OutgoingAnchors.Contains(targetAnchor) && !targetAnchor.IncomingAnchors.Contains(anchor))
                {
                    foreach (Anchor outgoingAnchor in anchor.OutgoingAnchors.ToArray())
                    {
                        if (outgoingAnchor == targetAnchor)
                        {
                            AnchorSpliner.CleanupIncomingSplinesWithAnchor(anchor, targetAnchor);
                            anchor.OutgoingAnchors.RemoveAll(item => item == targetAnchor);
                        }
                    }
                }
            }

            AnchorSpliner.CleanupSplines(targetAnchor);
        }

        #endregion

    }
}
