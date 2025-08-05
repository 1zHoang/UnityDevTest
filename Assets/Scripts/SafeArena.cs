using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;
    [SerializeField] float value;
    [SerializeField] Transform topUI;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
        if (!topUI) return;
        if (rectTransform.anchorMax.y < 1) topUI.localPosition = new Vector2(topUI.localPosition.x, topUI.localPosition.y + value);
    }
}
