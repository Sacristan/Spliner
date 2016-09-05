using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour
{
    AnchorManager anchorManager;

    public static Anchor Create(AnchorManager manager)
    {
        GameObject anchorGO = Instantiate(manager.anchorTemplate.gameObject) as GameObject;
        Anchor anchor = anchorGO.GetComponent<Anchor>();
        anchor.anchorManager = manager;
        anchor.Setup();
        return anchor;
    }

    private void Setup()
    {
        transform.SetParent(anchorManager.transform);
        transform.localPosition = Vector3.right * Random.Range(50, 800);
    }

    void OnDestroy()
    {

    }

}
