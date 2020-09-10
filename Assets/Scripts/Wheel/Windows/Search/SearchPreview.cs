using ArchiveLoad;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace VRWheel.Windows.Search
{
    public class SearchPreview : MonoBehaviour
    {
        [SerializeField] private Image _previewImage = default;

        private SearchArchive _archive;
        private ArchiveInfo _archiveInfo;
        private float _alphaTracker;
        public void Initialize(SearchArchive archive, ArchiveInfo info)
        {
            _archive = archive;
            _archiveInfo = info;
            _previewImage.sprite = info.Image;
            transform.position = archive.Window.transform.position + (-archive.Window.transform.forward * 0.2f);
            transform.forward = archive.transform.forward;

            transform.localScale = Vector3.one * 0.1f;
            transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutCirc);
            DOTween.To(() => _alphaTracker,
                    x => _alphaTracker = x, 1, 0.35f)
                .OnUpdate(UpdateAlpha);
        }

        private void UpdateAlpha()
        {
            Color c = Color.white;
            c.a = _alphaTracker;
            _previewImage.color = c;
        }

        public void Close()
        {
            _archive.PreviewEnd();
            Destroy(gameObject);
        }
        public void Save()
        {
            _archive.AddToLayerIntention();
            
            transform.DOLocalMoveX(transform.localPosition.x + 100f, 0.4f);
            DOTween.To(() => _alphaTracker,
                    x => _alphaTracker = x, 0, 0.35f)
                .OnUpdate(UpdateAlpha).OnComplete(Close);
        }
    }
}