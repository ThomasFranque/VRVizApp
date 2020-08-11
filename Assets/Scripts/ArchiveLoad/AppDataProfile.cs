using System;
using UnityEngine;

namespace ArchiveLoad
{
    [CreateAssetMenu(menuName = "VRViz/App Data Info", fileName = "Data Info")]
    public class AppDataProfile : ScriptableObject
    {
        [SerializeField]
        private Environment.SpecialFolder _appDataLocation = Environment.SpecialFolder.MyDocuments;
        [SerializeField] private string _dataFolderName = "VRViz_Data";
        [SerializeField] private string _imagesFolderName = "ImgStereoCards";
        [SerializeField] private string _sbsFolderName = "SBS";

        [SerializeField, Tooltip("File extension included")]
        private string _databaseName = "BaseDadosCarlosRelvas_VRViz.xlsx";

        public Environment.SpecialFolder AppDataLocation => _appDataLocation;
        public string DataFolderName => _dataFolderName;
        public string ImagesFolderName => _imagesFolderName;
        public string DatabaseName => _databaseName;
        public string SbsFolderName => _sbsFolderName;
    }
}