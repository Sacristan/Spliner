using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour
{
    private static int width = 100;
    private static int height = 25;
    private static int xOffset = -50;
    private static int yOffset = -40;

    private static Anchor startAnchor;
    private static Anchor endAnchor;

    void OnGUI()
    {
        if (!Application.isEditor) return;

        Vector2 pos = Camera.main.transform.InverseTransformPoint(transform.position);
        pos.y = Screen.height - pos.y - height;

        pos += new Vector2(xOffset, yOffset);

        Rect rect = new Rect(pos, new Vector2(width, height));

        if (startAnchor == null)
        {
            if (GUI.Button(rect, "Create Spline"))
            {
                StartAnchor();
            }
        }
        else
        {
            if (startAnchor == this) return;
            if (GUI.Button(rect, "Finish Spline"))
            {
                EndAnchor();
            }
        }

    }

    void StartAnchor()
    {
        startAnchor = this;
    }

    void EndAnchor()
    {
        endAnchor = this;

        startAnchor = null;
        endAnchor = null;
    }
}