using UnityEngine;
using System.Collections;

namespace BeetrootLab.Features
{
    public class SplineMap : MonoBehaviour
    {
        [SerializeField]
        GameObject _anchorVisualisation;

        [SerializeField]
        GameObject _knobVisualisation;

        [SerializeField]
        Material _anchorLockedMaterial;

        [SerializeField]
        Material _anchorAvailableMaterial;

        [SerializeField]
        Material _anchorSelectedMaterial;

        public GameObject AnchorVisualisation
        {
            get { return _anchorVisualisation; }
        }

        public GameObject KnobVisualisation
        {
            get { return _knobVisualisation; }
        }

        public Material AnchorLockedMaterial
        {
            get { return _anchorLockedMaterial; }
        }

        public Material AnchorAvailableMaterial
        {
            get { return _anchorAvailableMaterial; }
        }

        public Material AnchorSelectedMaterial
        {
            get { return _anchorSelectedMaterial; }
        }
    }
}