using UnityEngine;
using System.Collections;

public class SplineMap : MonoBehaviour
{
    [SerializeField]
    GameObject _anchorVisualisation;

    [SerializeField]
    GameObject _knobVisualisation;

    public GameObject AnchorVisualisation
    {
        get { return _anchorVisualisation; }
    }
}
