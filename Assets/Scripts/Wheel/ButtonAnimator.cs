using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimator : MonoBehaviour
{
    private Button _button;
    private Vector3 _initialScale;

    [SerializeField] private float _expandSpeed = 1.3f;
    [SerializeField] private float _shrinkSpeed = .3f;

    public void Awake()
    {
        _button = GetComponent<Button>();
        _initialScale = transform.localScale;
    }

    public void Expand()
    {
        transform.DOKill();
        transform.DOScale(_initialScale, _expandSpeed);
    }

    public void Shrink()
    {
        transform.DOKill();
        transform.DOScale(_initialScale / 1.5f, _shrinkSpeed);
    }
}