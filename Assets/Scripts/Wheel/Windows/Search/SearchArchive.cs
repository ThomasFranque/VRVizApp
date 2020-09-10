using ArchiveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRWheel.Layer;
using VRWheel.Windows.Layers;

namespace VRWheel.Windows.Search
{
    public class SearchArchive : MonoBehaviour
    {
        [SerializeField] private Image _image = default;
        [SerializeField] private GameObject _hasSBSimage = default;
        [SerializeField] private Button _button = default;
        [SerializeField] private TextMeshProUGUI _idPro = default;
        private ArchiveInfo _attachedInfo;
        private SearchWindow _window;
        public Transform ImageTransform => _image.transform;
        public ArchiveInfo AttachedInfo => _attachedInfo;
        public bool AddedToLayers { get; private set; }
        public SearchWindow Window => _window;
        public void Initialize(ArchiveInfo info, SearchWindow window)
        {
            _attachedInfo = info;
            _image.sprite = info.Images.Full;
            _button.onClick.AddListener(Preview);
            _hasSBSimage.SetActive(info.HasSbs);
            _window = window;

            if (!string.IsNullOrWhiteSpace(info.NumberOriginal))
                _idPro.SetText(info.NumberOriginal);
            else
                _idPro.SetText(info.NumberRelvas);
        }

        public void Preview()
        {
            _window.Previewing();
            SearchPreview prev = Instantiate(SearchWindow.PreviewPrefab, _window.transform).GetComponent<SearchPreview>();
            prev.Initialize(this, AttachedInfo);
            // instantiate prefab get component
            // init
            // ree
            // AddToLayerIntention();
        }

        public void PreviewEnd()
        {
            _window.PreviewStop();
        }

        public void AddToLayerIntention()
        {
            if (AddedToLayers) return;

            AddToLayers();
        }

        private void AddToLayers()
        {
            if (LayerWindowManager.Instance.TryAdd(this))
            {
                AddedToLayers = true;
                DisableSelection();
            }
        }

        public void RemovedFromLayers(LayerImage img)
        {
            AddedToLayers = false;
            _button.interactable = true;
        }

        public void DisableSelection()
        {
            _button.interactable = false;
        }

        public void EnableSelection()
        {
            _button.interactable = !AddedToLayers;
        }
    }
}