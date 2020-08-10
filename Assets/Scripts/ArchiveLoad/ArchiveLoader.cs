using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using ExcelDataReader;

namespace ArchiveLoad
{
    public class ArchiveLoader
    {
        private const string _DB_FILE_NAME = "VRViz_Data/BaseDadosCarlosRelvas_VRViz.xlsx";

        private const string _IMG_FOLDER_NAME = "VRViz_Data/imgsStereoCards";

        private readonly string _dbFilePath;

        private readonly string _imgFolderPath;

        private System.Data.DataSet resultTable;

        private float result;

        public List<ArchiveInfo> ArchivesInfo { get; }

        public ArchiveLoader()
        {
            _dbFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                _DB_FILE_NAME);

            _imgFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                _IMG_FOLDER_NAME);

            if (_dbFilePath != null)
                Debug.Log($"{_DB_FILE_NAME} found in {_dbFilePath}.");
            else
            {
                Debug.Log($"An error occurred!");
                return;
            }

            if (_imgFolderPath != null)
                Debug.Log($"{_IMG_FOLDER_NAME} found in {_imgFolderPath}.");
            else
            {
                Debug.Log($"An error occurred!");
                return;
            }

            ArchivesInfo = new List<ArchiveInfo>();

            ArchivesInfo = GetInfo();

            if (ArchivesInfo != null)
                Debug.Log($"Files successfully read.");
            else
            {
                Debug.Log($"An error occurred!");
                return;
            }
        }

        private List<ArchiveInfo> GetInfo()
        {
            List<ArchiveInfo> archInf = new List<ArchiveInfo>();

            float id = 0;
            string metadata = null;
            string imageSize = null;
            string description = null;
            string numberRelvas = null;
            string numberOriginal = null;
            string owner = null;
            string theme = null;
            string subject = null;
            string date = null;

            int x = 0;
            int y = 0;

            using (FileStream stream = File.Open(_dbFilePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    resultTable = reader.AsDataSet();
                }
            }

            DirectoryInfo dir = new DirectoryInfo(_imgFolderPath);
            FileInfo[] jpgs = dir.GetFiles("*.jpg");
            int i = 0;

            for (y = 3; y < resultTable.Tables[0].Rows.Count; y++)
            {
                if (y > 2 && y < 21)
                {
                    for (x = 0; x < resultTable.Tables[0].Columns.Count; x++)
                    {
                        switch (x)
                        {
                            case 0:
                                float.TryParse(
                                    resultTable.Tables[0].Rows[y][x].ToString(),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out result);

                                id = result;

                                break;

                            case 1:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    metadata = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    metadata = "";

                                break;

                            case 2:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    imageSize = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    imageSize = "";

                                break;

                            case 3:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    description = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    description = "";

                                break;

                            case 5:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    numberRelvas = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    numberRelvas = "";

                                break;

                            case 6:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    numberOriginal = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    numberOriginal = "";

                                break;

                            case 7:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    owner = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    owner = "";

                                break;

                            case 9:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    theme = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    theme = "";

                                break;

                            case 11:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    subject = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    subject = "";

                                break;

                            case 12:
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    date = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    date = "";

                                break;
                        }
                    }
                }

                float width = 0;

                float height = 0;

                if (imageSize != null)
                {
                    string[] sizes = imageSize.Split('x');

                    width = float.Parse(sizes[0], CultureInfo.InvariantCulture.NumberFormat);

                    height = float.Parse(sizes[1], CultureInfo.InvariantCulture.NumberFormat);
                }

                int startYear = 0;

                int endYear = 0;

                if (date != null)
                {
                    string[] dates = date.Split('-');

                    startYear = int.Parse(dates[0], CultureInfo.InvariantCulture.NumberFormat);

                    endYear = int.Parse(dates[1], CultureInfo.InvariantCulture.NumberFormat);
                }

                Sprite img = null;

                if (i < jpgs.Length - 1 && jpgs[i] != null)
                    img = LoadNewSprite(jpgs[i].FullName);

                ArchiveInfo ai =
                    new ArchiveInfo(
                        id,
                        metadata,
                        img, 
                        width, 
                        height, 
                        startYear, 
                        endYear, 
                        theme, 
                        owner, 
                        description);

                archInf.Add(ai);

                i++;
            }

            //loadingCanvas.OnLoadingFinished?.Invoke();

            return archInf;
        }

        private Sprite LoadNewSprite(
            string FilePath, 
            float PixelsPerUnit = 100.0f)
        {
            Texture2D SpriteTexture = LoadTexture(FilePath);
            Sprite NewSprite = Sprite.Create(
                SpriteTexture, 
                new Rect(
                    0, 
                    0, 
                    SpriteTexture.width, 
                    SpriteTexture.height), 
                    new Vector2(0, 0), 
                    PixelsPerUnit);

            return NewSprite;
        }

        private Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                    return Tex2D;
            }
            return null;
        }
    }
}
