using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectResetView : MonoBehaviour
{
        // Start is called before the first frame update
    void Start()
    {
        ResetPosition();
    }

    void ResetPosition()
    {
        GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
    }
}
