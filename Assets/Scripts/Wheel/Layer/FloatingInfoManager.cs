using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRWheel.Layer;

public class FloatingInfoManager : MonoBehaviour
{
    public static FloatingInfoManager Instance;
    [SerializeField] private GameObject _prefab = default;
    [SerializeField] private Transform _floatingParent = default;
    [SerializeField] private Transform _stationarySpawnPoint = default;

    private void Awake()
    {
        Instance = this;
    }

    public FloatingImageInfo CreateInfo(ArchiveLoad.ArchiveInfo info, LayerImage img)
    {
        FloatingImageInfo floatingInf = 
            Instantiate(_prefab, _stationarySpawnPoint).GetComponent<FloatingImageInfo>();

        floatingInf.Init(info, img);
        return floatingInf;
    }

    public void Pin(FloatingImageInfo fImage)
    {

    }
}