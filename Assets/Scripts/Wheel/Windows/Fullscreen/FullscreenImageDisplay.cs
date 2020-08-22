﻿using ArchiveLoad;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VRWheel.Fullscreen
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FullscreenImageDisplay : MonoBehaviour
    {
        private static FullscreenImageDisplay instance;

        public static void DisplayDual(ArchiveInfo info) => instance.InstanceDisplayDual(info);
        public static void DisplayZoomed(ArchiveInfo info) => instance.InstanceDisplayZoomed(info);

        private static Color transparent;

        private ZoomedDisplay _zoomed;
        private DualDisplay _dual;
        private CanvasGroup _canvasGroup;

        [SerializeField] private Image _zoomImage = default;

        [Space]

        [SerializeField] private Image _leftDualImage = default;
        [SerializeField] private Image _rightDualImage = default;

        private bool _open;

        private void Awake()
        {
            instance = this;
            transparent = Color.white;
            transparent.a = 0;

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _open = false;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
                Close();
        }

        private void InstanceDisplayDual(ArchiveInfo info)
        {
            _open = true;
            AnimateFade(1);
            _zoomImage.color = transparent;
            _leftDualImage.color = Color.white;
            _rightDualImage.color = Color.white;

            _leftDualImage.sprite = info.Images.Left;
            _rightDualImage.sprite = info.Images.Right;
        }
        private void InstanceDisplayZoomed(ArchiveInfo info)
        {
            _open = true;
            AnimateFade(1);
            _zoomImage.color = Color.white;
            _leftDualImage.color = transparent;
            _rightDualImage.color = transparent;

            _zoomImage.sprite = info.Images.Full;
        }
        public void Close()
        {
            if (!_open) return;
            _open = false;
            AnimateFade(0);
        }

        private float _fadeIntensity;
        private Tween _fadeTween;
        private void AnimateFade(float to)
        {
            if (_fadeTween != null)
            {
                DOTween.Kill(_fadeTween);
                _fadeIntensity = _canvasGroup.alpha;
            }
            _canvasGroup.gameObject.SetActive(true);

            _fadeTween = DOTween.To(() => _fadeIntensity,
                    x => _fadeIntensity = x, to, 0.8f)
                .OnUpdate(() => _canvasGroup.alpha = _fadeIntensity);
            _fadeTween.OnComplete(OnComplete);

            void OnComplete()
            {
                _fadeTween = null;
                if (to == 0)
                    _canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}