using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExcelDataReader;
using UnityEngine;

namespace ArchiveLoad
{
    public class ArchiveLoader
    {
        private readonly string _dbFilePath;

        private readonly string _imgFolderPath;
        private readonly string _sbsFolderPath;

        private System.Data.DataSet resultTable;

        private float result;

        public List<ArchiveInfo> ArchivesInfo { get; }

        public ArchiveLoader(AppDataProfile dataInfo)
        {
            // Initialize variables
            _dbFilePath = Path.Combine(
                Environment.GetFolderPath(dataInfo.AppDataLocation),
                dataInfo.DatabaseName);

            _imgFolderPath = Path.Combine(
                Environment.GetFolderPath(dataInfo.AppDataLocation),
                dataInfo.ImagesFolderName);

            _sbsFolderPath = Path.Combine(
                _imgFolderPath,
                dataInfo.SbsFolderName);

            if (_dbFilePath != null)
                Debug.Log($"{dataInfo.DatabaseName} found in {_dbFilePath}.");
            else
            {
                Debug.LogError($"An error occurred!\nDatabase file not found.");
                return;
            }

            if (_imgFolderPath != null)
                Debug.Log($"{dataInfo.ImagesFolderName} found in {_imgFolderPath}.");
            else
            {
                Debug.LogError($"An error occurred!\nImages directory not found.");
                return;
            }

            // Load infos
            ArchivesInfo = new List<ArchiveInfo>();

            ArchivesInfo = GetArchiveInfoCollection();

            if (ArchivesInfo != null)
                Debug.Log($"Files successfully read.");
            else
            {
                Debug.LogError($"An error occurred! Info loading was not successful.");
                return;
            }
        }

        private List<ArchiveInfo> GetArchiveInfoCollection()
        {
            List<ArchiveInfo> loadedArchiveInfos = new List<ArchiveInfo>();

            float id = 0;
            string metadata = null;
            string imageSize = null;
            string physicalDescription = null;
            string numberRelvas = null;
            string numberOriginal = null;
            string owner = null;
            string theme = null;
            string subject = null;
            string date = null;

            int x = 0;
            int y = 0;

            using(FileStream stream = File.Open(_dbFilePath, FileMode.Open, FileAccess.Read))
            {
                using(IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
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
                            case 0: // ID [A]
                                float.TryParse(
                                    resultTable.Tables[0].Rows[y][x].ToString(),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out result);

                                id = result;
                                break;

                            case 1: // Metadata [B]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    metadata = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    metadata = "";
                                break;

                            case 2: // Image Size [C]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    imageSize = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    imageSize = "";
                                break;

                            case 4: // Physical Description (Eng) [E]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    physicalDescription = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    physicalDescription = "";
                                break;

                            case 5: // Number Relvas Catalogue [F]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    numberRelvas = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    numberRelvas = "";
                                break;

                            case 6: // Owner Number [G]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    numberOriginal = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    numberOriginal = "";
                                break;

                            case 7: // Owner [H]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    owner = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    owner = "";
                                break;

                            case 9: // Theme (Eng) [J]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    theme = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    theme = "";
                                break;

                            case 11: // Subject (Eng) [L]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    subject = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    subject = "";
                                break;

                            case 12: // Date [M]
                                if (resultTable.Tables[0].Rows[y][x].ToString() != null)
                                    date = resultTable.Tables[0].Rows[y][x].ToString();
                                else
                                    date = "";
                                break;
                            case 13: // Inscriptions (Eng) [O]
                                break;
                            case 14: // Backside [Verso: P]
                                break;
                            case 16: // Series (Eng) [R]
                                break;
                            case 18: // Other proof [T]
                                break;
                            case 19: // Related [U]
                                break;
                            case 21: // Notes (Eng) [W]
                                break;
                        }
                    }
                }

                // Get size
                float width = 0;
                float height = 0;

                if (imageSize != null)
                {
                    string[] sizes = imageSize.Split('x');

                    width = float.Parse(sizes[0], CultureInfo.InvariantCulture.NumberFormat);
                    height = float.Parse(sizes[1], CultureInfo.InvariantCulture.NumberFormat);
                }

                // Get year
                int startYear = 0;
                int endYear = 0;

                if (date != null)
                {
                    string[] dates = date.Split('-');

                    startYear = int.Parse(dates[0], CultureInfo.InvariantCulture.NumberFormat);

                    endYear = int.Parse(dates[1], CultureInfo.InvariantCulture.NumberFormat);
                }

                // Get the images
                Sprite img = null;

                if (i < jpgs.Length - 1 && jpgs[i] != null)
                    img = LoadNewSprite(jpgs[i].FullName);

                // Finalize archive info creation
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
                        physicalDescription);

                loadedArchiveInfos.Add(ai);

                i++;
            }

            //loadingCanvas.OnLoadingFinished?.Invoke();

            return loadedArchiveInfos;
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