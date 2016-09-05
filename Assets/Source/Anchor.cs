using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour
{
    AnchorManager anchorManager;

    public static Anchor Create(AnchorManager manager)
    {
        GameObject anchorGO = Instantiate(manager.anchorTemplate) as GameObject;
        Anchor anchor = anchorGO.GetComponent<Anchor>();
        anchor.anchorManager = manager;
        anchor.transform.SetParent(manager.transform);
        anchor.transform.localPosition = Vector3.zero;
        return anchor;
    }

    void OnDestroy()
    {

    }

}
