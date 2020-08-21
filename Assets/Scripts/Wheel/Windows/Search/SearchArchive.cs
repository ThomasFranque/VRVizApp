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
        public Transform ImageTransform => _image.transform;
        public ArchiveInfo AttachedInfo => _attachedInfo;
        public bool AddedToLayers { get; private set; }

        public void Initialize(ArchiveInfo info)
        {
            _attachedInfo = info;
            _image.sprite = info.Images.Full;
            _button.onClick.AddListener(AddToLayerIntention);
            _hasSBSimage.SetActive(info.HasSbs); 

            if (!string.IsNullOrWhiteSpace(info.NumberOriginal))
                _idPro.SetText(info.NumberOriginal);
            else
                _idPro.SetText(info.NumberRelvas);
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
                _button.interactable = false;
            }
        }

        public void RemovedFromLayers(LayerImage img)
        {
            AddedToLayers = false;
            _button.interactable = true;
        }
    }
}