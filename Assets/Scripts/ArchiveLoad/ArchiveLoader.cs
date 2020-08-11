using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private AppDataProfile _dataInfo;
        public List<ArchiveInfo> InfoCollection { get; }

        public ArchiveLoader(AppDataProfile dataInfo)
        {
            _dataInfo = dataInfo;

            // Initialize variables
            _dbFilePath = Path.Combine(
                Environment.GetFolderPath(dataInfo.AppDataLocation),
                dataInfo.DataFolderName,
                dataInfo.DatabaseName);

            _imgFolderPath = Path.Combine(
                Environment.GetFolderPath(dataInfo.AppDataLocation),
                dataInfo.DataFolderName,
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
            InfoCollection = new List<ArchiveInfo>();
            InfoCollection = LoadInfoCollection();

            if (InfoCollection != null)
                Debug.Log($"Files successfully read.");
            else
            {
                Debug.LogError($"An error occurred! Info loading was not successful.");
                return;
            }
        }

        private List<ArchiveInfo> LoadInfoCollection()
        {
            List<ArchiveInfo> loadedArchiveInfos = new List<ArchiveInfo>();
            Dictionary<string, ArchiveImages> imagesCollection = LoadImages();

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
            ArchiveImages images;

            using(FileStream stream = File.Open(_dbFilePath, FileMode.Open, FileAccess.Read))
            {
                using(IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    resultTable = reader.AsDataSet();
                }
            }

            int i = 0;
            // Rows
            for (int y = 3; y < resultTable.Tables[0].Rows.Count; y++)
            {
                // If within limits
                if (y > 2 && y < 21)
                {
                    // Columns
                    for (int x = 0; x < resultTable.Tables[0].Columns.Count; x++)
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

                if (!imagesCollection.TryGetValue(numberOriginal, out images))
                {
                    Debug.LogWarning($"{numberOriginal} does not have images!");
                }

                //Sprite img = null;

                // if (i < jpgs.Length - 1 && jpgs[i] != null)
                //     img = LoadSpriteFromFile(jpgs[i]);

                // Finalize archive info creation
                ArchiveInfo ai =
                    new ArchiveInfo(
                        id,
                        metadata,
                        width,
                        height,
                        startYear,
                        endYear,
                        theme,
                        owner,
                        physicalDescription,
                        images);

                loadedArchiveInfos.Add(ai);

                i++;
            }

            //loadingCanvas.OnLoadingFinished?.Invoke();

            return loadedArchiveInfos;
        }

        private Dictionary<string, ArchiveImages> LoadImages()
        {

            DirectoryInfo mainImageDirInf = new DirectoryInfo(_imgFolderPath);
            DirectoryInfo sbsImageDirInf = new DirectoryInfo(_sbsFolderPath);
            FileInfo[] mains = mainImageDirInf.GetFiles("*.jpg");
            FileInfo[] sbs = sbsImageDirInf.GetFiles("*.jpg");

            Dictionary<string, ArchiveImages> loadedImages =
                new Dictionary<string, ArchiveImages>(mains.Length);

            for (int i = 0; i < mains.Length; i++)
            {
                FileInfo full = mains[i];
                FileInfo left;
                FileInfo right;
                ArchiveImages images;

                Sprite fSprite = default;
                Sprite lSprite = default;
                Sprite rSprite = default;

                string parsedName = full.Name.Remove(full.Name.Length - _dataInfo.ImageExtension.Length);

                FileInfo[] sbsLinks =
                    (from file in sbs where file.Name.Contains(parsedName) select file)
                    .ToArray();

                left = sbsLinks.FirstOrDefault(l => l.Name.EndsWith(_dataInfo.LeftImageExtension));
                right = sbsLinks.FirstOrDefault(r => r.Name.Contains(_dataInfo.RightImageExtension));

                fSprite = LoadSpriteFromFile(full);

                if (lSprite != null)
                    lSprite = LoadSpriteFromFile(left);
                else
                    Debug.LogWarning($"{full.Name} does not have a Left Side image!");

                if (rSprite != null)
                    rSprite = LoadSpriteFromFile(right);
                else
                    Debug.LogWarning($"{full.Name} does not have a Right Side image!");

                images = new ArchiveImages(fSprite, lSprite, rSprite);

                loadedImages.Add(parsedName, images);
            }

            return loadedImages;

            // if (i < jpgs.Length - 1 && jpgs[i] != null)
            //     img = LoadSpriteFromFile(jpgs[i]);
        }

        private Sprite LoadSpriteFromFile(FileInfo file, float pixelsPerUnit = 100.0f)
        {
            Texture2D spriteTexture = LoadTexture(file.FullName);
            Sprite newSprite = Sprite.Create(
                spriteTexture,
                new Rect(
                    0,
                    0,
                    spriteTexture.width,
                    spriteTexture.height),
                new Vector2(0, 0),
                pixelsPerUnit);

            return newSprite;
        }

        private Texture2D LoadTexture(string fullFilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(fullFilePath))
            {
                FileData = File.ReadAllBytes(fullFilePath);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                    return Tex2D;
            }
            return null;
        }
    }
}