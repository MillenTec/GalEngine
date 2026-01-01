using System.IO;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class PathManager {
    public static string GetApplicationRootPath() {
        string rootPath;
        #if UNITY_EDITOR
            rootPath =  Application.dataPath + "/TestingRootPath";
        #else
            rootPath = Application.persistentDataPath;
        #endif
        return rootPath;
    }
    
    public static string GetPacksRootPath() {
        string rootPath;
        #if UNITY_EDITOR
            rootPath =  Application.dataPath + "/TestingRootPath/TestingPacks";
        #else
            rootPath = $"{Application.persistentDataPath}/Packs";
        #endif
        if (!Directory.Exists(rootPath)) {
            Directory.CreateDirectory(rootPath);
        }

        return rootPath;
    }
}

public static class LoadedResource {
    private static Sprite _gamingBackground;
    private static AudioClip _gamingVoice;

    public static Sprite GamingBackground {
        get => _gamingBackground;
        set {
            if (value != _gamingBackground) {
                _gamingBackground = value;
                GameEvents.SendEventOnGamingBackgroundChanged(_gamingBackground);
            }
        }
    }

    public static AudioClip GamingVoice {
        get => _gamingVoice;
        set {
            if (value != _gamingVoice) {
                _gamingVoice = value;
                GameEvents.SendEventOnGamingVoiceChanged(_gamingVoice);
            }
        }
    }
}

public class ExternalResourceLoader {
    public static Sprite PageBackground;
    public static float BackgroundAspectRatio;
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

    public static IEnumerator LoadGamingBackground(string filePath) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture($"file://{filePath}");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            yield return null;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            yield return null;
            LoadedResource.GamingBackground = sprite;
        } else {
            Debug.LogError(www.error);
        }
    }

    public static Sprite LoadSpriteFormFile(string filePath) {
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
                Debug.LogError(ex);
                return null;
            }
        } else {
            Debug.LogError($"ERROR: 提供的文件{filePath}不是JSON文件");
            return null;
        }
    }

    public static void GetPageBackground() {
        if (Directory.Exists($"{PathManager.GetApplicationRootPath()}/UI/pageBackground")) {
            try {
                string uiPath = $"{PathManager.GetApplicationRootPath()}/UI/pageBackground";
                if (File.Exists($"{uiPath}/config.json")) {
                    string data = File.ReadAllText($"{uiPath}/config.json");
                    JObject config = JObject.Parse(data);
                    if (config["image"] != null) {
                        PageBackground = LoadSpriteFormFile($"{uiPath}/{config["image"]}");
                        float width = PageBackground.rect.width;
                        float height = PageBackground.rect.height;
                        BackgroundAspectRatio = width / height;
                    }
                }
            }catch (Exception ex) {
                Debug.Log(ex.Message);
                PageBackground = Resources.Load<Sprite>("Image/background/background");
                float width = PageBackground.rect.width;
                float height = PageBackground.rect.height;
                BackgroundAspectRatio = width / height;
            }
        } else {
            PageBackground = Resources.Load<Sprite>("Image/background/background");
            float width = PageBackground.rect.width;
            float height = PageBackground.rect.height;
            BackgroundAspectRatio = width / height;
        }
    }

    public void RefreshPackList() {
        OnRefreshPackList?.Invoke();
    }
}