using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VRWheel.Layer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LayerImage : MonoBehaviour
    {
        [Header("General References")]
        [SerializeField] private Button _removeButton = default;
        [SerializeField] private CanvasGroup _optionsCanvasGroup = default;
        [SerializeField] private GameObject[] _optionsButtons = default;

        [Header("Image info references")]
        [SerializeField] private Image _image = default;
        [SerializeField] private TextMeshProUGUI _theme = default;
        [SerializeField] private TextMeshProUGUI _collection = default;

        [Header("Main Fade Settings")]
        [SerializeField] float _fadeDuration = .5f;
        private CanvasGroup _mainCanvasGroup;
        private ArchiveLoad.ArchiveInfo _attachedInfo;

        private void Awake()
        {
            _mainCanvasGroup = GetComponent<CanvasGroup>();
            _mainCanvasGroup.alpha = 0;
            _optionsCanvasGroup.alpha = 0;
            _optFadeIntensity = _optionsCanvasGroup.alpha;
            _mainFadeIntensity = _mainCanvasGroup.alpha;
            _removeButton.onClick.AddListener(Remove);
            //Initialize();
        }

        private void Start() { }

        public void Initialize(ArchiveLoad.ArchiveInfo info)
        {
            _attachedInfo = info;
            _image.sprite = info.Image;
            _collection.SetText(info.Owner);
            _theme.SetText(info.Topic);

            Showtime();
        }

        public void Showtime()
        {
            gameObject.SetActive(true);
            AnimateMainFade(1);
            CloseOptions();
        }

        public void ShowtimeOver()
        {
            AnimateMainFade(0);
            _mainFadeTween.OnComplete(() => gameObject.SetActive(false));
            CloseOptions();
        }

        public void Remove()
        {
            _mainCanvasGroup.interactable = false;
            _optionsCanvasGroup.interactable = false;
            _removeButton.gameObject.SetActive(false);
            AnimateMainFade(0);
            transform.DOScale(Vector3.zero, 0.4f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => Destroy(gameObject));
            OnRemove?.Invoke(this);
        }

        public void OpenOptions()
        {
            _removeButton.gameObject.SetActive(false);
            AnimateOptionsFade(1);
            for (int i = 0; i < _optionsButtons.Length; i++)
            {
                Transform target = _optionsButtons[i].transform;
                KillRunningAnimationsOn(target);
                target.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f * i);
            }
        }

        public void CloseOptions()
        {
            _removeButton.gameObject.SetActive(true);
            AnimateOptionsFade(0);
            for (int i = 0; i < _optionsButtons.Length; i++)
            {
                Transform target = _optionsButtons[i].transform;
                KillRunningAnimationsOn(target);
                target.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack);
            }
        }

        private float _optFadeIntensity;
        private Tween _optFadeTween;
        public void AnimateOptionsFade(float to)
        {
            if (_optFadeTween != null)
            {
                DOTween.Kill(_optFadeTween);
                _optFadeIntensity = _optionsCanvasGroup.alpha;
            }
            _optionsCanvasGroup.gameObject.SetActive(true);

            _optFadeTween = DOTween.To(() => _optFadeIntensity,
                    x => _optFadeIntensity = x, to, 0.3f)
                .OnUpdate(() => _optionsCanvasGroup.alpha = _optFadeIntensity);
            _optFadeTween.OnComplete(OnOptComplete);

            void OnOptComplete()
            {
                _optFadeTween = null;
                if (to == 0)
                    _optionsCanvasGroup.gameObject.SetActive(false);
            }
        }

        private float _mainFadeIntensity;
        private Tween _mainFadeTween;
        public void AnimateMainFade(float to)
        {
            if (_mainFadeTween != null)
            {
                DOTween.Kill(_mainFadeTween);
                _mainFadeIntensity = _mainCanvasGroup.alpha;
            }

            _mainFadeTween = DOTween.To(() => _mainFadeIntensity,
                    x => _mainFadeIntensity = x, to, _fadeDuration)
                .OnUpdate(() => _mainCanvasGroup.alpha = _mainFadeIntensity);
            _mainFadeTween.OnComplete(() => _mainFadeTween = null);
        }

        protected void KillRunningAnimationsOn(Transform target)
        {
            transform.DOKill();
        }

        public event Action<LayerImage> OnRemove;
    }
}