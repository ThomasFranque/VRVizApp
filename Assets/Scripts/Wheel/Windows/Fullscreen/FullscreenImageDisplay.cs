using ArchiveLoad;
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
        [SerializeField] private Transform _positionTarget = default;

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
            ToggleSBSImages(false);
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetMouseButtonDown(0))
                Close();
        }

        private void AlignAndMove(Transform subject, Transform referenceTarget)
        {
            subject.transform.forward = referenceTarget.transform.forward;
            subject.transform.position = referenceTarget.transform.position;
        }

        private void InstanceDisplayDual(ArchiveInfo info)
        {
            _open = true;
            AnimateFade(1);
            _zoomImage.color = transparent;
            _leftDualImage.color = Color.white;
            _rightDualImage.color = Color.white;

            ToggleSBSImages(true);

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
            ToggleSBSImages(false);
            if (!_open) return;

            _open = false;
            AnimateFade(0);
        }

        private void ToggleSBSImages(bool value)
        {
            AlignAndMove(_rightDualImage.transform, _positionTarget);
            AlignAndMove(_leftDualImage.transform, _positionTarget);

            _rightDualImage.enabled = value;
            _leftDualImage.enabled = value;
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