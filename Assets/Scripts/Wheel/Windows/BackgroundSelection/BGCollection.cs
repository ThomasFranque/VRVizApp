using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VRViz/Background Collection")]
public class BGCollection : ScriptableObject
{
    [SerializeField] private List<SkyboxInfo> _skyboxes = default;

    public List<SkyboxInfo> Skyboxers => _skyboxes;
}

[System.Serializable]
public struct SkyboxInfo 
{
    [SerializeField] private string _name;
    [SerializeField] private SkyboxType _type;
    [SerializeField] private Material _skyboxMaterial;

    public SkyboxType Type => _type;
    public string Name => _name;
    public Material SkyboxMaterial => _skyboxMaterial;
}

public enum SkyboxType
{
    Abstract,
    Indoors,
    Outdoors,
}
