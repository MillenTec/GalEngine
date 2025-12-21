using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public class PathManager {
    public static string GetApplicationRootPath() {
        string rootPath;
        #if UNITY_EDITOR
            rootPath =  Application.dataPath;
        #else
            rootPath = Application.persistentDataPath;
        #endif
        return rootPath;
    }
    
    public static string GetPacksRootPath() {
        string rootPath;
        #if UNITY_EDITOR
            rootPath =  Application.dataPath + "/TestingPacks";
        #else
            rootPath = $"{Application.persistentDataPath}/Packs";
        #endif
        if (!Directory.Exists(rootPath)) {
            Directory.CreateDirectory(rootPath);
        }

        return rootPath;
    }
}

public class ExternalResourceLoader {
    public event Action OnRefreshPackList;
    
    public static string[] ListAllPackDir() {
        List<string> validDirList = new List<string>();
        string[] dirList =  Directory.GetDirectories(PathManager.GetPacksRootPath());
        foreach (var dir in dirList) {
            if (File.Exists($"{dir}/config.json")) {
                validDirList.Add(dir);
            }
        }

        return validDirList.ToArray();
    }

    public static JObject GetPackInformation(string packPath) {
        JObject config = new JObject();
        try {
            if (!File.Exists($"{packPath}/config.json")) {
                throw new Exception("ERROR: 此包没有Config文件");
            }

            string configText = File.ReadAllText($"{packPath}/config.json");
            if (configText == "") {
                throw new Exception("ERROR: 此包的Config文件为空");
            }

            config = JObject.Parse(configText);
            if (config["title"] == null) {
                throw new WarningException($"Warning: 包{packPath}缺少标题信息");
            } else if (config["author"] == null) {
                throw new WarningException($"WARNING: 包{packPath}缺少作者信息");
            } else if (config["id"] == null) {
                throw new Exception($"ERROR: 包{packPath}缺少唯一标识码(ID)");
            }
        } catch (WarningException warnEx) {
            Debug.LogWarning(warnEx);
            return config;
        } catch (Exception ex) {
            Debug.LogError(ex);
            return null;
        }

        return config;
    }

    public static Sprite LoadSpriteFromFile(string filePath) {
        try {
            if (!File.Exists(filePath)) {
                throw new Exception($"ERROR: 文件{filePath}不存在");
            }

            byte[] fileByte = File.ReadAllBytes(filePath);
            Texture2D texture = new(2, 2);
            bool isSuccess = texture.LoadImage(fileByte);
            if (!isSuccess) {
                Debug.LogError($"无法加载图片: {filePath}");
                return null;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            sprite.name = Path.GetFileNameWithoutExtension(filePath);
            return sprite;
        } catch (Exception ex) {
            Debug.LogError($"加载图片失败: {ex.Message}");
            return null;
        }
    }

    public JArray GetPlotJsonData(string filePath) {
        if (File.Exists(filePath) && Regex.IsMatch(filePath, @"\.(json | JSON)$")) {
            JArray jsonData;
            try {
                string fileText = File.ReadAllText(filePath);
                if (fileText == "") {
                    throw new Exception($"ERROR: 提供的文件{filePath}为空");
                }

                jsonData = JArray.Parse(fileText);
                return jsonData;
            } catch (Exception ex) {
                Debug.Log(ex);
                return null;
            }
        } else {
            Debug.Log($"ERROR: 提供的文件{filePath}不是JSON文件");
            return null;
        }
    }

    public void RefreshPackList() {
        OnRefreshPackList?.Invoke();
    }
}