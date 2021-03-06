﻿using UnityEngine;
using System;

namespace BeetrootLab.Features
{
    public class BezierSpline
    {
        private Spline _spline;

        public Spline Spline
        {
            get { return _spline; }
        }

        /// <summary>
        /// Sets Spline ref, creates and places points if required
        /// </summary>
        /// <param name="spline"></param>
        public BezierSpline(Spline spline)
        {
            _spline = spline;

            if (spline.StartAnchor == null || spline.EndAnchor == null)
            {
                UnityEngine.Object.DestroyImmediate(spline.gameObject);
                return;
            }

            Vector3 startPos = Spline.transform.InverseTransformPoint(Spline.StartAnchor.transform.position);
            Vector3 endPos = Spline.transform.InverseTransformPoint(Spline.EndAnchor.transform.position);

            Vector3 diff = endPos - startPos;

            if (Spline.P1 == Vector3.zero)
            {
                Vector3 calculatedP1 = startPos + diff * 0.45f + Vector3.up;
                SetControlPoint(1, calculatedP1);
            }

            if (Spline.P2 == Vector3.zero)
            {
                Vector3 calculatedP2 = startPos + diff * 0.87f + Vector3.up;
                SetControlPoint(2, calculatedP2);
            }

            UpdateAnchorControlPoints();
        }

        #region Properties
        public Vector3 StartPoint
        {
            get { return GetPoint(0f); }
        }

        public Vector3 EndPoint
        {
            get { return GetPoint(1f); }
        }

        public float Length
        {
            get
            {
                float result = 0f;
                for (int i = 1; i < Points.Length; i++)
                {
                    result += Vector3.Distance(Points[i - 1], Points[i]);
                }
                return result;
            }
        }

        public int ControlPointCount
        {
            get
            {
                return Points.Length;
            }
        }

        private Vector3[] Points
        {
            get
            {
                return new Vector3[] {
                Spline.P0,
                Spline.P1,
                Spline.P2,
                Spline.P3
            };
            }
        }

        public int CurveCount
        {
            get
            {
                return (Points.Length - 1) / 3;
            }
        }
        #endregion

        /// <summary>
        /// Gets point by passed info
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetControlPoint(int index)
        {
            return Points[index];
        }

        /// <summary>
        /// Updates first and Last point whose are considered Anchor points
        /// </summary>
        public void UpdateAnchorControlPoints()
        {
            SetControlPoint(0, Spline.transform.InverseTransformPoint(Spline.StartAnchor.transform.position));
            SetControlPoint(Points.Length - 1, Spline.transform.InverseTransformPoint(Spline.EndAnchor.transform.position));
        }

        /// <summary>
        /// Sets point (must InverseTransformPoint already) at index. Supports 0..3 indexes
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        public void SetControlPoint(int index, Vector3 point)
        {
            switch (index)
            {
                case 0:
                    Spline.P0 = point;
                    break;
                case 1:
                    Spline.P1 = point;
                    break;
                case 2:
                    Spline.P2 = point;
                    break;
                case 3:
                    Spline.P3 = point;
                    break;
            }

        }

        /// <summary>
        /// Gets a Vector3 at a certain length along Spline path
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return Spline.transform.TransformPoint(Bezier.GetPoint(Points[i], Points[i + 1], Points[i + 2], Points[i + 3], t));
        }

        /// <summary>
        /// Gets velocity at a certain length along Spline path 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return Spline.transform.TransformPoint(Bezier.GetFirstDerivative(Points[i], Points[i + 1], Points[i + 2], Points[i + 3], t)) - Spline.transform.position;
        }

        /// <summary>
        /// Gets Normal's at a certain length along Spline path  
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

    }
}