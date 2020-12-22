using System.Collections;
using System.Collections.Generic;
using ArchiveLoad;
using UnityEngine;

public class CtrlToggle : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Canvas _canvas = default;
    public ArchiveManager _loader = default;
    [Header("VR")]
    [SerializeField] private Camera _vrCanvasCam = default;
    [SerializeField] private GameObject _vrCtrlsObj = default;
    [Header("PC")]
    [SerializeField] private Camera _pcCanvasCam = default;
    [SerializeField] private GameObject _pcCtrlsObj = default;
    [Space]
    public bool IsInVR = true;

    public void ChangeToVRControls()
    {
        _canvas.worldCamera = _vrCanvasCam;
        _pcCtrlsObj.SetActive(false);
        _vrCtrlsObj.SetActive(true);
        IsInVR = true;
    }
    public void ChangeToPCControls()
    {
        _canvas.worldCamera = _pcCanvasCam;
        _vrCtrlsObj.SetActive(false);
        _pcCtrlsObj.SetActive(true);
        IsInVR = false;
    }

    public void ToggleLoader()
    {
        _loader.gameObject.SetActive(!_loader.gameObject.activeSelf);
    }
}