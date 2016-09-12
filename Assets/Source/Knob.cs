using UnityEngine;
using System.Collections;

namespace BeetrootLab.Features
{
    public class Knob : MonoBehaviour
    {
        [SerializeField]
        private Spline _spline;

        public Spline Spline
        {
            get { return _spline; }
            set { _spline = value; }
        }

    }
}
