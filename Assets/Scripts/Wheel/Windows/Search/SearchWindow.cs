using System.Collections;
using System.Collections.Generic;
using ArchiveLoad;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VRWheel.Windows.Search
{
    public class SearchWindow : Window, ICollectionLoadedListener
    {
        [SerializeField] private Transform[] _separatorBars = default;
        [SerializeField] private Transform[] _barsContentHolders = default;
        [SerializeField] private Transform _backgroundPanel = default;
        [SerializeField] private GameObject _prefab = default;
        public static GameObject PreviewPrefab { get; private set; }

        private List<SearchArchive> _archives;

        private SearchArchive[, ] _animatedArchives;
        private readonly float _barsClosedXScale = 0.001f;

        protected override void OnAwake()
        {
            PreviewPrefab = Resources.Load<GameObject>("Prefabs/SearchPreview");
        }

        protected override void OnOpen()
        {
            AnimatePanel(Vector3.one, true);
            AnimateBars(1, true);
            AnimateArchives(Vector3.one, 0.04f, Ease.OutBack, 0.5f, true);
        }
        protected override void OnClose()
        {
            AnimatePanel(Vector3.one * 0.001f, false);
            AnimateBars(_barsClosedXScale, false);
            AnimateArchives(Vector3.one * 0.001f, 0, Ease.InCirc, 0.2f, false);
        }

        private void AnimatePanel(Vector3 newPanelScale, bool enabledState)
        {
            if (enabledState) _backgroundPanel.gameObject.SetActive(enabledState);
            KillRunningAnimationsOn(_backgroundPanel);
            _backgroundPanel.DOScale(newPanelScale, 0.2f).SetEase(Ease.OutCirc)
                .OnComplete(() => _backgroundPanel.gameObject.SetActive(enabledState));
        }

        private void AnimateBars(float newXValue, bool enabledState)
        {
            for (int i = 0; i < _separatorBars.Length; i++)
            {
                Transform target = _separatorBars[i];
                if (enabledState) target.gameObject.SetActive(enabledState);
                KillRunningAnimationsOn(target);
                target.DOScaleX(newXValue, 0.5f).SetEase(Ease.OutCirc)
                    .OnComplete(() => target.gameObject.SetActive(enabledState));
            }
        }
        private void AnimateArchives(Vector3 newValue, float delay, Ease ease, float speed, bool enabledState)
        {
            if (_animatedArchives == default) return;
            for (int barI = 0; barI < _barsContentHolders.Length; barI++)
            {
                int length = _animatedArchives.GetLength(1);
                for (int i = 0; i < length; i++)
                {
                    if (_animatedArchives[barI, i] == default) continue;
                    Transform target = _animatedArchives[barI, i].ImageTransform;
                    if (enabledState) target.gameObject.SetActive(enabledState);

                    KillRunningAnimationsOn(target);
                    target.DOScale(newValue, speed)
                        .SetEase(ease)
                        .SetDelay(delay * i)
                        .OnComplete(() => target.gameObject.SetActive(enabledState));
                }
            }
        }

        public void CollectionLoaded()
        {
            Populate();
            OnClose();
        }

        public void Previewing()
        {
            foreach(SearchArchive a in _archives) a.DisableSelection();
        }

        public void PreviewStop()
        {
            foreach(SearchArchive a in _archives) a.EnableSelection();
        }

        private void Populate()
        {
            int amountPerBar = ArchiveManager.Archives.Length / _barsContentHolders.Length;
            _animatedArchives = new SearchArchive[_barsContentHolders.Length, amountPerBar];
            _archives = new List<SearchArchive>(ArchiveManager.Archives.Length);
            int barIndex = 0;
            for (int i = 0; i < ArchiveManager.Archives.Length; i++)
            {
                if (i % amountPerBar == 0 && i != 0)
                    barIndex++;

                ArchiveInfo inf = ArchiveManager.Archives[i];
                Transform parent = _barsContentHolders[barIndex % 3];
                SearchArchive newArchive;
                newArchive = Instantiate(_prefab, parent).GetComponent<SearchArchive>();
                newArchive.Initialize(inf, this);
                _animatedArchives[barIndex % 3, i % amountPerBar] = newArchive;
                _archives.Add(newArchive);
            }
        }
    }
}