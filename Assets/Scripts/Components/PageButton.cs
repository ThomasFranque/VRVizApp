using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PageButton : MonoBehaviour
{
    private Button _btn;
    private Image _img;
    public int Index { get; private set; }
    private PageDisplay _pageDisplay;
    private float _targetAlpha;

    public void Init(PageDisplay pageDisplay, int index)
    {
        _pageDisplay = pageDisplay;
        Index = index;
    }

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _img = GetComponent<Image>();

        _btn.onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        _pageDisplay.PageSelected(Index);
    }

    public void FadeTo(float percentage)
    {
        _targetAlpha = percentage;
        _img.DOFade(_targetAlpha, 0.5f).SetEase(Ease.OutCirc);
    }

    private void HardFade(float percentage)
    {
        Color c = _img.color;
        c.a = percentage;
        _img.color = c;
    }
}