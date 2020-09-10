using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VRWheel.VR;

namespace VRWheel
{
    [RequireComponent(typeof(Button))]
    public class Grabber : MonoBehaviour
    {
        [SerializeField] private float _animationIntensity = 1.5f;
        [SerializeField] private Transform _pointerTarget = default;
        [SerializeField] private Transform _dragTarget = default;
        private float _distance = 0.8f;
        private Wheel _wheel;
        private Button _button;
        private Vector3 _initialScale;
        public void INIT(Wheel wheel)
        {
            _button = GetComponent<Button>();
            //_button.onClick.AddListener(CloseAll);
            _wheel = wheel;
            _initialScale = transform.localScale;
        }

        private void LateUpdate()
        {
            UpdateAction?.Invoke();
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

        public void StartFollow()
        {
            UpdateAction = FollowPointerTarget;
            _distance = Vector3.Distance(_pointerTarget.position, _dragTarget.position);
        }

        public void EndFollow()
        {
            UpdateAction = default;
        }

        private void FollowPointerTarget()
        {
            Vector3 targetPos = _pointerTarget.position + (_pointerTarget.forward * _distance);
            _dragTarget.position = Vector3.Lerp(_dragTarget.position, targetPos, 0.1f);
        }

        private void CloseAll()
        {
            _wheel.CloseAll();
        }

        Action UpdateAction;
    }
}