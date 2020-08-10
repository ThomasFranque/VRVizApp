using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplay : MonoBehaviour
{
    private const int BUTTON_AMOUNT = 4;
    private const int FADE_AMOUNT = (int)(240 / BUTTON_AMOUNT);
    private PageButton[] _buttons;

    private void Awake()
    {
        _buttons = new PageButton[BUTTON_AMOUNT];
        for (int i = 0; i < BUTTON_AMOUNT; i++)
        {
            _buttons[i] = transform.GetChild(i).gameObject.AddComponent<PageButton>();
            _buttons[i].Init(this, i);
        }
    }

    public void PageSelected(int index)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            float newFade = ((255 - (Mathf.Abs(_buttons[i].Index - index) * (float)FADE_AMOUNT)) / 255);
            _buttons[i].FadeTo(newFade);
        }

        OnPageSelected?.Invoke(index);
    }

    public Action<int> OnPageSelected;
}