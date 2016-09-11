using UnityEngine;
using System.Collections;

public class Spline : MonoBehaviour
{
    [SerializeField]
    private Transform _decoratorContainer;

    [SerializeField]
    private SplineDecorator _splineDecorator;

    [SerializeField]
    private Anchor _startAnchor;
    [SerializeField]
    private Anchor _endAnchor;

    public Anchor StartAnchor
    {
        get { return _startAnchor; }
    }

    public Anchor EndAnchor
    {
        get { return _endAnchor; }
    }

    public Knob[] Knobs
    {
        get { return DecoratorContainer.GetComponentsInChildren<Knob>(); }
    }

    public SplineDecorator SplineDecorator
    {
        get
        {
            if (_splineDecorator == null) _splineDecorator = GetComponent<SplineDecorator>();
            return _splineDecorator;
        }
    }

    public Transform DecoratorContainer
    {
        get
        {
            if (_decoratorContainer == null)
            {
                _decoratorContainer = new GameObject("Decorators").transform;
                _decoratorContainer.SetParent(transform);
            }
            return _decoratorContainer;
        }
    }

    public void MarkForDestruction()
    {
        DestroyImmediate(gameObject);
    }

    public void Decorate()
    {
        SplineDecorator.GenerateKnobs();
    }

    public static Spline Create(Anchor startAnchor, Anchor endAnchor)
    {
        GameObject splineGO = Instantiate(AnchorManager.SplineTemplate.gameObject) as GameObject;
        Spline spline = splineGO.GetComponent<Spline>();

        spline._startAnchor = startAnchor;
        spline._endAnchor = endAnchor;

        spline.transform.SetParent(AnchorManager.SplineContainer.transform);
        spline.gameObject.name = string.Format("Spline_{0}->{1}_{2}", spline.StartAnchor.gameObject.name, spline.EndAnchor.gameObject.name, System.Guid.NewGuid());

        return spline;
    }

}
