﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ArchiveLoad;

public class FloatingImageInfo : MonoBehaviour
{
    // All the fields we need to display info
        [SerializeField] private Image _photographImage = default;
        [SerializeField] private TMP_Text _ownerPro = default;
        [SerializeField] private TMP_Text _typePro = default;
        [SerializeField] private TMP_Text _topicPro = default;
        [SerializeField] private TMP_Text _physicalDescrPro = default;
        [SerializeField] private TMP_Text _yearPro = default;
        [SerializeField] private TMP_Text _idPro = default;

        // Stores the feeded info
        public ArchiveInfo _targetinfo;

        // The good old reliable Awake 
        private void Awake()
        {
            // Prevent users from seeing the defaults
            ToggleFields(false);
        }

        // Initialize the object with provided info, assigning all the necessary
        // info to their respective fields.
        public void Init(ArchiveInfo info)
        {
            // Store the given info
            _targetinfo = info;

            // Update the fields using given info
            UpdateFields(_targetinfo);

            // Enable the fields again already with the feeded info (disabled on Awake)
            ToggleFields(true);
        }

        // Update all the fields with the given info
        private void UpdateFields(ArchiveInfo info)
        {
            // Bla bla bla assign stuff
            if (_photographImage != null)
                _photographImage.sprite = info.Image;

            if (_ownerPro != null)
                _ownerPro.text = info.Owner;
            if (_typePro != null)
                _typePro.text = "Archive info still has no field for the type"; //! Change later
            if (_topicPro != null)
                _topicPro.text = info.Topic;
            if (_physicalDescrPro != null)
                _physicalDescrPro.text = info.PhysicalDescription;
            if (_yearPro != null)
                _yearPro.text = ParseYear();
            if (_idPro != null)
                _idPro.text = string.IsNullOrWhiteSpace(info.NumberOriginal) ? info.NumberRelvas : info.NumberOriginal;

            // Get the pretty year string from info
            string ParseYear() => $"{info.StartYear}\n-\n{info.EndYear}";
        }

        // Used to toggle every field to the desired state.
        private void ToggleFields(bool active)
        {
            // This script is used both for basic info and detailed info.
            // Basic info doesn't have all the fields, so to prevent errors,
            // check if they are not null before messing with them.

            if (_photographImage != null)
                _photographImage.enabled = active;

            if (_ownerPro != null)
                _ownerPro.enabled = active;

            if (_ownerPro != null)
                _typePro.enabled = active;

            if (_topicPro != null)
                _topicPro.enabled = active;

            if (_physicalDescrPro != null)
                _physicalDescrPro.enabled = active;

            if (_yearPro != null)
                _yearPro.enabled = active;
        }
}
