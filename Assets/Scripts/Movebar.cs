using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movebar : MonoBehaviour
{
    private Vector2 lastMousePos;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            lastMousePos = mousePos;
        }

        if (Input.GetMouseButton(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastMousePos;
                rectTransform.anchoredPosition += delta;
                lastMousePos = Input.mousePosition;
            }
        }
    }
}
