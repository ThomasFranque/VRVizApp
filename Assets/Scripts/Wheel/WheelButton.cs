using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VRWheel
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public abstract class WheelButton : MonoBehaviour
    {
        private const float INTERACTION_DELAY = .2f;

        [Header("Animation")]
        [SerializeField] private RectTransform _icon = default;
        [SerializeField] private Transform _openPosition = default;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _moveMod = 1.2f;
        [SerializeField] private float _scaleMod = 1.5f;
        private Vector2 _closedIconLocalPosition;
        private Vector2 _openIconLocalPosition;
        private Vector2 _initialLocalPosition;
        private Vector3 _initialScale;
        [Header("Wheel Properties")]
        [SerializeField] protected WheelButtonType _type = default;
        private Button _button;
        private float _interactionDelayCounter;
        public WheelButtonType Type => _type;
        protected Wheel MainWheel { get; private set; }

        // Know if it is opened
        public bool Opened { get; private set; }
        public bool OnDelayCooldown => _interactionDelayCounter <= 0;

        // Called on the main wheel's Start()
        public void INIT(Wheel wheel)
        {
            MainWheel = wheel;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Toggle);
            _closedIconLocalPosition = _icon.localPosition;
            _openIconLocalPosition = _openPosition.localPosition;
            _initialScale = transform.localScale;
            _initialLocalPosition = transform.localPosition;
            OnInit();
        }

        // Called after the parent class has been setup
        protected virtual void OnInit() { }

        #region  Open and Closing management
        // Called on button click
        private void Toggle()
        {
            if (Opened) Close();
            else Open();
        }

        // Call to open 
        public void Open()
        {
            if (Opened || !OnDelayCooldown) return;
            _interactionDelayCounter = INTERACTION_DELAY;
            Opened = true;
            DoOpenAnimations();

            OnOpen?.Invoke();
            MainWheel.ButtonOpened(this);
        }

        // Call to close
        public void Close()
        {
            if (!Opened || !OnDelayCooldown) return;
            _interactionDelayCounter = INTERACTION_DELAY;
            Opened = false;
            DoCloseAnimations();

            OnClose?.Invoke();
            MainWheel.ButtonClosed(this);
        }
        #endregion

        protected virtual void Update()
        {
            _interactionDelayCounter = Mathf.Clamp(_interactionDelayCounter - Time.deltaTime, 0, float.MaxValue);
        }

        #region Animation
        private void DoOpenAnimations()
        {
            KillRunningAnimations();
            _icon.DOLocalMove(_openIconLocalPosition, _duration).SetEase(Ease.OutCirc);
            transform.DOLocalMove(_initialLocalPosition / _moveMod, _duration).SetEase(Ease.OutBack);
            transform.DOScale(_initialScale / _scaleMod, _duration / 1.8f);
        }

        private void DoCloseAnimations()
        {
            KillRunningAnimations();
            _icon.DOLocalMove(_closedIconLocalPosition, _duration).SetEase(Ease.OutCirc);
            transform.DOLocalMove(_initialLocalPosition, _duration).SetEase(Ease.OutBack);
            transform.DOScale(_initialScale, _duration / 1.8f);
        }

        private void KillRunningAnimations()
        {
            _icon.DOKill();
            transform.DOKill();
        }
        #endregion

        #region Events
        public void OnOtherButtonOpen(WheelButton wheelButton)
        {
            OnOtherOpen?.Invoke(wheelButton);
        }

        public void OnOtherButtonClose(WheelButton wheelButton)
        {
            OnOtherClose?.Invoke(wheelButton);
        }
        protected Action<WheelButton> OnOtherOpen;
        protected Action<WheelButton> OnOtherClose;
        public event Action OnOpen;
        public event Action OnClose;
        #endregion
    }
}