using ArchiveLoad;
using UnityEngine;
using VRWheel.Individuals;
using VRWheel.Layer;
using VRWheel.Windows.Search;

namespace VRWheel.Windows.Layers
{
    public class LayerWindowManager : MonoBehaviour
    {
        [SerializeField] private DockLayerSlice[] _layerButtons = default;
        [SerializeField] private GameObject _layerImagePrefab = default;

        public static LayerWindowManager Instance { get; private set; }

        private GameObject[, ] _addedLayerImages;

        private int addedImages;

        private void Awake()
        {
            Instance = this;
        }

        public bool TryAdd(SearchArchive searchArchive)
        {
            ArchiveInfo info = searchArchive.AttachedInfo;
            LayerImage lImg = Instantiate(_layerImagePrefab, transform).GetComponent<LayerImage>();
            for (int i = 0; i < _layerButtons.Length; i++)
            {
                Transform point;
                DockLayerSlice slice = _layerButtons[i];
                if (slice.TryAddNewImage(lImg, out point))
                {
                    lImg.transform.position = point.position;
                    lImg.Initialize(info);
                    lImg.OnRemove += searchArchive.RemovedFromLayers;
                    slice.Open();
                    return true;
                }
            }
            Destroy(lImg.gameObject);

            return false;
        }
    }
}