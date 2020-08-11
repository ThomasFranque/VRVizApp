using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveLoad
{
    public class ArchiveManager : MonoBehaviour
    {
        private Texture2D[] _textures;
        public static ArchiveInfo[] Archives { get; private set; }

        [SerializeField] private AppDataProfile _dataInfoProfile = default;
        [SerializeField] private GameObject _blocker = default;
        [SerializeField, Tooltip("Needs to have a component that implements ICollectionLoaderListener!")]
        private GameObject[] _listeners = default;

        private void Awake()
        {
            _blocker.SetActive(true);
            for (int i = 0; i < _listeners.Length; i++)
            {
                ICollectionLoadedListener listener;
                listener = _listeners[i].GetComponent<ICollectionLoadedListener>();
                if (listener != null)
                    OnCollectionLoaded += listener.CollectionLoaded;
                else
                    Debug.LogWarning($"Assigned listener \"{_listeners[i].name}\" does not have a component that implements ICollectionLoaderListener!");
            }
            Load();
        }

        private void Load()
        {
            StartCoroutine(LoadAll());
        }

        private IEnumerator LoadAll()
        {
            //ArchiveLoader al = new ArchiveLoader(_loadingCanvas);
            ArchiveLoader al = new ArchiveLoader(_dataInfoProfile);
            List<ArchiveInfo> archiveInfos = al.InfoCollection;

            //_loadingCanvas?.SetFilesToLoad(archiveInfos.Count);

            _textures = new Texture2D[archiveInfos.Count];

            string pathPreFix = @"file://";

            int i = 0;

            foreach (ArchiveInfo ai in archiveInfos)
            {
                yield return null;

                if (ai.Image != null)
                {
                    string pathTemp = pathPreFix + ai.Image.name;
                    WWW www = new WWW(pathTemp);
                    yield return www;
                    while (!www.isDone)
                        yield return null;
                    _textures[i] = www.texture;
                }
                else
                {
                    _textures[i] = null;
                }

                i++;

                //_loadingCanvas?.NewFilesLoaded();
            }

            Archives = archiveInfos.ToArray();
            //SpawnArchive(archiveInfos);
            _blocker.SetActive(false);

            foreach (ArchiveInfo ai in Archives)
                Debug.Log(ai);
            OnCollectionLoaded?.Invoke();
        }

        private void SpawnArchive(List<ArchiveInfo> archiveInfos)
        {
            // Transform _currentPos = _reference;

            // foreach (ArchiveInfo ai in archiveInfos)
            // {
            //     GameObject obj = 
            //         Instantiate(
            //             _prefab, 
            //             _currentPos.position, 
            //             _prefab.transform.rotation, 
            //             _parent);

            //     ArchiveDisplay arch = obj.GetComponent<ArchiveDisplay>();

            //     arch.Init(ai);

            //     Debug.Log($"Added: {ai.Id}");

            //     _currentPos.position += 
            //         _reference.transform.forward * 0.5f;
            // }
        }

        private void SpawnDetailedArchive()
        {
            // DestroyLast();

            // GameObject obj =
            //     Instantiate(
            //         _detailedPrefab,
            //         _detailedArchiveParent.position,
            //         _prefab.transform.rotation,
            //         _detailedArchiveParent);

            // ArchiveDisplay arch = obj.GetComponent<ArchiveDisplay>();

            // ArchiveInfo ai = _currentSelected.GetComponent<ArchiveDisplay>()._targetinfo;

            // arch.Init(ai);
        }

        private void Confirm()
        {
            // _currentSelected.transform.parent = 
            //     _archiveParents[_currentPos].transform;

            // _currentSelected.GetComponent<Archive>()
            //     .ConfirmPos(_archiveParents[_currentPos].transform.position);

            // _currentPos++;

            // if (_currentPos > _archiveParents.Length - 1)
            // {
            //     _currentPos = 0;
            // }
        }

        private void DestroyLast()
        {
            // GameObject obj = null;

            // if (_detailedArchiveParent.childCount > 0)
            //     obj = _detailedArchiveParent.GetChild(0).gameObject;

            // if (obj != null)
            //     Destroy(obj);
        }
        private Action OnCollectionLoaded;
    }
}