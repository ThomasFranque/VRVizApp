﻿using System.Collections;
using System.Collections.Generic;
using ArchiveLoad;
using UnityEngine;
using VRWheel.Layer;

namespace VRWheel.Individuals
{
    public class DockLayerSlice : WheelButton
    {
        [SerializeField, Range(1, 4)] private int _sliceNumber = 1;
        [SerializeField] private Transform _spawnTransformsParent = default;
        [SerializeField] private DotPopulator _dotPopulator = default;
        public int SliceNumber => _sliceNumber;
        private Transform[] _spawnTransforms = default;
        private(Transform pos, LayerImage taken) [] _spawnpoints;

        protected override void OnInit()
        {
            SetupSpawnpoints();
            _type = WheelButtonType.Dock_Layer;
            OnOtherOpen += CheckType;
            OnOpen += EnableAll;
            OnOpen += _dotPopulator.Open;
            OnClose += DisableAll;
            OnClose += _dotPopulator.Close;
        }

        public void SetupSpawnpoints()
        {
            _spawnTransforms = new Transform[_spawnTransformsParent.childCount];
            _spawnpoints = new(Transform pos, LayerImage taken) [_spawnTransforms.Length];
            for (int i = 0; i < _spawnTransforms.Length; i++)
            {
                _spawnTransforms[i] = _spawnTransformsParent.GetChild(i);
                _spawnpoints[i] = (_spawnTransforms[i], null);
            }
        }

        public bool TryAddNewImage(LayerImage image, out Transform availableSlot)
        {
            availableSlot = default;
            for (int i = 0; i < _spawnpoints.Length; i++)
            {
                if (_spawnpoints[i].taken == null)
                {
                    availableSlot = _spawnpoints[i].pos;
                    _spawnpoints[i].taken = image;
                    image.OnRemove += RemoveImage;
                    return true;
                }
            }
            return false;
        }

        private void RemoveImage(LayerImage image)
        {
            for (int i = 0; i < _spawnpoints.Length; i++)
                if (_spawnpoints[i].taken == image)
                {
                    _spawnpoints[i].taken = null;
                    break;
                }
        }

        // Prevent more than one Layer slice open at the same time
        private void CheckType(WheelButton b)
        {
            // if (b.Type == Type)
            //     Close();
        }

        private void EnableAll()
        {
            foreach ((Transform pos, LayerImage taken) b in _spawnpoints)
                b.taken?.Showtime();
        }
        private void DisableAll()
        {
            foreach ((Transform pos, LayerImage taken) b in _spawnpoints)
                b.taken?.ShowtimeOver();
        }
    }
}