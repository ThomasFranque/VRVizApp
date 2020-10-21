using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGToggleManager : MonoBehaviour
{
    private const string LABEL_FORMAT = "{0}\n<SIZE=50%>{1}";
    private const string DEFAULT = "Garage";
    [SerializeField] private Toggle _defaultToggle = default;
    [SerializeField] private BGCollection _bgCollection = default;

    private Toggle[] _toggles;

    private void Awake()
    {
        _toggles = new Toggle[_bgCollection.Skyboxers.Count];

        int i = 0;
        foreach(SkyboxInfo inf in _bgCollection.Skyboxers)
        {
            Toggle toggle =
                Instantiate(_defaultToggle.gameObject, _defaultToggle.transform.parent).GetComponent<Toggle>();
            TextMeshProUGUI label = toggle.GetComponentInChildren<TextMeshProUGUI>();
            label.text = string.Format(LABEL_FORMAT, inf.Name, inf.Type.ToString());
            toggle.onValueChanged.AddListener((bool yes) => ChangeSkybox(inf, toggle));
            _toggles[i] = toggle;
            i++;
            if (inf.SkyboxMaterial == RenderSettings.skybox)
                toggle.SetIsOnWithoutNotify(true);
        }

        Destroy(_defaultToggle.gameObject);
    }

    private void ChangeSkybox(SkyboxInfo skyInf, Toggle toggle)
    {
        foreach(Toggle t in _toggles)
            if (t != toggle)
                t.SetIsOnWithoutNotify(false);
            else 
                t.SetIsOnWithoutNotify(true);
        RenderSettings.skybox = skyInf.SkyboxMaterial;
    }
}
