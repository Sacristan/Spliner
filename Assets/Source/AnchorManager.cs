using UnityEngine;
using System.Collections;

public class AnchorManager : MonoBehaviour
{

    private static AnchorManager _singletone;

    [HideInInspector]
    [SerializeField]
    private Transform _splineContainer;

    [SerializeField]
    private Spline splineTemplate;

    private static AnchorManager Singletone
    {
        get
        {
            if (_singletone == null)
                _singletone = FindObjectOfType<AnchorManager>();

            return _singletone;
        }
    }

    public static Transform SplineContainer
    {
        get
        {
            if (Singletone._splineContainer == null)
            {
                Singletone._splineContainer = new GameObject("Splines").transform;
                Singletone._splineContainer.SetParent(Singletone.transform);
            }
            return Singletone._splineContainer;
        }
    }

    public static GameObject SplineTemplate
    {
        get { return Singletone.splineTemplate.gameObject; }
    }
}
