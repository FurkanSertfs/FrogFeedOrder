using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.position = Input.mousePosition;
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            Cursor.visible = false;
        }
    }
}
