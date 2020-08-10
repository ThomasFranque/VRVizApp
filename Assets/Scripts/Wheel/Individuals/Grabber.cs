using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VRWheel
{
    [RequireComponent(typeof(Button))]
    public class Grabber : MonoBehaviour
    {
        [SerializeField] private float _animationIntensity = 1.5f;
        private Wheel _wheel;
        private Button _button;
        private Vector3 _initialScale;
        public void INIT(Wheel wheel)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CloseAll);
            _wheel = wheel;
            _initialScale = transform.localScale;
        }

        public void Expand()
        {
            transform.DOKill();
            transform.DOScale(_initialScale, 1.3f);
        }

        public void Shrink()
        {
            transform.DOKill();
            transform.DOScale(_initialScale / 1.5f, .3f);
        }

        private void CloseAll()
        {
            _wheel.CloseAll();
        }
    }
}