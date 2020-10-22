using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorInverter : MonoBehaviour
{
    [SerializeField] private Image _image1 = default;
    [SerializeField] private Image _image2 = default;

    public void Invert()
    {
        Color c = _image1.color;
        _image1.color = _image2.color;
        _image2.color = c;
    }
}