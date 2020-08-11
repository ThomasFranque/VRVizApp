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

        [Space]
        [SerializeField] private string _imageExtension = ".jpg";
        [SerializeField] private string _leftImageExtension = "_l.jpg";
        [SerializeField] private string _rightImageExtension = "_r.jpg";

        public Environment.SpecialFolder AppDataLocation => _appDataLocation;
        public string DataFolderName => _dataFolderName;
        public string ImagesFolderName => _imagesFolderName;
        public string DatabaseName => _databaseName;
        public string SbsFolderName => _sbsFolderName;

        public string ImageExtension => _imageExtension;
        public string LeftImageExtension => _leftImageExtension;
        public string RightImageExtension => _rightImageExtension;
    }
}