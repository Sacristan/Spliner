using UnityEngine;
using System.Collections;

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

    public GameObject AnchorVisualisation
    {
        get { return _anchorVisualisation; }
    }

    public Material AnchorLockedMaterial
    {
        get { return _anchorLockedMaterial; }
    }

    public Material AnchorAvailableMaterial
    {
        get { return _anchorAvailableMaterial; }
    }
}
